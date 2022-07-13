using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorScene_Ogui : BaseScene
{
    UI_IndoorScene _sceneUI;

    MyIndoorPlayerInput _input;


    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.IndoorOgui;

        _sceneUI = Managers.UI.ShowSceneUI<UI_IndoorScene>();

        _input = GetComponent<MyIndoorPlayerInput>();
    }

    public override void Clear()
    {

    }
}
