using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkCore
{
    public class Connector
    {
        Func<Session> _sessionGenerator;


        public void Connect(IPEndPoint endPoint, Func<Session> sessionGenerator, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _sessionGenerator = sessionGenerator;

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectCompleted);
                args.RemoteEndPoint = endPoint;
                args.UserToken = socket;

                RegisterConnect(args);
            }
        }

        void RegisterConnect(SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;
            if (socket == null)
                return;

            try
            {
                bool pending = socket.ConnectAsync(args);

                // if connect synchronously
                if (pending == false)
                    ConnectCompleted(null, args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error : Fail to register connect {e}");
            }
        }
        void ConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError == SocketError.Success)
                {
                    Session session = _sessionGenerator.Invoke();
                    session.Start(args.ConnectSocket);
                    session.OnConnected();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error : Fail to complete connect {e}");
            }
        }
    }
}
