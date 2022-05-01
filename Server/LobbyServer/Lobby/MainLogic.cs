using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer
{
    public class MainLogic : JobSerializer
    {
        #region Singleton
        static MainLogic _instance = new MainLogic();
        public static MainLogic Instance { get { return _instance; } }
        #endregion

        public Lobby Lobby { get; } = new Lobby(); 


        public void Update()
        {
            Flush();
            Lobby.Flush();
        }
    }
}
