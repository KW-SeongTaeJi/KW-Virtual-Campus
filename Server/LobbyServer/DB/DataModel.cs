using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LobbyServer.DB
{
    [Table("Account")]
    public class AccountDb
    {
        public int AccountDbId { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public PlayerDb Player { get; set; }
    }

    [Table("Player")]
    public class PlayerDb
    {
        public int PlayerDbId { get; set; }

        [ForeignKey("Account")]
        public int AccountDbId { get; set; }
        public AccountDb Account { get; set; }

        [InverseProperty("Me")]
        public ICollection<FriendRelationDb> Friends { get; set; }

        [InverseProperty("To")]
        public ICollection<FriendRequestDb> Requests { get; set; }

        public HairType HairType { get; set; }
        public FaceType FaceType { get; set; }
        public JacketType JacketType { get; set; }
        public HairColor HairColor { get; set; }
        public float FaceColor_X { get; set; }
        public float FaceColor_Y { get; set; }
        public float FaceColor_Z { get; set; }
    }

    [Table("FriendRelation")]
    public class FriendRelationDb
    {
        public int FriendRelationDbId { get; set; }

        [ForeignKey("Me")]
        public int MeDbId { get; set; }
        public PlayerDb Me { get; set; }

        [ForeignKey("Friend")]
        public int FriendDbId { get; set; }
        public PlayerDb Friend { get; set; }
    }

    [Table("FriendRequest")]
    public class FriendRequestDb
    {
        public int FriendRequestDbId { get; set; }

        [ForeignKey("From")]
        public int FromDbId { get; set; }
        public PlayerDb From { get; set; }

        [ForeignKey("To")]
        public int ToDbId { get; set; }
        public PlayerDb To { get; set; }
    }
}
