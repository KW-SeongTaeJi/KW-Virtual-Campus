using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameObject
    {
        public GameObjectType ObjectType { get; protected set; } = GameObjectType.None;

        public ObjectInfo Info { get; set; } = new ObjectInfo();
        public Vector3D Position { get; set; } = new Vector3D();

        public int Id
        {
            get { return Info.ObjectId; }
            set { Info.ObjectId = value; }
        }
        public float RotationY
        {
            get { return Info.RotationY; }
            set { Info.RotationY = value; }
        }


        public GameObject()
        {
            Info.Position = Position;
        }
    }
}
