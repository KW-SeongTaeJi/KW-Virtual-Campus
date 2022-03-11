using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        // Set Scene UI Canvas
        Managers.UI.SetCanvas(gameObject, sort: false);
    }
}
