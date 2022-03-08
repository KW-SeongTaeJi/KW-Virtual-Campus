using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkCore
{
    public class Listener
    {
        Socket _socket;
        Func<Session> _sessionGenerator;


        public void Init(IPEndPoint endPoint, Func<Session> sessionGenerator, int register = 10, int backlog = 100)
        {
            _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionGenerator += sessionGenerator;

            _socket.Bind(endPoint);

            _socket.Listen(backlog);

            for (int i = 0; i < register; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptCompleted);
                RegisterAccept(args);
            }
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // empty past socket
            args.AcceptSocket = null;

            try
            {
                bool pending = _socket.AcceptAsync(args);

                // if accept synchronously
                if (pending == false)
                    AcceptCompleted(null, args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error : Fail to server accept {e}");
            }
        }
        void AcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError == SocketError.Success)
                {
                    Session session = _sessionGenerator.Invoke();
                    session.Start(args.AcceptSocket);
                    session.OnConnected();
                }
                else
                {
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error : Fail to complete accept {e}");
            }

            RegisterAccept(args);
        }
    }
}
