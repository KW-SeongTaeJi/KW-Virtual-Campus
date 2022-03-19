using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameObject
    {
        public GameObjectType ObjectType { get; protected set; } = GameObjectType.None;

        public GameWorld World { get; set; }

        public ObjectInfo Info { get; set; } = new ObjectInfo();
        public PositionInfo PosInfo { get; private set; } = new PositionInfo();

        public int Id
        {
            get { return Info.ObjectId; }
            set { Info.ObjectId = value; }
        }


        public GameObject()
        {
            Info.PosInfo = PosInfo;
        }
    }
}
