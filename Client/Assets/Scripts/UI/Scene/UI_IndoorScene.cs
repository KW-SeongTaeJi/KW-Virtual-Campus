using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_IndoorScene : UI_Scene
{
    // each name of components to bind
    enum InputFields
    {
        ChatInputField
    }
    enum Buttons
    {
        HelpButton,
        OutdoorButton,
        SettingsButton,
        ExitButton,
        FriendListButton,
        ChatLogButton,
        BuildingButton
    }
    enum Texts
    {
        BuildingTitleText
    }

    bool _prevEnter = false;

    MyIndoorPlayerInput _input;
    Transform _content;
    UI_Alert2Popup _alert2Popup;

    public IndoorPlayerCanvasController MyPlayerCanvas { get; set; }
    public TMP_InputField ChatInputField { get; set; }
    public GameObject FriendListPanel { get; set; }
    public Dictionary<string, UI_FriendListSlot> FriendListSlots { get; set; } = new Dictionary<string, UI_FriendListSlot>();


    private void Update()
    {
        HandleEnter();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<TextMeshProUGUI>(typeof(Texts));

        BaseScene currentScene = Managers.SceneLoad.CurrentScene;
        switch (currentScene.SceneType)
        {
            case Define.Scene.IndoorBima:
                _input = ((IndoorScene_Bima)currentScene).GetComponent<MyIndoorPlayerInput>();
                GetText((int)Texts.BuildingTitleText).text = "비마관";
                break;
            case Define.Scene.IndoorHanwool:
                _input = ((IndoorScene_Hanwool)currentScene).GetComponent<MyIndoorPlayerInput>();
                GetText((int)Texts.BuildingTitleText).text = "한울관";
                break;
            case Define.Scene.IndoorHwado:
                _input = ((IndoorScene_Hwado)currentScene).GetComponent<MyIndoorPlayerInput>();
                GetText((int)Texts.BuildingTitleText).text = "화도관";
                break;
            case Define.Scene.IndoorLibrary:
                _input = ((IndoorScene_Library)currentScene).GetComponent<MyIndoorPlayerInput>();
                GetText((int)Texts.BuildingTitleText).text = "중앙도서관";
                break;
            case Define.Scene.IndoorOgui:
                _input = ((IndoorScene_Ogui)currentScene).GetComponent<MyIndoorPlayerInput>();
                GetText((int)Texts.BuildingTitleText).text = "옥의관";
                break;
            case Define.Scene.IndoorSaebit:
                _input = ((IndoorScene_Saebit)currentScene).GetComponent<MyIndoorPlayerInput>();
                GetText((int)Texts.BuildingTitleText).text = "새빛관";
                break;
        }
        _content = gameObject.FindChild<Transform>("Content", true);
        FriendListPanel = gameObject.FindChild("FriendListPanel");

        // Bind button event
        GetButton((int)Buttons.HelpButton).gameObject.BindEvent(OnClickHelpButton, Define.UIEvent.Click);
        GetButton((int)Buttons.OutdoorButton).gameObject.BindEvent(OnClickOutdoorButton, Define.UIEvent.Click);
        GetButton((int)Buttons.SettingsButton).gameObject.BindEvent(OnClickSettingsButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FriendListButton).gameObject.BindEvent(OnClickFriendListButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ChatLogButton).gameObject.BindEvent(OnClickChatLogButton, Define.UIEvent.Click);
        GetButton((int)Buttons.BuildingButton).gameObject.BindEvent(OnClickBuildingButton, Define.UIEvent.Click);

        // Binc chatting event
        ChatInputField = GetInputField((int)InputFields.ChatInputField);
        ChatInputField.onSubmit.AddListener(delegate { OnSubmitChat(); });
    }

    public void OnClickHelpButton(PointerEventData evt)
    {

    }

    public void OnClickOutdoorButton(PointerEventData evt)
    {
        _alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
        _alert2Popup.SetMessageText("밖으로 이동하시겠습니까?");
        _alert2Popup.OnConfirmFunction -= OnClickOutdoorButton_Step2;
        _alert2Popup.OnConfirmFunction += OnClickOutdoorButton_Step2;
    }
    public void OnClickOutdoorButton_Step2()
    {
        Managers.Object.Clear();
        Managers.SceneLoad.LoadSceneAsync(Define.Scene.Game, 0);
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

    public void OnClickFriendListButton(PointerEventData evt)
    {
        if (FriendListPanel.activeSelf == false)
        {
            FriendListPanel.SetActive(true);
        }
        else
        {
            FriendListPanel.SetActive(false);
        }
    }

    public void OnClickChatLogButton(PointerEventData evt)
    {

    }

    public void OnClickBuildingButton(PointerEventData evt)
    {
        string building = GetText((int)Texts.BuildingTitleText).text;
        switch (building)
        {
            case "비마관":
                Managers.UI.ShowPopupUI<UI_BimaIntroPopup>();
                break;
            case "화도관":
                Managers.UI.ShowPopupUI<UI_HwadoIntroPopup>();
                break;
            case "한울관":
                Managers.UI.ShowPopupUI<UI_HanwoolIntroPopup>();
                break;
            case "중앙도서관":
                Managers.UI.ShowPopupUI<UI_LibraryIntroPopup>();
                break;
            case "옥의관":
                Managers.UI.ShowPopupUI<UI_OguiIntroPopup>();
                break;
            case "새빛관":
                Managers.UI.ShowPopupUI<UI_SaebitIntroPopup>();
                break;
        }
    }

    void OnSubmitChat()
    {
        string msg = ChatInputField.text;
        if (msg == "")
            return;

        MyPlayerCanvas.OnEnterChat(msg);
        ChatInputField.text = "";
        ChatInputField.ActivateInputField();

        // Send message to server
        C_Chat chatPacket = new C_Chat() { Message = msg };
        Managers.Network.Send(chatPacket);
    }

    public void SetFriendList()
    {
        foreach (FriendInfo friend in Managers.Object.MyIndoorPlayer.Friends.Values)
        {
            UI_FriendListSlot friendSlot = Managers.Resource.Instantiate("UI/Popup/UI_FriendListSlot").GetComponent<UI_FriendListSlot>();
            friendSlot.transform.SetParent(_content);
            friendSlot.SetFriendInfo(friend);
            FriendListSlots.Add(friend.Name, friendSlot);
        }
    }

    void HandleEnter()
    {
        if ((_prevEnter == false) && _input.Enter)
        {
            _prevEnter = true;
            /* if chat box is inactive */
            if (ChatInputField.isFocused == false)
            {
                ChatInputField.Select();
            }
        }

        if (_input.Enter == false)
        {
            _prevEnter = false;
        }
    }
}
