using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LoadingCirclePopup : UI_Popup
{
    // each name of components to bind
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
        Get<TextMeshProUGUI>((int)Texts.LoadingMessageText).text = message;
    }
}
