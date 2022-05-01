using Google.Protobuf.Protocol;
using LobbyServer.DB;
using NetworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer
{
    public partial class ClientSession : PacketSession
    {
        public Player MyPlayer { get; set; }


        public void HandleEnterLobby(B_EnterLobby packet)
        {
            using (RedisDb redisDb = new RedisDb())
            {
                // Compare authentication token from client and in Redis 
                string tokenKey = packet.AccountId;
                string tokenValue = redisDb.GetHash(tokenKey, "data");
                
                /* If valid access */  
                if (tokenValue == packet.Token)
                {
                    Lobby lobby = MainLogic.Instance.Lobby;
                    lobby.PushQueue(lobby.EnterLobby, this, packet.AccountId, packet.Name);
                }
                /* If invalid access */
                else
                {
                    L_EnterLobby enterPacket = new L_EnterLobby();
                    enterPacket.AccessOk = false;
                    Send(enterPacket);
                }
            }
        }
    }
}
