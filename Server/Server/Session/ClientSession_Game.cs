using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using NetworkCore;
using Server.DB;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class ClientSession : PacketSession
    {
        public int AccountDbId { get; private set; }
        public Player MyPlayer { get; set; }

        // temp
        float _dummyPosZ = -48.0f;

        public void HandleEnterGame(C_EnterGame enterGamePacket)
        {
            

            // Find client information
            using (AppDbContext db = new AppDbContext())
            {
                AccountDb findAccount = db.Accounts
                    .Include(a => a.Player)
                    .ThenInclude(p => p.Friends)
                    .ThenInclude(f => f.Friend)
                    .ThenInclude(p => p.Account)
                    .AsNoTracking()
                    .Where(a => a.AccountId == enterGamePacket.AccountId)
                    .FirstOrDefault();

                if (findAccount != null)
                {
                    // Update token validation
                    using (RedisDb cache = new RedisDb())
                    {
                        cache.Set($"{enterGamePacket.AccountId}Where", "game");
                    }

                    // Load client player infomation
                    AccountDbId = findAccount.AccountDbId;
                    MyPlayer = ObjectManager.Instance.Add<Player>();
                    {
                        MyPlayer.Position.X = -37;
                        MyPlayer.Position.Y = 14;
                        MyPlayer.Position.Z = -37;
                        MyPlayer.RotationY = 0;
                        MyPlayer.Place = Place.Outdoor;
                        MyPlayer.PlayerDbId = findAccount.Player.PlayerDbId;
                        MyPlayer.Name = findAccount.Name;
                        MyPlayer.Session = this;
                        MyPlayer.HairType = findAccount.Player.HairType;
                        MyPlayer.FaceType = findAccount.Player.FaceType;
                        MyPlayer.JacketType = findAccount.Player.JacketType;
                        MyPlayer.HairColor = findAccount.Player.HairColor;
                        MyPlayer.FaceColor.X = findAccount.Player.FaceColor_X;
                        MyPlayer.FaceColor.Y = findAccount.Player.FaceColor_Y;
                        MyPlayer.FaceColor.Z = findAccount.Player.FaceColor_Z;
                    }
                    foreach (FriendRelationDb relation in findAccount.Player.Friends)
                    {
                        Vector3D faceColor = new Vector3D()
                        {
                            X = relation.Friend.FaceColor_X,
                            Y = relation.Friend.FaceColor_Y,
                            Z = relation.Friend.FaceColor_Z
                        };
                        PlayerInfo friend = new PlayerInfo()
                        {
                            Name = relation.Friend.Account.Name,
                            HairType = relation.Friend.HairType,
                            FaceType = relation.Friend.FaceType,
                            JacketType = relation.Friend.JacketType,
                            HairColor = relation.Friend.HairColor,
                            FaceColor = faceColor
                        };
                        MyPlayer.Friends.Add(friend.Name, friend);
                    }

                    GameWorld gameWorld = GameLogic.Instance.GameWorld;
                    gameWorld.PushQueue(gameWorld.EnterGame, MyPlayer);
                }
                // dummy client 처리용 temp
                else
                {
                    MyPlayer = ObjectManager.Instance.Add<Player>();
                    {
                        MyPlayer.Position.X = -48;
                        MyPlayer.Position.Y = 14;
                        MyPlayer.Position.Z = _dummyPosZ;
                        MyPlayer.RotationY = 180;
                        MyPlayer.Place = Place.Outdoor;
                        //MyPlayer.PlayerDbId = findAccount.Player.PlayerDbId;
                        MyPlayer.Name = enterGamePacket.AccountId;
                        MyPlayer.Session = this;
                        MyPlayer.HairType = HairType.HairOne;
                        MyPlayer.FaceType = FaceType.FaceOne;
                        MyPlayer.JacketType = JacketType.ComputerEngineering;
                        MyPlayer.HairColor = HairColor.Red;
                        MyPlayer.FaceColor.X = 0.5f;
                        MyPlayer.FaceColor.Y = 0.5f;
                        MyPlayer.FaceColor.Z = 0.5f;
                    }
                    _dummyPosZ -= 1;
                    GameWorld gameWorld = GameLogic.Instance.GameWorld;
                    gameWorld.PushQueue(gameWorld.EnterGameDummy, MyPlayer);
                }

                // TODO : DB에 정보 없을 때 오류 처리
            }
        }
    }
}
