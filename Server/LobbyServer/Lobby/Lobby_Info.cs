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
        public void HandleCustermize(ClientSession session, B_SaveCustermize packet)
        {
            DbTransaction.CustermizeWork(session, packet);
        }
        public void HandleCustermize_Step2(ClientSession session, B_SaveCustermize packet)
        {
            session.MyPlayer.HairType = packet.HairType;
            session.MyPlayer.FaceType = packet.FaceType;
            session.MyPlayer.JacketType = packet.JacketType;
            session.MyPlayer.HairColor = packet.HairColor;
            session.MyPlayer.FaceColor_X = packet.FaceColor.X;
            session.MyPlayer.FaceColor_Y = packet.FaceColor.Y;
            session.MyPlayer.FaceColor_Z = packet.FaceColor.Z;

            L_SaveCustermize custermizePacket = new L_SaveCustermize();
            custermizePacket.SaveOk = true;
            session.Send(custermizePacket);
        }

        public void HandleSaveInfo(ClientSession session, B_SaveInfo packet)
        {
            // Check same name already exist
            using (AppDbContext db = new AppDbContext())
            {
                AccountDb findAccount = db.Accounts
                    .AsNoTracking()
                    .Where(a => a.Name == packet.Name)
                    .FirstOrDefault();
                /* ErrorCode 1 : same name already exist */
                if (findAccount != null && packet.Name != session.MyPlayer.Name)
                {
                    HandleSaveInfo_Step2(session, packet, 1);
                    return;
                }
            }
            // Check password is correct if want to change password
            if (packet.Password != "")
            {
                using (WebDbContext webDb = new WebDbContext())
                {
                    UserAccountDb findAccount = webDb.UserAccounts
                        .AsNoTracking()
                        .Where(a => a.AccountId == session.MyPlayer.AccountId)
                        .FirstOrDefault();
                    /* ErrorCode 2 : incorrect password */
                    if (findAccount.Password != packet.Password)
                    {
                        HandleSaveInfo_Step2(session, packet, 2);
                        return;
                    }
                }
            }
            // Condition is OK
            DbTransaction.SaveInfoWork(session, packet);
        }
        public void HandleSaveInfo_Step2(ClientSession session, B_SaveInfo packet, int errorCode)
        {
            L_SaveInfo saveInfoPacket = new L_SaveInfo();
            saveInfoPacket.ErrorCode = errorCode;
            // Save info success
            if (errorCode == 0)
            {
                saveInfoPacket.SaveOk = true;
                saveInfoPacket.Name = packet.Name;
                session.MyPlayer.Name = packet.Name;
            }
            // Save info fail
            else
            {
                saveInfoPacket.SaveOk = false;
            }
            session.Send(saveInfoPacket);
        }
    }
}
