using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorScene_Library : BaseScene
{
    UI_IndoorScene _sceneUI;

    MyIndoorPlayerInput _input;


    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.IndoorLibrary;

        Screen.SetResolution(1920, 1080, Screen.fullScreenMode);

        _sceneUI = Managers.UI.ShowSceneUI<UI_IndoorScene>();

        _input = GetComponent<MyIndoorPlayerInput>();
    }

    public override void Clear()
    {

    }
}
