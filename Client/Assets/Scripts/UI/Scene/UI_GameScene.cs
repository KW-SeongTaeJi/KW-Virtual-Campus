using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Google.Protobuf.Protocol;
using UnityEngine.UI;
using System;
using System.Net;

public class UI_GameScene : UI_Scene
{
    // UI component name to bind
    enum InputFields
    {
        ChatInputField
    }
    enum Buttons
    {
        YesEmotionButton,
        NoEmotionButton,
        SitEmotionButton,
        LieEmotionButton,
        WaveHandEmotionButton,
        PointEmotionButton,
        ClapEmotionButton,
        GuitarEmotionButton,
        BookEmotionButton,
        BallEmotionButton,
        NotebookEmotionButton,
        HelpButton,
        LobbyButton,
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
    bool _prevOne = false;
    bool _prevTwo = false;
    bool _prevThree = false;
    bool _prevFour = false;
    bool _prevFive = false;
    bool _prevSix = false;

    MyPlayerInput _input;
    Transform _content;
    GameObject _selectedEmotionButton;

    public PlayerCanvasController MyPlayerCanvas { get; set; }
    public MyPlayerController MyPlayerController { get; set; }
    public TMP_InputField ChatInputField { get; set; }
    public UI_Alert2Popup Alert2Popup { get; set; }
    public GameObject EmotionPanel { get; set; }
    public GameObject FriendListPanel { get; set; }
    public Dictionary<string, UI_FriendListSlot> FriendListSlots { get; set; } = new Dictionary<string, UI_FriendListSlot>();


    private void Update()
    {
        HandleEnter();
        HandleTap();
        HandleNum();
    }

    public override void Init()
    {
        base.Init();

        _input = ((GameScene)Managers.SceneLoad.CurrentScene).GetComponent<MyPlayerInput>();
        _content = gameObject.FindChild<Transform>("Content", true);

        EmotionPanel = gameObject.FindChild("EmotionPanel");
        FriendListPanel = gameObject.FindChild("FriendListPanel");

        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // Bind button event
        GetButton((int)Buttons.HelpButton).gameObject.BindEvent(OnClickHelpButton, Define.UIEvent.Click);
        GetButton((int)Buttons.LobbyButton).gameObject.BindEvent(OnClickLobbyButton, Define.UIEvent.Click);
        GetButton((int)Buttons.SettingsButton).gameObject.BindEvent(OnClickSettingsButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FriendListButton).gameObject.BindEvent(OnClickFriendListButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ChatLogButton).gameObject.BindEvent(OnClickChatLogButton, Define.UIEvent.Click);
        GetButton((int)Buttons.BuildingButton).gameObject.BindEvent(OnClickBuildingButton, Define.UIEvent.Click);

        // Bind button enter & exit event
        foreach (Buttons btnName in Enum.GetValues(typeof(Buttons)))
        {
            if (btnName == Buttons.HelpButton)
                break;
            Button btn = GetButton((int)btnName);
            btn.gameObject.BindEvent(OnEnterEmotionButton, Define.UIEvent.Enter);
            btn.gameObject.BindEvent(OnExitEmotionButton, Define.UIEvent.Exit);
        }

        // Binc chatting event
        ChatInputField = Get<TMP_InputField>((int)InputFields.ChatInputField);
        ChatInputField.onSubmit.AddListener(delegate { OnSubmitChat(); });
    }

    public void OnClickHelpButton(PointerEventData evt)
    {

    }

    public void OnClickLobbyButton(PointerEventData evt)
    {
        Alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
        Alert2Popup.SetMessageText("로비로 이동하시겠습니까?");
        Alert2Popup.OnConfirmFunction -= OnClickLobbyButton_Step2;
        Alert2Popup.OnConfirmFunction += OnClickLobbyButton_Step2;
    }
    public void OnClickLobbyButton_Step2()
    {
        Managers.Object.Clear();
        Managers.Network.DisconnectSession();
        Managers.SceneLoad.LoadSceneAsync(Define.Scene.Lobby);
    }

    public void OnClickSettingsButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_SettingsPopup>();
    }

