using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class Player : GameObject
    {
        public int PlayerDbId { get; set; }
        public ClientSession Session { get; set; }

        public PlayerInfo PlayerInfo { get; set; } = new PlayerInfo();

        public string Name
        {
            get { return PlayerInfo.Name; }
            set { PlayerInfo.Name = value; }
        }
        public HairType HairType
        {
            get { return PlayerInfo.HairType; }
            set { PlayerInfo.HairType = value; }
        }
        public FaceType FaceType
        {
            get { return PlayerInfo.FaceType; }
            set { PlayerInfo.FaceType = value; }
        }
        public JacketType JacketType
        {
            get { return PlayerInfo.JacketType; }
            set { PlayerInfo.JacketType = value; }
        }
        public HairColor HairColor
        {
            get { return PlayerInfo.HairColor; }
            set { PlayerInfo.HairColor = value; }
        }
        public Place Place
        {
            get { return PlayerInfo.Place; }
            set { PlayerInfo.Place = value; }
        }
        public Vector3D FaceColor { get; set; } = new Vector3D();

        public Dictionary<string, PlayerInfo> Friends { get; set; } = new Dictionary<string, PlayerInfo>();


        public Player()
        {
            ObjectType = GameObjectType.Player;
            PlayerInfo.FaceColor = FaceColor;
        }
    }
}
