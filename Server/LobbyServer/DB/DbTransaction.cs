using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LobbyServer.DB
{
    public class DbTransaction : JobSerializer
    {
        #region Singleton
        public static DbTransaction Instance { get; } = new DbTransaction();
        #endregion


        public static void EnterLobbyWork(ClientSession session, string accountId, string name)
        {
            if (session == null)
            {
                Console.WriteLine("Error from EnterLobbyWork()");
                return;
            }

            // Create new player's data
            AccountDb accountDb = new AccountDb()
            {
                AccountId = accountId,
                Name = name
            };
            PlayerDb playerDb = new PlayerDb()
            {
                Account = accountDb,
                HairType = HairType.NoHair,
                FaceType = FaceType.FaceOne,
                JacketType = JacketType.ComputerEngineering,
                HairColor = HairColor.Red,
                FaceColor_X = 1,
                FaceColor_Y = 1,
                FaceColor_Z = 1,
            };

            /* Push to DB thread (DB update) */
            Instance.PushQueue(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    db.Accounts.Add(accountDb);
                    db.Players.Add(playerDb);
                    bool success = db.SaveChangesEx();

                    /* Push to MainLogic thread (Save player info in memory and Send to client) */  
                    if (success)
                    {
                        Lobby lobby = MainLogic.Instance.Lobby;
                        lobby.PushQueue(lobby.EnterLobby_Step2, session, playerDb);
                    }
                }
            });
        }

        public static void CustermizeWork(ClientSession session, B_SaveCustermize packet)
        {
            if (session == null || packet == null)
            {
                Console.WriteLine("Error from CustermizeWork()");
                return;
            }

            // Update player's data
            PlayerDb playerDb = new PlayerDb();
            playerDb.PlayerDbId = session.MyPlayer.PlayerDbId;
            playerDb.HairType = packet.HairType;
            playerDb.FaceType = packet.FaceType;
            playerDb.JacketType = packet.JacketType;
            playerDb.HairColor = packet.HairColor;
            playerDb.FaceColor_X = packet.FaceColor.X;
            playerDb.FaceColor_Y = packet.FaceColor.Y;
            playerDb.FaceColor_Z = packet.FaceColor.Z;

            /* Push to DB thread (DB update) */
            Instance.PushQueue(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    db.Entry(playerDb).State = EntityState.Unchanged;
                    db.Entry(playerDb).Property(nameof(PlayerDb.HairType)).IsModified = true;
                    db.Entry(playerDb).Property(nameof(PlayerDb.FaceType)).IsModified = true;
                    db.Entry(playerDb).Property(nameof(PlayerDb.JacketType)).IsModified = true;
                    db.Entry(playerDb).Property(nameof(PlayerDb.HairColor)).IsModified = true;
                    db.Entry(playerDb).Property(nameof(PlayerDb.FaceColor_X)).IsModified = true;
                    db.Entry(playerDb).Property(nameof(PlayerDb.FaceColor_Y)).IsModified = true;
                    db.Entry(playerDb).Property(nameof(PlayerDb.FaceColor_Z)).IsModified = true;
                    bool success = db.SaveChangesEx();

                    /* Push to MainLogic thread (Send save ok to client) */
                    if (success)
                    {
                        Lobby lobby = MainLogic.Instance.Lobby;
                        lobby.PushQueue(lobby.HandleCustermize_Step2, session);
                    }
                }
            });
        }
    }
}
