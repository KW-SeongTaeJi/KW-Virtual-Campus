using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LobbyServer
{
    class SessionManager
    {
        #region Singleton
        static SessionManager _instance = new SessionManager();
        public static SessionManager Instance { get { return _instance; } }
        #endregion

        object _lock = new object();

        int _sessionId = 0;
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();


        public List<ClientSession> GetSessionList()
        {
            List<ClientSession> sessions = new List<ClientSession>();
            lock (_lock)
            {
                sessions = _sessions.Values.ToList();
            }
            return sessions;
        }

        public ClientSession GenerateSession()
        {
            lock (_lock)
            {
                int sessionId = ++_sessionId;

                ClientSession session = new ClientSession();
                session.SessionId = sessionId;
                _sessions.Add(sessionId, session);

                Console.WriteLine($"Connected ({_sessions.Count}) Players");

                return session;
            }
        }

        public ClientSession Find(int id)
        {
            lock (_lock)
            {
                ClientSession session = null;
                _sessions.TryGetValue(id, out session);
                return session;
            }
        }
        public ClientSession Find(ClientSession session)
        {
            return Find(session.SessionId);
        }

        public void Remove(ClientSession session)
        {
            lock (_lock)
            {
                _sessions.Remove(session.SessionId);
                Console.WriteLine($"Connected ({_sessions.Count}) Players");
            }
        }
    }
}
