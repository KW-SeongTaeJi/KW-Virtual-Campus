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

        //Managers.Web.BaseUrl = "https://localhost:5001/api";
        Managers.Web.BaseUrl = "http://13.125.241.14:5000/api";  // AWS ec2 public ip

        int curWidth = Screen.width;
        int curHeight = Screen.height;
        Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
        _sceneUI = Managers.UI.ShowSceneUI<UI_LoginScene>();
        Screen.SetResolution(curWidth, curHeight, Screen.fullScreenMode);

        _loginInput = GetComponent<LoginInput>();
    }

    public override void Clear()
    {

    }
}
