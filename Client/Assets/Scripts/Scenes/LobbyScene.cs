using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    UI_LobbyScene _sceneUI;

    public PlayerInfo PlayerInfo { get; set; }


    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Lobby;

        int curWidth = Screen.width;
        int curHeight = Screen.height;
        Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
        _sceneUI = Managers.UI.ShowSceneUI<UI_LobbyScene>();
        Screen.SetResolution(curWidth, curHeight, Screen.fullScreenMode);
    }

    public override void Clear()
    {

    }
}
