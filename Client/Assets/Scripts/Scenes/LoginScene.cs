using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    UI_LoginScene _sceneUI;
    LoginInput _loginInput;


    private void Update()
    {
        // Keyboard event ����
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

        _sceneUI = Managers.UI.ShowSceneUI<UI_LoginScene>();
        _loginInput = GetComponent<LoginInput>();

        // TODO : URL ���� ������
        Managers.Web.BaseUrl = "https://localhost:5001/api";
        //Managers.Web.BaseUrl = "http://3.39.234.208:5000/api";  // AWS ec2 public ip

        // TODO : �ػ� ó��
        Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
    }

    public override void Clear()
    {

    }
}
