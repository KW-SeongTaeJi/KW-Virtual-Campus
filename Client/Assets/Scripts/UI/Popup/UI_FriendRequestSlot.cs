using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Google.Protobuf.Protocol;
using TMPro;

public class UI_FriendRequestSlot : UI_Base
{
    enum Texts
    {
        FriendNameText
    }
    enum Buttons
    {
        AcceptButton,
        RefuseButton
    }

    string _friendName;

    UI_LoadingCirclePopup _loadingPopup;
    UI_AlertPopup _alertPopup;


    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.AcceptButton).gameObject.BindEvent(OnClickAcceptButton, Define.UIEvent.Click);
        GetButton((int)Buttons.RefuseButton).gameObject.BindEvent(OnClickRefuseButton, Define.UIEvent.Click);
    }

    public void SetFriendName(string name)
    {
        _friendName = name;
        Get<TextMeshProUGUI>((int)Texts.FriendNameText).text = _friendName;
    }

    public void OnClickAcceptButton(PointerEventData evt)
    {
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("수락 처리 중");

        B_AcceptFriend acceptPacket = new B_AcceptFriend()
        {
            Accept = true,
            FriendName = _friendName
        };
        Managers.Network.Send(acceptPacket);
    }

    public void OnClickRefuseButton(PointerEventData evt)
    {
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("거절 처리 중");

        B_AcceptFriend acceptPacket = new B_AcceptFriend()
        {
            Accept = false,
            FriendName = _friendName
        };
        Managers.Network.Send(acceptPacket);
    }

    public void OnRecvAcceptFriendPacket(L_AcceptFriend packet)
    {
        _loadingPopup.ClosePopup();

        if (packet.Success == false)
        {
            return;
        }

        // Accept
        if (packet.Accept)
        {
            FriendInfo friend = new FriendInfo()
            {
                Name = packet.Friend.Name,
                HairType = packet.Friend.HairType,
                FaceType = packet.Friend.FaceType,
                JacketType = packet.Friend.JacketType,
                HairColor = packet.Friend.HairColor,
                FaceColor_X = packet.Friend.FaceColor.X,
                FaceColor_Y = packet.Friend.FaceColor.Y,
                FaceColor_Z = packet.Friend.FaceColor.Z
            };
            ((LobbyScene)Managers.SceneLoad.CurrentScene).PlayerInfo.Friends.Add(friend.Name, friend);
            ((UI_LobbyScene)Managers.UI.SceneUI).FriendPopup.AddFriendSlot(friend);

            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("친구 요청을 수락하였습니다.");
        }
        // Refuse
        else
        {
            _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            _alertPopup.SetMessageText("친구 요청을 거절하였습니다.");
        }

        Destroy(gameObject);
    }
}
