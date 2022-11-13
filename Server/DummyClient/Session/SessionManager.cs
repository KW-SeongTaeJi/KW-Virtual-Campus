using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace DummyClient.Session
{
    class SessionManager
    {
        public static SessionManager Instance { get; } = new SessionManager();

        HashSet<ServerSession> _sessions = new HashSet<ServerSession>();
        object _lock = new object();
        int _dummyId = 1;

        float _dummyPosZ = -48.0f;

        public ServerSession Generate()
        {
            lock (_lock)
            {
                ServerSession session = new ServerSession();
                session.DummyId = _dummyId;
                _dummyId++;

                session.PosZ = _dummyPosZ;
                _dummyPosZ--;

                _sessions.Add(session);
                Console.WriteLine($"Connected ({_sessions.Count}) Players");
                return session;
            }
        }

        public void Remove(ServerSession session)
        {
            lock (_lock)
            {
                _sessions.Remove(session);
                Console.WriteLine($"Connected ({_sessions.Count}) Players");
            }
        }

        public void DummyMove()
        {
            foreach (ServerSession session in _sessions)
            {
                C_Move movePacket = new C_Move();
                movePacket.Position = new Vector3D();
                movePacket.Position.X = session.PosX;
                movePacket.Position.Y = session.PosY;
                movePacket.Position.Z = session.PosZ;
                movePacket.RotationY = 0f;
                movePacket.TargetSpeed = 2.0f;
                movePacket.TargetRotation = 180.0f;
                movePacket.Jump = false;
                session.Send(movePacket);

                session.PosZ--;
            }
        }
    }
}
