using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RegisterPopup : UI_Popup
{
    UI_AlertPopup _alertPopup;
    UI_LoadingCirclePopup _loadingPopup;

    // each name of components to bind
    enum InputFields
    {
        Name,
        Id,
        Password,
        PasswordCheck
    }
    enum Buttons
    {
        RegisterButton,
        CancelButton
    }


    public override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.RegisterButton).gameObject.BindEvent(OnClickRegisterButton, Define.UIEvent.Click);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelButton, Define.UIEvent.Click);
    }

    public void OnClickRegisterButton(PointerEventData evt)
    {
        // Show Loading UI
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("사용자 등록 중");

        string name = GetInputField((int)InputFields.Name).text; 
        string id = GetInputField((int)InputFields.Id).text;
        string password = GetInputField((int)InputFields.Password).text;
        string passwordCheck = GetInputField((int)InputFields.PasswordCheck).text;

        /* Register Fail */
        // 1. empty information
        if (name == "" || id == "" || password == "")
        {
            _loadingPopup.ClosePopup();
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("모든 정보를 입력해주세요!");
            return;
        }
        // 2. Include blank
        if (name.Contains(" ") || id.Contains(" ") || password.Contains(" "))
        {
            _loadingPopup.ClosePopup();
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("공백이 포함된 정보가 존재합니다!");
            return;
        }
        // 3. Input password is different
        if (password != passwordCheck)
        {
            _loadingPopup.ClosePopup();
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("입력한 비밀번호가 일치하지 않습니다.");
            return;
        }

        // TODO : PW 해시 처리

        // Send Register Packet to Account Server
        CreateAccountPacketReq packet = new CreateAccountPacketReq()
        {
            Name = name,
            AccountId = id,
            Password = password
        };
        Managers.Web.SendPostRequest<CreateAccountPacketRes>("account/createAccount", packet, OnRecvRegisterPacket);
    }
    void OnRecvRegisterPacket(CreateAccountPacketRes res)
    {
        // Close Loading Popup
        _loadingPopup.ClosePopup();
        
        // Register Success
        if (res.CreateAccountOk)
        {
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("사용자 등록 성공!");
            _alertPopup.CloasAll = true;
        }
        // Register Fail
        else
        {
            // ErrorCode 1 : Same name is already used 
            if (res.ErrorCode == 1)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("사용자 등록 실패!\n이미 사용중인 이름입니다.");
            }
            // ErrorCode 2 : Same ID is already used 
            else if (res.ErrorCode == 2)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("사용자 등록 실패!\n이미 사용중인 아이디입니다.");
            }
        }
    }

    public void OnClickCancelButton(PointerEventData evt)
    {
        ClosePopup();
    }
}
