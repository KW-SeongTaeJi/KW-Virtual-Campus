using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameObject
    {
        public GameObjectType ObjectType { get; protected set; } = GameObjectType.None;

        public ObjectInfo ObjectInfo { get; set; } = new ObjectInfo();

        public int Id
        {
            get { return ObjectInfo.ObjectId; }
            set { ObjectInfo.ObjectId = value; }
        }
        public float RotationY
        {
            get { return ObjectInfo.RotationY; }
            set { ObjectInfo.RotationY = value; }
        }
        public Vector3D Position { get; set; } = new Vector3D();


        public GameObject()
        {
            ObjectInfo.Position = Position;
        }
    }
}
