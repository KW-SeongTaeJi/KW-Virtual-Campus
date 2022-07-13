using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendsPopup : UI_Popup
{
    // UI component name to bind
    enum Buttons
    {
        CloseButton,
        AddFriendButton,
        FriendRequestButton
    }

    Transform _content;
    
    UI_LoadingCirclePopup _loadingPopup;

    public UI_FriendAddPopup FriendAddPopup { get; set; }
    public UI_FriendRequestPopup FriendRequestPopup { get; set; }
    public Dictionary<string, UI_FriendSlot> FriendSlots { get; set; } = new Dictionary<string, UI_FriendSlot>();


    public override void Init()
    {
        base.Init();

        // Show Loading UI
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("친구 정보 로딩 중");

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);
        GetButton((int)Buttons.AddFriendButton).gameObject.BindEvent(OnClickAddFriendButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FriendRequestButton).gameObject.BindEvent(OnClickFriendRequestButton, Define.UIEvent.Click);

        _content = gameObject.FindChild<Transform>("Content", true);

        // Send friend list packet
        B_FriendList packet = new B_FriendList();
        Managers.Network.Send(packet);
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnClickAddFriendButton(PointerEventData evt)
    {
        FriendAddPopup = Managers.UI.ShowPopupUI<UI_FriendAddPopup>();
    }

    public void OnClickFriendRequestButton(PointerEventData evt)
    {
        FriendRequestPopup = Managers.UI.ShowPopupUI<UI_FriendRequestPopup>();
    }

    public void OnRecvFriendListPacket(L_FriendList packet)
    {
        // Set friends list
        Player playerInfo = ((LobbyScene)Managers.SceneLoad.CurrentScene).PlayerInfo;
        playerInfo.Friends.Clear();
        for (int i = 0; i < packet.Friends.Count; i++)
        {
            FriendInfo friend = new FriendInfo()
            {
                Name = packet.Friends[i].Name,
                HairType = packet.Friends[i].HairType,
                FaceType = packet.Friends[i].FaceType,
                JacketType = packet.Friends[i].JacketType,
                HairColor = packet.Friends[i].HairColor,
                FaceColor_X = packet.Friends[i].FaceColor.X,
                FaceColor_Y = packet.Friends[i].FaceColor.Y,
                FaceColor_Z = packet.Friends[i].FaceColor.Z
            };
            playerInfo.Friends.Add(friend.Name, friend);
            AddFriendSlot(friend);
        }

        // Close loading UI 
        UI_Popup friendPopup = Managers.UI.PopupUIStack.Pop();
        UI_Popup loadingPopup = Managers.UI.PopupUIStack.Pop();
        Managers.UI.PopupUIStack.Push(friendPopup);
        Managers.UI.PopupUIStack.Push(loadingPopup);
        _loadingPopup.ClosePopup();
    }

    public void AddFriendSlot(FriendInfo info)
    {
        UI_FriendSlot friendSlot = Managers.Resource.Instantiate("UI/Popup/UI_FriendSlot").GetComponent<UI_FriendSlot>();
        friendSlot.transform.SetParent(_content, false);
        friendSlot.SetFriendInfo(info);
        FriendSlots.Add(info.Name, friendSlot);
    }
}
