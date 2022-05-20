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
            _alertPopup.SetMessageText("ģ�� �̸��� �Է����ּ���!");
            return;
        }

        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("ģ�� ��û ������ ��");

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
            _alertPopup.SetMessageText("ģ�� ��û�� ���½��ϴ�.");
        }
        else
        {
            switch (packet.ErrorCode)
            {
                case 1:
                    _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                    _alertPopup.SetMessageText("��ϵ��� ���� ������Դϴ�!");
                    break;
                case 2:
                    _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                    _alertPopup.SetMessageText("�̹� ģ�� ��ϵ� ������Դϴ�!");
                    break;
                case 3:
                    _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
                    _alertPopup.SetMessageText("�̹� ģ�� ��û�� ���°ų�, �ش� ����ڷκ��� ģ�� ��û�� �޾ҽ��ϴ�!");
                    break;
            }
        }
    }
}
