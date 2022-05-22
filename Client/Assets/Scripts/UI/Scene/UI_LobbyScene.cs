using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    // each name of components to bind
    enum Buttons
    {
        LogoutButton,
        HelpButton,
        SettingsButton,
        ExitButton,
        CampusEnterButton,
        InfoButton,
        FriendsButton,
        CustermizeButton
    }
    enum Texts
    {
        NameText
    }

    UI_Alert2Popup _alert2Popup;
    UI_LoadingCirclePopup _loadingPopup;

    public UI_InfoPopup InfoPopup { get; set; }
    public UI_FriendsPopup FriendPopup { get; set; }
    public UI_CustermizePopup CustermizePopup { get; set; }
    public TextMeshProUGUI NameText { get; set; }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetButton((int)Buttons.LogoutButton).gameObject.BindEvent(OnClickLogoutButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HelpButton).gameObject.BindEvent(OnClickHelpButton, Define.UIEvent.Click);
        GetButton((int)Buttons.SettingsButton).gameObject.BindEvent(OnClickSettingsButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton, Define.UIEvent.Click);
        GetButton((int)Buttons.CampusEnterButton).gameObject.BindEvent(OnClickCampusEnterButton, Define.UIEvent.Click);
        GetButton((int)Buttons.CustermizeButton).gameObject.BindEvent(OnClickCutermizeButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FriendsButton).gameObject.BindEvent(OnClickFriendsButton, Define.UIEvent.Click);
        GetButton((int)Buttons.InfoButton).gameObject.BindEvent(OnClickInfoButton, Define.UIEvent.Click);

        NameText = Get<TextMeshProUGUI>((int)Texts.NameText);
        NameText.text = $"{Managers.Network.Name} 님";
    }

    public void OnClickLogoutButton(PointerEventData evt)
    {
        Managers.Network.DisconnectSession();
        Managers.SceneLoad.LoadScene(Define.Scene.Login);
    }

    public void OnClickHelpButton(PointerEventData evt)
    {

    }

    public void OnClickSettingsButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_SettingsPopup>();
    }

    public void OnClickExitButton(PointerEventData evt)
    {
        _alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
        _alert2Popup.SetMessageText("프로그램을 종료하시겠습니까?");
        _alert2Popup.Quit = true;
    }

    public void OnClickCampusEnterButton(PointerEventData evt)
    {
        // TODO : 주소 재배치
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        //IPAddress ipAddr = ipHost.AddressList[0];
        IPAddress ipAddr = IPAddress.Parse("54.180.31.154");
        ChannelInfo channel = new ChannelInfo()
        {
            IpAddress = ipAddr.ToString(),
            Port = 8000
        };

        Managers.Network.DisconnectSession();
        Managers.Network.ConnectToGameServer(channel);
        Managers.SceneLoad.LoadScene(Define.Scene.Game);
    }

    public void OnClickInfoButton(PointerEventData evt)
    {
        InfoPopup = Managers.UI.ShowPopupUI<UI_InfoPopup>();
    }

    public void OnClickFriendsButton(PointerEventData evt)
    {
        FriendPopup = Managers.UI.ShowPopupUI<UI_FriendsPopup>();
    }

    public void OnClickCutermizeButton(PointerEventData evt)
    {
        CustermizePopup = Managers.UI.ShowPopupUI<UI_CustermizePopup>();
    }
}
