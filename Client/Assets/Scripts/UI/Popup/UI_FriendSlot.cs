using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Google.Protobuf.Protocol;

public class UI_FriendSlot : UI_Base
{
    enum Texts
    {
        FriendNameText
    }
    enum Buttons
    {
        FriendDeleteButton
    }

    UI_Alert2Popup _alert2Popup;
    UI_LoadingCirclePopup _loadingPopup;

    public FriendInfo FriendInfo { get; set; }


    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.FriendDeleteButton).gameObject.BindEvent(OnClickFriendDeleteButton, Define.UIEvent.Click);
    }

    public void SetFriendInfo(FriendInfo info)
    {
        FriendInfo = info;
        Get<TextMeshProUGUI>((int)Texts.FriendNameText).text = FriendInfo.Name;
    }

    public void OnClickFriendDeleteButton(PointerEventData evt)
    {
        _alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
        _alert2Popup.SetMessageText("해당 친구를 삭제하시겠습니까?");
        _alert2Popup.OnConfirmFunction -= OnClickFriendDeleteButton_Step2;
        _alert2Popup.OnConfirmFunction += OnClickFriendDeleteButton_Step2;
    }
    public void OnClickFriendDeleteButton_Step2()
    {
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();

        B_DeleteFriend packet = new B_DeleteFriend()
        {
            FriendName = FriendInfo.Name
        };
        Managers.Network.Send(packet);
    }

    public void OnRecvDeleteFriendPacket()
    {
        ((LobbyScene)Managers.SceneLoad.CurrentScene).PlayerInfo.Friends.Remove(FriendInfo.Name);
        ((UI_LobbyScene)Managers.UI.SceneUI).FriendPopup.FriendSlots.Remove(FriendInfo.Name);

        _loadingPopup.ClosePopup();

        Destroy(gameObject);
    }
}
