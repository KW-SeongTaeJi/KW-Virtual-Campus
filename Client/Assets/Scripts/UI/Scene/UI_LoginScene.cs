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
        _loadingPopup.SetMessageText("�α��� ��");

        string id = GetInputField((int)InputFields.Id).text;
        string password = GetInputField((int)InputFields.Password).text;

        // TODO : PW �ؽ� ó��

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
            // TODO : �κ񼭹��� ���� �� �� �̵�
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
                _alertPopup.SetMessageText("�α��� ����!\n��ϵ��� ���� ���̵��Դϴ�.");
            }
            // ErrorCode 2 : incorrect PW
            else if (res.ErrorCode == 2)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("�α��� ����!\n��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            }
        }
    }

    public void OnClickRegisterButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_RegisterPopup>();
    }
}
