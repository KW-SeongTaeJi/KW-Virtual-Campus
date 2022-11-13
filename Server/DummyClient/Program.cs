using DummyClient.Session;
using NetworkCore;
using System;
using System.Net;
using System.Threading;

namespace DummyClient
{
    class Program
    {
        static int DummyClientCount { get; } = 300;

        static void Main(string[] args)
        {
            // 서버 대기
            Thread.Sleep(5000);

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = null;
            foreach (IPAddress ip in ipHost.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddr = ip;
                    break;
                }
            }
            //IPAddress gameIpAddr1 = IPAddress.Parse("13.125.207.171");  // AWS ec2 public ip
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 8000);

            Connector connector = new Connector();
            connector.Connect(endPoint, 
                () => { return SessionManager.Instance.Generate(); },
                DummyClientCount);

            while (true)
            {
                Thread.Sleep(500);
                SessionManager.Instance.DummyMove();
            }
        }
    }
}
