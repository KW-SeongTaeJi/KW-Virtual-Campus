using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer
{
    public class PlayerManager
    {
        #region Singleton
        static PlayerManager _instance = new PlayerManager();
        public static PlayerManager Instance { get { return _instance; } }
        #endregion

        Dictionary<int, Player> _players = new Dictionary<int, Player>();

        object _lock = new object();


        public Player Add(int playerDbId)
        {
            if (_players.ContainsKey(playerDbId))
                return null;

            Player player = new Player();
            player.PlayerDbId = playerDbId;

            lock (_lock)
            {
                _players.Add(playerDbId, player);
            }
            return player;
        }
        public Player Add(Player player)
        {
            if (_players.ContainsKey(player.PlayerDbId))
                return null;
         
            lock (_lock)
            {
                _players.Add(player.PlayerDbId, player);
            }
            return player;
        }

        public bool Remove(int playerDbId)
        {
            lock (_lock)
            {
                return _players.Remove(playerDbId);
            }
        }

        public Player Find(int playerDbId)
        {
            lock (_lock)
            {
                Player player = null;
                if (_players.TryGetValue(playerDbId, out player))
                    return player;
            }
            return null;
        }
    }
}
