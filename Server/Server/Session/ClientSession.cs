using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public partial class ClientSession : PacketSession
    {
        public int SessionId { get; set; }

        object _lock = new object();

        List<ArraySegment<byte>> _sendQueue = new List<ArraySegment<byte>>();
        int _reservedBytes = 0;
        long _lastSendTick = 0;

        long _lastPingPongTick = 0;


        // 5s interval Ping signal to check connection
        public void Ping()
        {
            if (_lastPingPongTick > 0)
            {
                // if no pong signal in 60s, disconnect
                long delta = (System.Environment.TickCount64 - _lastPingPongTick);
                if (delta > 60 * 1000)
                {
                    Console.WriteLine();
                    Disconnect();
                    return;
                }
            }

            S_Ping pingPacket = new S_Ping();
            Send(pingPacket);

            GameLogic.Instance.PushTimer(5000, Ping);
        }
        public void HandlePong()
        {
            _lastPingPongTick = System.Environment.TickCount64;
        }

        public void Send(IMessage packet)
        {
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

            // queueing send packet first
            lock (_lock)
            {
                _sendQueue.Add(sendBuffer);
                _reservedBytes += sendBuffer.Length;
            }
        }
        public void FlushSendQueue()
        {
            List<ArraySegment<byte>> sendList = null;

            // send all packet in queue (0.1s interval or over 10000 bytes)
            lock (_lock)
            {
                long delta = (System.Environment.TickCount64 - _lastSendTick);
                if (delta < 100 && _reservedBytes < 10000)
                    return;

                sendList = _sendQueue;
                _sendQueue = new List<ArraySegment<byte>>();
                _reservedBytes = 0;
                _lastSendTick = System.Environment.TickCount64;
            }
            Send(sendList);
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnConnected()
        {
            S_Connected connectedPacket = new S_Connected();
            Send(connectedPacket);

            GameLogic.Instance.PushTimer(5000, Ping);
        }

        public override void OnDisconnected()
        {
            GameLogic.Instance.PushQueue(() =>
            {
                if (MyPlayer == null)
                    return;

                GameWorld gameWorld = GameLogic.Instance.GameWorld;
                gameWorld.PushQueue(gameWorld.LeaveGame, MyPlayer.Info.ObjectId);
            });

            SessionManager.Instance.Remove(this);
        }

        public override void OnSend()
        {

        }
    }
}
