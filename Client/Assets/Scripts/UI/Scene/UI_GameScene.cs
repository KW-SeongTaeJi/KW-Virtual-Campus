using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Google.Protobuf.Protocol;

public class UI_GameScene : UI_Scene
{
    // each name of components to bind
    enum InputFields
    {
        ChatInputField
    }

    MyPlayerInput _input;

    bool _prevEnter = false;

    public PlayerCanvasController MyPlayerCanvas { get; set; }
    public TMP_InputField ChatInputField { get; set; }


    private void Update()
    {
        HandleEnter();
    }

    public override void Init()
    {
        base.Init();

        Bind<TMP_InputField>(typeof(InputFields));

        ChatInputField = Get<TMP_InputField>((int)InputFields.ChatInputField);
        ChatInputField.onSubmit.AddListener(delegate { OnSubmitChat(); });

        _input = ((GameScene)Managers.SceneLoad.CurrentScene).GetComponent<MyPlayerInput>();
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
}
