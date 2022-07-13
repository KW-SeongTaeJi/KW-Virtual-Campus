using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    UI_LobbyScene _sceneUI;

    public Player PlayerInfo { get; set; }


    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Lobby;

        _sceneUI = Managers.UI.ShowSceneUI<UI_LobbyScene>();

        // TODO : 해상도 처리
        //Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
    }

    public override void Clear()
    {

    }
}
