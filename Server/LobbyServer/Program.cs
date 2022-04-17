using NetworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace LobbyServer
{
    class Program
    {
        static Listener _listener = new Listener();

        static void MainLogicTask()
        {
            while (true)
            {
                MainLogic.Instance.Update();
                Thread.Sleep(0);
            }
        }

        static void PacketSendTask()
        {
            while (true)
            {
                List<ClientSession> sessions = SessionManager.Instance.GetSessionList();
                foreach (ClientSession session in sessions)
                {
                    session.FlushSendQueue();
                }
                Thread.Sleep(0);
            }
        }

        static void Main(string[] args)
        {
            // TODO : 접속 주소 변경
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 4000);

            // N Threads : Receive Packet
            _listener.Init(endPoint, () => { return SessionManager.Instance.GenerateSession(); });
            Console.WriteLine("Listening...");

            // 1 Thread : NetworkTask
            {
                Thread t = new Thread(PacketSendTask);
                t.Name = "PacketSend";
                t.Start();
            }

            // Main Thread : MainLogicTask
            Thread.CurrentThread.Name = "MainLogic";
            MainLogicTask();
        }
    }
}
