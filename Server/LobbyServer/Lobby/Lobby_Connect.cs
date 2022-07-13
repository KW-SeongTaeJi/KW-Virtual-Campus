using Google.Protobuf.Protocol;
using LobbyServer.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LobbyServer
{
    public partial class Lobby : JobSerializer
    {
        public void EnterLobby(ClientSession session, string accountId, string name)
        {
            // Update token validation
            using (RedisDb redisDb = new RedisDb())
            {
                redisDb.Set($"{accountId}Where", "lobby");
            }

            using (AppDbContext db = new AppDbContext())
            {
                // Find player infomation 
                PlayerDb findPlayer = db.Players
                    .Include(p => p.Account)
                    .AsNoTracking()
                    .Where(p => p.Account.AccountId == accountId)
                    .FirstOrDefault();

                /* new player in DB */
                if (findPlayer == null)
                {
                    DbTransaction.EnterLobbyWork(session, accountId, name);
                }
                /* existing player in DB */
                else
                {
                    EnterLobby_Step2(session, findPlayer);
                }
            }
        }
        public void EnterLobby_Step2(ClientSession session, PlayerDb playerDb)
        {
            // Save player infomation in memory
            Player newPlayer = new Player()
            {
                PlayerDbId = playerDb.PlayerDbId,
                AccountId = playerDb.Account.AccountId,
                Name = playerDb.Account.Name,
                Session = session,
                HairType = playerDb.HairType,
                FaceType = playerDb.FaceType,
                JacketType = playerDb.JacketType,
                HairColor = playerDb.HairColor,
                FaceColor_X = playerDb.FaceColor_X,
                FaceColor_Y = playerDb.FaceColor_Y,
                FaceColor_Z = playerDb.FaceColor_Z
            };
            session.MyPlayer = PlayerManager.Instance.Add(newPlayer);

            /* if same player already entered */
            if (session.MyPlayer == null)
            {
                L_EnterLobby enterPacket = new L_EnterLobby();
                enterPacket.AccessOk = false;
                session.Send(enterPacket);
                Console.WriteLine("EnterLobby : already entered player");
            }
            /* enter success */
            else
            {
                // Send player information to client
                L_EnterLobby enterPacket = new L_EnterLobby()
                {
                    AccessOk = true,
                    HairType = newPlayer.HairType,
                    FaceType = newPlayer.FaceType,
                    JacketType = newPlayer.JacketType,
                    HairColor = newPlayer.HairColor,
                    FaceColor = newPlayer.FaceColor
                };
                session.Send(enterPacket);
            }
        }

        public void LeaveLobby(int playerDbId, string accountId)
        {
            // Remove client player
            if (PlayerManager.Instance.Remove(playerDbId) == false)
                return;

            // Update token validation
            using (RedisDb redisDb = new RedisDb())
            {
                redisDb.Set($"{accountId}Where", "end");
            }

            // TODO : 종료 전 DB 저장 및 패킷전송
        }
    }
}
