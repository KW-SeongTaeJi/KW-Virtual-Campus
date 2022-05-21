using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendInfo
{
    public string Name { get; set; }

    #region Custermization Info
    public HairType HairType { get; set; }
    public FaceType FaceType { get; set; }
    public JacketType JacketType { get; set; }
    public HairColor HairColor { get; set; }
    public float FaceColor_X { get; set; }
    public float FaceColor_Y { get; set; }
    public float FaceColor_Z { get; set; }
    #endregion
}
