using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LoginScene : UI_Scene
{
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
        string id = GetInputField((int)InputFields.Id).text;
        // TODO : PW �ؽ� ó��
        string password = GetInputField((int)InputFields.Password).text;

        LoginPacketReq packet = new LoginPacketReq()
        {
            AccountId = id,
            Password = password
        };

        Managers.Web.SendPostRequest<LoginPakcetRes>("account/login", packet, OnRecvLoginPacket);
    }
    void OnRecvLoginPacket(LoginPakcetRes res)
    {
        GetInputField((int)InputFields.Id).text = "";
        GetInputField((int)InputFields.Password).text = "";

        if (res.LoginOk)
        {
            // TODO : ���Ӽ����� ���� �� �� �̵�
            Managers.Network.AccountId = res.AccountId;
            Managers.Network.Token = res.Token;
            Managers.Network.Name = res.Name;

            // Temp
            Managers.Network.ConnectToGameServer(res.Channel);
            Managers.SceneLoad.LoadScene(Define.Scene.Game);
        }
        else
        {
            // TODO : �α��� ���� �˸�â
        }
    }

    public void OnClickRegisterButton(PointerEventData evt)
    {
        
    }
    void OnRecvRegisterPacket(CreateAccountPacketRes res)
    {

    }
}