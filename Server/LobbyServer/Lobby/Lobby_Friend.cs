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
        public void HandleFriendList(ClientSession session)
        {
            Player myPlayer = session.MyPlayer;
            myPlayer.Friends.Clear();

            L_FriendList friendListPacket = new L_FriendList();

            using (AppDbContext db = new AppDbContext())
            {
                List<FriendRelationDb> friends = db.FriendRelations
                    .Include(f => f.Friend)
                    .Include(f => f.Friend.Account)
                    .AsNoTracking()
                    .Where(f => f.MeDbId == myPlayer.PlayerDbId)
                    .ToList();

                foreach (FriendRelationDb friend in friends)
                {
                    myPlayer.Friends.Add(friend.Friend.Account.Name, friend.Friend);

                    Vector3D faceColor = new Vector3D();
                    faceColor.X = friend.Friend.FaceColor_X;
                    faceColor.Y = friend.Friend.FaceColor_X;
                    faceColor.Z = friend.Friend.FaceColor_X;
                    PlayerInfo playerInfo = new PlayerInfo()
                    {
                        Name = friend.Friend.Account.Name,
                        HairType = friend.Friend.HairType,
                        FaceType = friend.Friend.FaceType,
                        JacketType = friend.Friend.JacketType,
                        HairColor = friend.Friend.HairColor,
                        FaceColor = faceColor
                    };
                    friendListPacket.Friends.Add(playerInfo);
                }
            }
            session.Send(friendListPacket);
        }

        public void HandleAddFriend(ClientSession session, B_AddFriend packet)
        {
            using (AppDbContext db = new AppDbContext())
            {
                // Check name exist
                AccountDb findPlayer = db.Accounts
                    .Include(a => a.Player)
                    .AsNoTracking()
                    .Where(a => a.Name == packet.FriendName)
                    .FirstOrDefault();
                if (findPlayer == null)
                {
                    HandleAddFriend_Step2(session, 1);
                    return;
                }

                // Check already friend
                int myPlayerDbId = session.MyPlayer.PlayerDbId;
                int friendDbId = findPlayer.Player.PlayerDbId;
                FriendRelationDb findRelation = db.FriendRelations
                    .AsNoTracking()
                    .Where(f => f.MeDbId == myPlayerDbId && f.FriendDbId == friendDbId)
                    .FirstOrDefault();
                if (findRelation != null)
                {
                    HandleAddFriend_Step2(session, 2);
                    return;
                }

                // Check already request 
                FriendRequestDb findRequest = db.FriendRequests
                    .AsNoTracking()
                    .Where(f => (f.ToDbId == myPlayerDbId && f.FromDbId == friendDbId) || (f.ToDbId == friendDbId && f.FromDbId == myPlayerDbId))
                    .FirstOrDefault();
                if (findRequest != null)
                {
                    HandleAddFriend_Step2(session, 3);
                    return;
                }

                /* all condition is OK */
                DbTransaction.AddFriendWork(session, myPlayerDbId, friendDbId);
            }
        }
        public void HandleAddFriend_Step2(ClientSession session, int errorCode)
        {
            L_AddFriend addFriendPacket = new L_AddFriend();
            addFriendPacket.ErrorCode = errorCode;

            if (errorCode == 0)
                addFriendPacket.Success = true;
            else
                addFriendPacket.Success = false;

            session.Send(addFriendPacket);
        }

        public void HandleFriendRequestList(ClientSession session, B_FriendRequestList packet)
        {
            L_FriendRequestList friendRequestListPacket = new L_FriendRequestList();

            // Get friend request to client player
            using (AppDbContext db = new AppDbContext())
            {
                List<FriendRequestDb> requestList = db.FriendRequests
                    .Include(f => f.From)
                    .Include(f => f.From.Account)
                    .AsNoTracking()
                    .Where(f => f.ToDbId == session.MyPlayer.PlayerDbId)
                    .ToList();

                foreach (FriendRequestDb request in requestList)
                    friendRequestListPacket.FriendNames.Add(request.From.Account.Name);
            }

            session.Send(friendRequestListPacket);
        }

        public void HandleAcceptFriend(ClientSession session, B_AcceptFriend packet)
        {
            DbTransaction.AcceptFriendWork(session, packet);
        }
        public void HandleAcceptFriend_Step2(ClientSession session, bool success, bool accept, string name)
        {
            L_AcceptFriend acceptFriendPacket = new L_AcceptFriend()
            {
                Success = success,
                Accept = accept
            };

            if (success)
            {
                PlayerDb friend;
                using (AppDbContext db = new AppDbContext())
                {
                    friend = db.Players
                        .Include(p => p.Account)
                        .AsNoTracking()
                        .Where(p => p.Account.Name == name)
                        .FirstOrDefault();
                }

                PlayerInfo friendInfo = new PlayerInfo();
                if (accept)
                {
                    friendInfo.Name = friend.Account.Name;
                    friendInfo.HairType = friend.HairType;
                    friendInfo.FaceType = friend.FaceType;
                    friendInfo.JacketType = friend.JacketType;
                    friendInfo.HairColor = friend.HairColor;
                    Vector3D faceColor = new Vector3D();
                    faceColor.X = friend.FaceColor_X;
                    faceColor.Y = friend.FaceColor_Y;
                    faceColor.Z = friend.FaceColor_Z;
                    friendInfo.FaceColor = faceColor;
                }
                else
                {
                    friendInfo.Name = friend.Account.Name;
                }
                acceptFriendPacket.Friend = friendInfo;
            }
            session.Send(acceptFriendPacket);
        }

        public void HandleDeleteFriend(ClientSession session, B_DeleteFriend packet)
        {
            DbTransaction.DeleteFriendWork(session, packet);
        }
        public void HandleDeleteFriend_Step2(ClientSession session, bool success, string name)
        {
            L_DeleteFriend deleteFriendPacket = new L_DeleteFriend()
            {
                Success = success,
                FriendName = name
            };
            session.Send(deleteFriendPacket);
        }
    }
}
