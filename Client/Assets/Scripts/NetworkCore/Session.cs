using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkCore
{
    public abstract class Session
    {
        object _lock = new object();

        int _isDisconnect = 1;

        Socket _socket;

        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();

        RecvBuffer _recvBuff = new RecvBuffer(65536);
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();

        // functions when connect, disconnect, send, recv
        public abstract void OnConnected();
        public abstract void OnDisconnected();
        public abstract void OnSend();
        public abstract int OnRecv(ArraySegment<byte> buffer);

        public void Start(Socket socket)
        {
            _isDisconnect = 0;

            _socket = socket;
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SendCompleted);
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(RecvCompleted);

            RegisterRecv();
        }

        void Clear()
        {
            lock (_lock)
            {
                _sendQueue.Clear();
                _pendingList.Clear();
            }
        }
        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _isDisconnect, 1) == 1)
                return;

            OnDisconnected();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            Clear();
        }

        public void Send(ArraySegment<byte> sendBuff)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(sendBuff);
                if (_pendingList.Count == 0)
                    RegisterSend();
            }
        }
        public void Send(List<ArraySegment<byte>> sendBuffList)
        {
            if (sendBuffList.Count == 0)
                return;

            lock (_lock)
            {
                foreach (ArraySegment<byte> sendBuff in sendBuffList)
                    _sendQueue.Enqueue(sendBuff);
                if (_pendingList.Count == 0)
                    RegisterSend();
            }
        }

        #region network send/recv process
        void RegisterSend()
        {
            if (_isDisconnect == 1)
                return;

            // sendQueue -> pendinglist
            while (_sendQueue.Count > 0)
            {
                ArraySegment<byte> buff = _sendQueue.Dequeue();
                _pendingList.Add(buff);
            }
            _sendArgs.BufferList = _pendingList;

            try
            {
                bool pending = _socket.SendAsync(_sendArgs);

                // if send synchronously
                if (pending == false)
                    SendCompleted(null, _sendArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error : Fail to register sendBuff. {e}");
            }
        }
        void SendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null;
                        _pendingList.Clear();

                        OnSend();

                        if (_sendQueue.Count > 0)
                            RegisterSend();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error :  Fail to complete send {e}");
                    }
                }
                else
                {
                    // if some problem, just disconnect.
                    Disconnect();
                }
            }
        }

        void RegisterRecv()
        {
            if (_isDisconnect == 1)
                return;

            _recvBuff.Clean();
            ArraySegment<byte> segment = _recvBuff.WriteSegment;
            _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

            try
            {
                bool pending = _socket.ReceiveAsync(_recvArgs);

                // if recv synchronously
                if (pending == false)
                    RecvCompleted(null, _recvArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error : Fail to register recvBuff. {e}");
            }
        }
        void RecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    if (_recvBuff.OnWrite(args.BytesTransferred) == false)
                    {
                        // if no free space in recv buffer,
                        Disconnect();
                        return;
                    }

                    int processSize = OnRecv(_recvBuff.ReadSegment);
                    if (processSize < 0 || _recvBuff.DataSize < processSize)
                    {
                        Disconnect();
                        return;
                    }

                    if (_recvBuff.OnRead(processSize) == false)
                    {
                        Disconnect();
                        return;
                    }

                    RegisterRecv();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error :  Fail to complete recv {e}");
                }
            }
            else
            {
                // if some problem, just disconnect.
                Disconnect();
            }
        }
        #endregion
    }
}
