using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Google.Protobuf.Protocol;

public class UI_InfoPopup : UI_Popup
{
    // each name of components to bind
    enum InputFields
    {
        NameInputField,
        PasswordInputField,
        NewPasswordInputField,
        NewPasswordCheckInputField
    }
    enum Buttons
    {
        CloseButton,
        SaveButton,
        ResetButton
    }

    Player _playerInfo;

    UI_LoadingCirclePopup _loadingPopup;
    UI_AlertPopup _alertPopup;


    public override void Init()
    {
        base.Init();

        _playerInfo = ((LobbyScene)Managers.SceneLoad.CurrentScene).PlayerInfo;

        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);
        GetButton((int)Buttons.SaveButton).gameObject.BindEvent(OnClickSaveButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ResetButton).gameObject.BindEvent(OnClickResetButton, Define.UIEvent.Click);

        ResetInfo();
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnClickSaveButton(PointerEventData evt)
    {
        // Show Loading UI
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("새 정보 저장 중");

        // Check empty info
        string name = Get<TMP_InputField>((int)InputFields.NameInputField).text;
        if (name == "")
        {
            _loadingPopup.ClosePopup();
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("빈 정보가 존재합니다!");
            return;
        }

        // Check if include password change
        string pwd = Get<TMP_InputField>((int)InputFields.PasswordInputField).text;
        string newPwd = Get<TMP_InputField>((int)InputFields.NewPasswordInputField).text;
        string newPwdCheck = Get<TMP_InputField>((int)InputFields.NewPasswordCheckInputField).text;
        bool pwdExcept = (pwd == "") && (newPwd == "") && (newPwdCheck == "");

        /* Not include password change */
        if (pwdExcept)
        {
            B_SaveInfo pakcet = new B_SaveInfo()
            {
                Name = name
            };
            Managers.Network.Send(pakcet);
        }
        /* Include password change */
        else
        {
            if (pwd == "")
            {
                _loadingPopup.ClosePopup();
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("기존 비밀번호가 입력되지 않았습니다!");
                return;
            }
            if (newPwd == "")
            {
                _loadingPopup.ClosePopup();
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("새 비밀번호가 입력되지 않았습니다!");
                return;
            }
            if (newPwd != newPwdCheck)
            {
                _loadingPopup.ClosePopup();
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("새 비밀번호가 일치하지 않습니다!");
                return;
            }
            B_SaveInfo pakcet = new B_SaveInfo()
            {
                Name = name,
                Password = pwd,
                NewPassword = newPwd
            };
            Managers.Network.Send(pakcet);
        }
    }
    public void OnRecvSaveInfoPacket(L_SaveInfo packet)
    {
        // Close loading UI 
        _loadingPopup.ClosePopup();

        if (packet.SaveOk)
        {
            // Update Player infomation
            Managers.Network.Name = packet.Name;
            _playerInfo.Name = packet.Name;
            ((UI_LobbyScene)Managers.UI.SceneUI).NameText.text = packet.Name;
            // Show alert UI
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("정보 수정 완료");
        }
        else
        {
            // Show alert UI
            if (packet.ErrorCode == 1)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("동일한 이름이 이미 존재합니다!");
            }
            else if (packet.ErrorCode == 2)
            {
                _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                _alertPopup.SetMessageText("기존 비밀번호가 일치하지 않습니다!");
            }
        }
    }

    public void OnClickResetButton(PointerEventData evt)
    {
        ResetInfo();
    }

    void ResetInfo()
    {
        // Set input field text to current player info
        Get<TMP_InputField>((int)InputFields.NameInputField).text = _playerInfo.Name;
        Get<TMP_InputField>((int)InputFields.PasswordInputField).text = "";
        Get<TMP_InputField>((int)InputFields.NewPasswordInputField).text = "";
        Get<TMP_InputField>((int)InputFields.NewPasswordCheckInputField).text = "";
    }
}
