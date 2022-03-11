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
        Debug.Log("Ca");

        string id = GetInputField((int)InputFields.Id).text;
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
            Managers.SceneLoad.LoadScene(Define.Scene.Game);

            // TODO : 게임서버로 연결 및 씬 이동
        }
        else
        {
            // TODO : 로그인 실패 알림창
        }
    }

    public void OnClickRegisterButton(PointerEventData evt)
    {

    }
    void OnRecvRegisterPacket(CreateAccountPacketRes res)
    {

    }
}
