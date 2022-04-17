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


        public void Update()
        {
            Flush();
        }
    }
}
