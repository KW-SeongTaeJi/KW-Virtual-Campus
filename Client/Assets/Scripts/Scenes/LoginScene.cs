using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    UI_LoginScene _sceneUI;

    LoginInput _loginInput;


    private void Update()
    {
        // Call input Key Event
        if (Managers.UI.PopupUIStack.Count == 0)
            _sceneUI.HandleKeyEvent(_loginInput.Tap, _loginInput.Enter);
        else
        {
            UI_Popup popup = Managers.UI.PopupUIStack.Peek();
            if (popup is UI_RegisterPopup)
                ((UI_RegisterPopup)popup).HandleKeyEvent(_loginInput.Tap);
            else if (popup is UI_AlertPopup)
                ((UI_AlertPopup)popup).HandleKeyEvent(_loginInput.Enter);
        }
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        // TODO : url check
        Managers.Web.BaseUrl = "https://localhost:5001/api";

        Screen.SetResolution(1280, 640, false);

        _sceneUI = Managers.UI.ShowSceneUI<UI_LoginScene>();

        _loginInput = GetComponent<LoginInput>();
    }

    public override void Clear()
    {

    }
}
