using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer
{
    public class Player
    {
        public int PlayerDbId { get; set; }
        public ClientSession Session { get; set; }

        #region Custermization Info
        public HairType HairType { get; set; }
        public FaceType FaceType { get; set; }
        public JacketType JacketType { get; set; }
        public HairColor HairColor { get; set; }
        public Vector3D FaceColor { get; set; } = new Vector3D();
        public float FaceColor_X { get { return FaceColor.X; } set { FaceColor.X = value; } }
        public float FaceColor_Y { get { return FaceColor.Y; } set { FaceColor.Y = value; } }
        public float FaceColor_Z { get { return FaceColor.Z; } set { FaceColor.Z = value; } }
        #endregion
    }
}
