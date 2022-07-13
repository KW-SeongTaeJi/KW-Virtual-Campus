using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LoadingCirclePopup : UI_Popup
{
    // UI component name to bind
    enum Texts
    {
        LoadingMessageText
    }


    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public void SetMessageText(string message)
    {
       GetText((int)Texts.LoadingMessageText).text = message;
    }
}
