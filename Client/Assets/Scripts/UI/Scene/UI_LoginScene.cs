using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_LoginScene : UI_Scene
{
    // UI component name to bind
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

    UI_AlertPopup _alertPopup;
    UI_LoadingCirclePopup _loadingPopup;
    UI_RegisterPopup _registerPopup;

    bool _prevTap = false;
    bool _prevEnter = false;


    public override void Init()
    {
        base.Init();

        Bind<TMP_InputField>(typeof(InputFields));
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
        // Close Loading UI
        _loadingPopup.ClosePopup();

        GetInputField((int)InputFields.Id).text = "";
        GetInputField((int)InputFields.Password).text = "";

        /* Login Success */
        if (res.LoginOk)
        {
            Managers.Network.AccountId = res.AccountId;
            Managers.Network.Token = res.Token;
            Managers.Network.Name = res.Name;
            Managers.Network.LobbyServer = res.LobbyServer;
            Managers.Network.GameServer1 = res.GameServer1;

            Managers.SceneLoad.LoadSceneAsync(Define.Scene.Lobby);
        }
        /* Login Fail */
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
            // ErrorCode 3 : same user access
            else if (res.ErrorCode == 3)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("로그인 실패!\n이미 접속중인 사용자 입니다!.");
            }
        }
    }

    public void OnClickRegisterButton(PointerEventData evt)
    {
        _registerPopup = Managers.UI.ShowPopupUI<UI_RegisterPopup>();
    }

    public void HandleKeyEvent(bool tap, bool enter)
    {
        bool isValid = (_loadingPopup == null) && (_alertPopup == null) && (_registerPopup == null);

        // Input "Tap"
        if ((_prevTap == false) && tap && isValid)
        {
            _prevTap = true;
            if (GetInputField((int)InputFields.Id).isFocused)
                GetInputField((int)InputFields.Password).Select();
            else
                GetInputField((int)InputFields.Id).Select();
        }
        if (tap == false)
            _prevTap = false;

        // Input "Enter"
        if ((_prevEnter == false) && enter && isValid)
        {
            _prevEnter = true;
            OnClickLoginButton(null);
        }
        if (enter == false)
            _prevEnter = false;
    }
}
