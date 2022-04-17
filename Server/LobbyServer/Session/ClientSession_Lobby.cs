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
        public void HandleEnterLobby(B_EnterLobby packet)
        {
            using (RedisDb redisDb = new RedisDb())
            {
                string tokenKey = packet.AccountId;
                string tokenValue = redisDb.GetHash(tokenKey, "data");

                L_EnterLobby enterPacket = new L_EnterLobby();
                if (tokenValue == packet.Token)
                {
                    enterPacket.AccessOk = true;
                }
                else
                {
                    enterPacket.AccessOk = false;
                }

                Send(enterPacket);
            }
        }
    }
}
