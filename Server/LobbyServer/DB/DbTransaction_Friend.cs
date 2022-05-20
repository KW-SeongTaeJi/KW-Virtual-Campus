using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LobbyServer.DB
{
    public partial class DbTransaction : JobSerializer
    {
        public static void AddFriendWork(ClientSession session, int myPlayerDbId, int friendDbId)
        {
            /* Push to DB thread (DB Add) */
            Instance.PushQueue(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    FriendRequestDb friendRequest = new FriendRequestDb()
                    {
                        FromDbId = myPlayerDbId,
                        ToDbId = friendDbId
                    };
                    db.FriendRequests.Add(friendRequest);
                    bool success = db.SaveChangesEx();

                    /* Push to MainLogic thread (Send success to client) */
                    if (success)
                    {
                        Lobby lobby = MainLogic.Instance.Lobby;
                        lobby.PushQueue(lobby.HandleAddFriend_Step2, session, 0);
                    }
                }
            });
        }

        public static void AcceptFriendWork(ClientSession session, B_AcceptFriend packet)
        {
            int myPlayerDbId = session.MyPlayer.PlayerDbId;
            int friendDbId;

            // Get friendDbId
            using (AppDbContext db = new AppDbContext())
            {
                AccountDb friend = db.Accounts
                    .Include(a => a.Player)
                    .AsNoTracking()
                    .Where(a => a.Name == packet.FriendName)
                    .FirstOrDefault();
                friendDbId = friend.Player.PlayerDbId;
            }

            /* Push to DB thread (DB Add and Delete) */
            Instance.PushQueue(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    // Remove friend request
                    FriendRequestDb findRequest = db.FriendRequests
                        .Where(f => f.FromDbId == friendDbId && f.ToDbId == myPlayerDbId)
                        .FirstOrDefault();
                    db.FriendRequests.Remove(findRequest);
                    bool success = db.SaveChangesEx();
                    if (!success)
                    {
                        return;
                    }

                    // if accept request, Add friend relation
                    if (packet.Accept)
                    {
                        List<FriendRelationDb> newRelation = new List<FriendRelationDb>()
                        {
                            new FriendRelationDb() { MeDbId = myPlayerDbId, FriendDbId = friendDbId },
                            new FriendRelationDb() { MeDbId = friendDbId, FriendDbId = myPlayerDbId }
                        };
                        db.FriendRelations.AddRange(newRelation);
                        success = db.SaveChangesEx();
                        /* Push to MainLogic thread (Send success to client) */
                        Lobby lobby = MainLogic.Instance.Lobby;
                        lobby.PushQueue(lobby.HandleAcceptFriend_Step2, session, success, true, packet.FriendName);
                    }
                    // if refuse request, nothing more
                    else
                    {
                        /* Push to MainLogic thread (Send success to client) */
                        Lobby lobby = MainLogic.Instance.Lobby;
                        lobby.PushQueue(lobby.HandleAcceptFriend_Step2, session, success, false, packet.FriendName);
                    }
                }
            });

        }

        public static void DeleteFriendWork(ClientSession session, B_DeleteFriend packet)
        {
            int myPlayerDbId = session.MyPlayer.PlayerDbId;
            int friendDbId;

            // Get friendDbId
            using (AppDbContext db = new AppDbContext())
            {
                AccountDb friend = db.Accounts
                    .Include(a => a.Player)
                    .AsNoTracking()
                    .Where(a => a.Name == packet.FriendName)
                    .FirstOrDefault();

                friendDbId = friend.Player.PlayerDbId;
            }

            /* Push to DB thread (DB delete) */
            Instance.PushQueue(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    List<FriendRelationDb> relations = db.FriendRelations
                        .AsNoTracking()
                        .Where(f => (f.MeDbId == myPlayerDbId && f.FriendDbId == friendDbId) || (f.MeDbId == friendDbId && f.FriendDbId == myPlayerDbId))
                        .ToList();

                    db.FriendRelations.RemoveRange(relations);
                    bool success = db.SaveChangesEx();

                    /* Push to MainLogic thread (Send success to client) */
                    Lobby lobby = MainLogic.Instance.Lobby;
                    lobby.PushQueue(lobby.HandleDeleteFriend_Step2, session, success, packet.FriendName);
                }
            });
        }
    }
}
