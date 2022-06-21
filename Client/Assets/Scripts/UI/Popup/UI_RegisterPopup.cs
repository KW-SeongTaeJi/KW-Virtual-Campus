using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UI_RegisterPopup : UI_Popup
{
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

    bool _prevTap = false;

    UI_AlertPopup _alertPopup;
    UI_LoadingCirclePopup _loadingPopup;


    public override void Init()
    {
        base.Init();

        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.RegisterButton).gameObject.BindEvent(OnClickRegisterButton, Define.UIEvent.Click);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelButton, Define.UIEvent.Click);
    }

    public void OnClickRegisterButton(PointerEventData evt)
    {
        // Show Loading UI
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("����� ��� ��");

        string name = GetText((int)InputFields.Name).text; 
        string id = GetText((int)InputFields.Id).text;
        string password = GetText((int)InputFields.Password).text;
        string passwordCheck = GetText((int)InputFields.PasswordCheck).text;

        /* Register Fail */
        // 1. empty information
        if (name == "" || id == "" || password == "")
        {
            _loadingPopup.ClosePopup();
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("��� ������ �Է����ּ���!");
            return;
        }
        // 2. Include blank
        if (name.Contains(" ") || id.Contains(" ") || password.Contains(" "))
        {
            _loadingPopup.ClosePopup();
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("������ ���Ե� ������ �����մϴ�!");
            return;
        }
        // 3. Input password is different
        if (password != passwordCheck)
        {
            _loadingPopup.ClosePopup();
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("�Է��� ��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        // TODO : PW �ؽ� ó��

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
            _alertPopup.SetMessageText("����� ��� ����!");
            _alertPopup.CloasAll = true;
        }
        // Register Fail
        else
        {
            // ErrorCode 1 : Same name is already used 
            if (res.ErrorCode == 1)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("����� ��� ����!\n�̹� ������� �̸��Դϴ�.");
            }
            // ErrorCode 2 : Same ID is already used 
            else if (res.ErrorCode == 2)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("����� ��� ����!\n�̹� ������� ���̵��Դϴ�.");
            }
        }
    }

    public void OnClickCancelButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void HandleKeyEvent(bool tap)
    {
        bool isValid = (_loadingPopup == null) && (_alertPopup == null);

        // Input "Tap"
        if ((_prevTap == false) && tap && isValid)
        {
            _prevTap = true;
            if (GetInputField((int)InputFields.Name).isFocused)
                GetInputField((int)InputFields.Id).Select();
            else if (GetInputField((int)InputFields.Id).isFocused)
                GetInputField((int)InputFields.Password).Select();
            else if (GetInputField((int)InputFields.Password).isFocused)
                GetInputField((int)InputFields.PasswordCheck).Select();
            else
                GetInputField((int)InputFields.Name).Select();
        }
        if (tap == false)
            _prevTap = false;
    }
}
