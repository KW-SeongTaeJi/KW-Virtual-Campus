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
                    AccountDbId = findAccount.AccountDbId;
                    MyPlayer = ObjectManager.Instance.Add<Player>();
                    {
                        // TODO : 초기 플레이어 접속 정보 설정
                        MyPlayer.Info.Name = findAccount.Name;
                        MyPlayer.Info.Position.X = -37;
                        MyPlayer.Info.Position.Y = 14;
                        MyPlayer.Info.Position.Z = -37;
                        MyPlayer.Info.RotationY = 0;
                        MyPlayer.PlayerDbId = findAccount.Player.PlayerDbId;
                        MyPlayer.Session = this;
                    }
                }

                GameWorld gameWorld = GameLogic.Instance.GameWorld;
                gameWorld.PushQueue(gameWorld.EnterGame, MyPlayer);
            }
        }
    }
}
