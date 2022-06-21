using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        // TODO : 해상도 처리
        Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
    }

    public override void Clear()
    {

    }
}
