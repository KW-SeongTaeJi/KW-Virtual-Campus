using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Google.Protobuf.Protocol;
using UnityEngine.UI;
using System;

public class UI_GameScene : UI_Scene
{
    // each name of components to bind
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
        NotebookEmotionButton
    }

    MyPlayerInput _input;

    public PlayerCanvasController MyPlayerCanvas { get; set; }
    public MyPlayerController MyPlayerController { get; set; }
    public TMP_InputField ChatInputField { get; set; }
    public GameObject EmotionPanel { get; set; }

    bool _prevEnter = false;

    GameObject _selectedEmotionButton;


    private void Update()
    {
        HandleEnter();
        HandleTap();
    }

    public override void Init()
    {
        base.Init();

        _input = ((GameScene)Managers.SceneLoad.CurrentScene).GetComponent<MyPlayerInput>();
        EmotionPanel = gameObject.FindChild("EmotionPanel");

        // Bind UI
        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));
                
        // Bind emotion enter & exit event
        foreach (Buttons btnName in Enum.GetValues(typeof(Buttons)))
        {
            Button btn = GetButton((int)btnName);
            btn.gameObject.BindEvent(OnEnterEmotionButton, Define.UIEvent.Enter);
            btn.gameObject.BindEvent(OnExitEmotionButton, Define.UIEvent.Exit);
        }

        // Add chatting event
        ChatInputField = Get<TMP_InputField>((int)InputFields.ChatInputField);
        ChatInputField.onSubmit.AddListener(delegate { OnSubmitChat(); });
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
}
