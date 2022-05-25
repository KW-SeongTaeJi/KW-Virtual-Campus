using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndoorPlayerCanvasController : UI_Base
{
    enum Texts
    {
        NameText,
        ChatText
    }
    enum Images
    {
        ChatImage
    }

    Coroutine _coActiveChatBox;

    float _activeTime = 5.0f;


    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void OnEnterChat(string msg)
    {
        if (_coActiveChatBox != null)
            StopCoroutine(_coActiveChatBox);

        _coActiveChatBox = StartCoroutine(CoActiveChatBox(msg));
    }
    IEnumerator CoActiveChatBox(string msg)
    {
        Image chatImage = Get<Image>((int)Images.ChatImage);
        TextMeshProUGUI chatText = Get<TextMeshProUGUI>((int)Texts.ChatText);

        chatImage.gameObject.SetActive(true);
        chatText.text = msg;

        yield return new WaitForSeconds(_activeTime);
        chatImage.gameObject.SetActive(false);
    }
}
