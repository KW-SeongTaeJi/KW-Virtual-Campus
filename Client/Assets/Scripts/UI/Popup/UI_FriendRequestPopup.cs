using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendRequestPopup : UI_Popup
{
    // UI component name to bind
    enum Buttons
    {
        CloseButton
    }

    Transform _content;
    UI_LoadingCirclePopup _loadingPopup;

    public Dictionary<string, UI_FriendRequestSlot> ReqeustSlots { get; set; } = new Dictionary<string, UI_FriendRequestSlot>();


    public override void Init()
    {
        base.Init();

        // Show loading UI
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("요청함 로딩 중");

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);

        _content = gameObject.FindChild<Transform>("Content", true);

        // Send friend request list packet
        B_FriendRequestList packet = new B_FriendRequestList();
        Managers.Network.Send(packet);
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnRecvFriendRequestListPacket(L_FriendRequestList packet)
    {
        // Set friends request list
        foreach (string name in packet.FriendNames)
        {
            UI_FriendRequestSlot requestSlot = Managers.Resource.Instantiate("UI/Popup/UI_FriendRequestSlot").GetComponent<UI_FriendRequestSlot>();
            requestSlot.transform.SetParent(_content, false);
            requestSlot.SetFriendName(name);
            ReqeustSlots.Add(name, requestSlot);
        }

        // Close loading UI 
        UI_Popup requestPopup = Managers.UI.PopupUIStack.Pop();
        UI_Popup loadingPopup = Managers.UI.PopupUIStack.Pop();
        Managers.UI.PopupUIStack.Push(requestPopup);
        Managers.UI.PopupUIStack.Push(loadingPopup);
        _loadingPopup.ClosePopup();
    }
}