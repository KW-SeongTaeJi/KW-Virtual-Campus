using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameWorld : JobSerializer
    {
        Dictionary<int, Player> _players = new Dictionary<int, Player>();


        public void Update()
        {
            Flush();
        }

        public void EnterGame(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            GameObjectType type = ObjectManager.GetObjectTypeById(gameObject.Id);

            // Get my player and other players
            if (type == GameObjectType.Player)
            {
                Player myPlayer = gameObject as Player;
                _players.Add(gameObject.Id, myPlayer);
                myPlayer.World = this;

                S_EnterGame enterPacket = new S_EnterGame();
                enterPacket.MyPlayer = myPlayer.Info;
                myPlayer.Session.Send(enterPacket);

                S_Spawn spawnPacket = new S_Spawn();
                foreach (Player player in _players.Values)
                {
                    if (player.PlayerDbId != myPlayer.PlayerDbId)
                        spawnPacket.Objects.Add(player.Info);
                }
                myPlayer.Session.Send(spawnPacket);
            }

            // Boradcast my player to other players
            {
                S_Spawn spawnPacket = new S_Spawn();
                spawnPacket.Objects.Add(gameObject.Info);
                Broadcast(spawnPacket);
            }
        }

        public void LeaveGame()
        {

        }

        public void Broadcast(IMessage packet)
        {
            foreach (Player player in _players.Values)
            {
                player.Session.Send(packet);
            }
        }
    }
}
