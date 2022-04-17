using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    UI_LobbyScene _sceneUI;


    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Lobby;

        Screen.SetResolution(1280, 640, false);

        _sceneUI = Managers.UI.ShowSceneUI<UI_LobbyScene>();
    }

    public override void Clear()
    {

    }
}
