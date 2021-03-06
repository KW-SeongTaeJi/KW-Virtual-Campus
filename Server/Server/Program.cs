using NetworkCore;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();

        static void GameLogicTask()
        {
            while (true)
            {
                GameLogic.Instance.Update();
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
            // Set localhost ipv4 address
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = null;
            foreach (IPAddress ip in ipHost.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddr = ip;
                    Console.WriteLine(ip);
                    break;
                }
            }
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 8000);

            // N Threads : Receive Packet
            _listener.Init(endPoint, () => { return SessionManager.Instance.GenerateSession(); });
            Console.WriteLine("Listening...");

            // 1 Thread : NetworkTask
            {
                Thread t = new Thread(PacketSendTask);
                t.Name = "PacketSend";
                t.Start();
            }

            // Main Thread : GameLogicTask
            Thread.CurrentThread.Name = "GameLogic";
            GameLogicTask();
        }
    }
}
