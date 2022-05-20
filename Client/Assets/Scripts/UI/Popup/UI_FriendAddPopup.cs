using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Google.Protobuf.Protocol;

public class UI_FriendAddPopup : UI_Popup
{
    enum Buttons
    {
        CloseButton,
        AddButton
    }
    enum InputFields
    {
        FriendNameInputField
    }

    UI_AlertPopup _alertPopup;
    UI_LoadingCirclePopup _loadingPopup;


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);
        GetButton((int)Buttons.AddButton).gameObject.BindEvent(OnClickAddButton, Define.UIEvent.Click);
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnClickAddButton(PointerEventData evt)
    {
        string friendName = Get<TMP_InputField>((int)InputFields.FriendNameInputField).text;

        if (friendName == "")
        {
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("친구 이름을 입력해주세요!");
            return;
        }

        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("친구 요청 보내는 중");

        B_AddFriend packet = new B_AddFriend()
        {
            FriendName = friendName
        };
        Managers.Network.Send(packet);
    }

    public void OnRecvAddFriendPacket(L_AddFriend packet)
    {
        _loadingPopup.ClosePopup();

        if (packet.Success)
        {
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("친구 요청을 보냈습니다.");
        }
        else
        {
            switch (packet.ErrorCode)
            {
                case 1:
                    _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                    _alertPopup.SetMessageText("등록되지 않은 사용자입니다!");
                    break;
                case 2:
                    _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                    _alertPopup.SetMessageText("이미 친구 등록된 사용자입니다!");
                    break;
                case 3:
                    _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                    _alertPopup.SetMessageText("이미 친구 요청을 보냈거나, 해당 사용자로부터 친구 요청을 받았습니다!");
                    break;
            }
        }
    }
}