    public void OnClickExitButton(PointerEventData evt)
    {
        Alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
        Alert2Popup.SetMessageText("프로그램을 종료하시겠습니까?");
        Alert2Popup.Quit = true;
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
                Managers.UI.ShowPopupUI<UI_SaebitIntroPopup>();
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

    public void OnEnterEmotionButton(PointerEventData evt)
    {
        _selectedEmotionButton = evt.pointerEnter;
    }
    public void OnExitEmotionButton(PointerEventData evt)
    {
        _selectedEmotionButton = null;
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
        foreach (FriendInfo friend in Managers.Object.MyPlayer.Friends.Values)
        {
            UI_FriendListSlot friendSlot = Managers.Resource.Instantiate("UI/Popup/UI_FriendListSlot").GetComponent<UI_FriendListSlot>();
            friendSlot.transform.SetParent(_content, false);
            friendSlot.SetFriendInfo(friend);
            FriendListSlots.Add(friend.Name, friendSlot);
        }
    }

    public void SetBuildingButton(string building)
    {
        GetButton((int)Buttons.BuildingButton).gameObject.SetActive(true);
        GetText((int)Texts.BuildingTitleText).text = building;
    }
    public void UnsetBuildingButton()
    {
        GetButton((int)Buttons.BuildingButton).gameObject.SetActive(false);
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

    void HandleTap()
    {
        if (_input.Tap)
        {
            if (EmotionPanel.activeSelf == false)
            {
                EmotionPanel.SetActive(true);
                _input.CursorLock = false;
            }
        }
        else
        {
            if (EmotionPanel.activeSelf == true)
            {
                if (_selectedEmotionButton != null)
                {
                    Buttons emotionBtn = (Buttons)Enum.Parse(typeof(Buttons), _selectedEmotionButton.name);
                    int emotionNum = (int)emotionBtn + 1;
                    MyPlayerController.SetEmotionAnimation(emotionNum);
                    C_Emotion emotionPacket = new C_Emotion() { EmotionNum = emotionNum };
                    Managers.Network.Send(emotionPacket);
                }
                EmotionPanel.SetActive(false);
                _input.CursorLock = true;
            }
        }
    }

    void HandleNum()
    {
        if ((_prevOne == false) && _input.One)
        {
            _prevOne = true;
            Alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
            Alert2Popup.SetMessageText("비마관으로 입장하시겠습니까?");
            Alert2Popup.OnConfirmFunction -= EnterBima;
            Alert2Popup.OnConfirmFunction += EnterBima;
        }
        if ((_prevTwo == false) && _input.Two)
        {
            _prevTwo = true;
            Alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
            Alert2Popup.SetMessageText("한울관으로 입장하시겠습니까?");
            Alert2Popup.OnConfirmFunction -= EnterHanwool;
            Alert2Popup.OnConfirmFunction += EnterHanwool;
        }
        if ((_prevThree == false) && _input.Three)
        {
            _prevThree = true;
            Alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
            Alert2Popup.SetMessageText("화도관으로 입장하시겠습니까?");
            Alert2Popup.OnConfirmFunction -= EnterHwado;
            Alert2Popup.OnConfirmFunction += EnterHwado;
        }
        if ((_prevFour == false) && _input.Four)
        {
            _prevFour = true;
            Alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
            Alert2Popup.SetMessageText("중앙도서관으로 입장하시겠습니까?");
            Alert2Popup.OnConfirmFunction -= EnterLibrary;
            Alert2Popup.OnConfirmFunction += EnterLibrary;
        }
        if ((_prevFive == false) && _input.Five)
        {
            _prevFive = true;
            Alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
            Alert2Popup.SetMessageText("옥의관으로 입장하시겠습니까?");
            Alert2Popup.OnConfirmFunction -= EnterOgui;
            Alert2Popup.OnConfirmFunction += EnterOgui;
        }
        if ((_prevSix == false) && _input.Six)
        {
            _prevSix = true;
            Alert2Popup = Managers.UI.ShowPopupUI<UI_Alert2Popup>();
            Alert2Popup.SetMessageText("새빛관으로 입장하시겠습니까?");
            Alert2Popup.OnConfirmFunction -= EnterSaebit;
            Alert2Popup.OnConfirmFunction += EnterSaebit;
        }

        if (_input.One == false)
        {
            _prevOne = false;
        }
        if (_input.Two == false)
        {
            _prevTwo = false;
        }
        if (_input.Three == false)
        {
            _prevThree = false;
        }
        if (_input.Four == false)
        {
            _prevFour = false;
        }
        if (_input.Five == false)
        {
            _prevFive = false;
        }
        if (_input.Six == false)
        {
            _prevSix = false;
        }
    }
    void EnterBima()
    {
        Managers.Object.Clear();
        Managers.SceneLoad.LoadSceneAsync(Define.Scene.IndoorBima);
    }
    void EnterHanwool()
    {
        Managers.Object.Clear();
        Managers.SceneLoad.LoadSceneAsync(Define.Scene.IndoorHanwool);
    }
    void EnterHwado()
    {
        Managers.Object.Clear();
        Managers.SceneLoad.LoadSceneAsync(Define.Scene.IndoorHwado);
    }
    void EnterLibrary()
    {
        Managers.Object.Clear();
        Managers.SceneLoad.LoadSceneAsync(Define.Scene.IndoorLibrary);
    }
    void EnterOgui()
    {
        Managers.Object.Clear();
        Managers.SceneLoad.LoadSceneAsync(Define.Scene.IndoorOgui);
    }
    void EnterSaebit()
    {
        Managers.Object.Clear();
        Managers.SceneLoad.LoadSceneAsync(Define.Scene.IndoorSaebit);
    }
}
