using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        // TODO : �ػ� ó��
        Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
    }

    public override void Clear()
    {

    }
}
