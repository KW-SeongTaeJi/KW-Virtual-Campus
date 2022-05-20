using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _sceneUI;

    MyPlayerInput _input;


    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Screen.SetResolution(1920, 1080, Screen.fullScreenMode);

        _sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();

        _input = GetComponent<MyPlayerInput>();
    }

    public override void Clear()
    {

    }
}
