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


        public void HandleEnterGame(C_EnterGame enterGamePacket)
        {
            using (AppDbContext db = new AppDbContext())
            {
                AccountDb findAccount = db.Accounts
                    .Include(a => a.Player)
                    .AsNoTracking()
                    .Where(a => a.AccountId == enterGamePacket.AccountId)
                    .FirstOrDefault();

                if (findAccount != null)
                {
                    // Load client player infomation
                    AccountDbId = findAccount.AccountDbId;
                    MyPlayer = ObjectManager.Instance.Add<Player>();
                    {
                        MyPlayer.Position.X = -37;
                        MyPlayer.Position.Y = 14;
                        MyPlayer.Position.Z = -37;
                        MyPlayer.RotationY = 0;
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
                    GameWorld gameWorld = GameLogic.Instance.GameWorld;
                    gameWorld.PushQueue(gameWorld.EnterGame, MyPlayer);
                }

                // TODO : DB에 정보 없을 때 오류 처리
            }
        }
    }
}
