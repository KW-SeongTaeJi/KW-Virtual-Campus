using Google.Protobuf.Protocol;
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

        _sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();
        _input = GetComponent<MyPlayerInput>();

        // TODO : 해상도 처리
        //Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
    }

    public override void Clear()
    {

    }
}
