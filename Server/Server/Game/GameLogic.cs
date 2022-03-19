using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameLogic : JobSerializer
    {
        #region Singleton
        static GameLogic _instance = new GameLogic();
        public static GameLogic Instance { get { return _instance; } }
        #endregion

        public GameWorld GameWorld { get; } = new GameWorld();


        public void Update()
        {
            Flush();
            GameWorld.Update();
        }
    }
}
