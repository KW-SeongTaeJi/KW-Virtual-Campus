using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LoginScene : UI_Scene
{
    UI_AlertPopup _alertPopup;
    UI_LoadingCirclePopup _loadingPopup;

    // each name of components to bind
    enum InputFields
    {
        Id,
        Password
    }
    enum Buttons
    {
        LoginButton,
        RegisterButton
    }


    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.LoginButton).gameObject.BindEvent(OnClickLoginButton, Define.UIEvent.Click);
        GetButton((int)Buttons.RegisterButton).gameObject.BindEvent(OnClickRegisterButton, Define.UIEvent.Click);
    }

    public void OnClickLoginButton(PointerEventData evt)
    {
        // Show Loading UI
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("로그인 중");

        string id = GetInputField((int)InputFields.Id).text;
        string password = GetInputField((int)InputFields.Password).text;

        // TODO : PW 해시 처리

        // Send Login Packet to Account Server
        LoginPacketReq packet = new LoginPacketReq()
        {
            AccountId = id,
            Password = password
        };
        Managers.Web.SendPostRequest<LoginPakcetRes>("account/login", packet, OnRecvLoginPacket);
    }
    void OnRecvLoginPacket(LoginPakcetRes res)
    {
        // Close Loading Popup
        _loadingPopup.ClosePopup();

        GetInputField((int)InputFields.Id).text = "";
        GetInputField((int)InputFields.Password).text = "";

        // Login Success
        if (res.LoginOk)
        {
            // TODO : 로비서버로 연결 및 씬 이동
            Managers.Network.AccountId = res.AccountId;
            Managers.Network.Token = res.Token;
            Managers.Network.Name = res.Name;

            // Temp
            Managers.Network.ConnectToGameServer(res.Channel);
            Managers.SceneLoad.LoadScene(Define.Scene.Game);
        }
        // Login Fail
        else
        {
            // ErrorCode 1 : not registered ID
            if (res.ErrorCode == 1)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("로그인 실패!\n등록되지 않은 아이디입니다.");
            }
            // ErrorCode 2 : incorrect PW
            else if (res.ErrorCode == 2)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("로그인 실패!\n비밀번호가 일치하지 않습니다.");
            }
        }
    }

    public void OnClickRegisterButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_RegisterPopup>();
    }
}
