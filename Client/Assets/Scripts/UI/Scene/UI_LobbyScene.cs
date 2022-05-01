using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    // each name of components to bind
    enum Buttons
    {
        LogoutButton,
        SettingsButton,
        CampusEnterButton,
        CustermizeButton,
        FriendsButton,
        InfoButton
    }
    enum Texts
    {
        NameText
    }

    public UI_CustermizePopup CustermizePopup { get; set; }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.LogoutButton).gameObject.BindEvent(OnClickLogoutButton, Define.UIEvent.Click);
        GetButton((int)Buttons.SettingsButton).gameObject.BindEvent(OnClickSettingsButton, Define.UIEvent.Click);
        GetButton((int)Buttons.CampusEnterButton).gameObject.BindEvent(OnClickCampusEnterButton, Define.UIEvent.Click);
        GetButton((int)Buttons.CustermizeButton).gameObject.BindEvent(OnClickCutermizeButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FriendsButton).gameObject.BindEvent(OnClickFriendsButton, Define.UIEvent.Click);
        GetButton((int)Buttons.InfoButton).gameObject.BindEvent(OnClickInfoButton, Define.UIEvent.Click);

        GetText((int)Texts.NameText).text = $"{Managers.Network.Name} ��";
    }

    public void OnClickLogoutButton(PointerEventData evt)
    {
        Managers.Network.DisconnectSession();
        Managers.SceneLoad.LoadScene(Define.Scene.Login);
    }

    public void OnClickSettingsButton(PointerEventData evt)
    {

    }

    public void OnClickCampusEnterButton(PointerEventData evt)
    {
        // TODO : �ּ� ���ġ
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        ChannelInfo channel = new ChannelInfo()
        {
            IpAddress = ipAddr.ToString(),
            Port = 8000
        };

        Managers.Network.DisconnectSession();
        Managers.Network.ConnectToGameServer(channel);
        Managers.SceneLoad.LoadScene(Define.Scene.Game);
    }

    public void OnClickCutermizeButton(PointerEventData evt)
    {
        CustermizePopup = Managers.UI.ShowPopupUI<UI_CustermizePopup>();
    }

    public void OnClickFriendsButton(PointerEventData evt)
    {

    }

    public void OnClickInfoButton(PointerEventData evt)
    {

    }
}
