using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Server.DB
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

        public HairType HairType { get; set; }
        public FaceType FaceType { get; set; }
        public JacketType JacketType { get; set; }
        public HairColor HairColor { get; set; }
        public float FaceColor_X { get; set; }
        public float FaceColor_Y { get; set; }
        public float FaceColor_Z { get; set; }
    }
}
