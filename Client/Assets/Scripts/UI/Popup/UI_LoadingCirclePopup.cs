using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        Bind<Text>(typeof(Texts));
    }

    public void SetMessageText(string message)
    {
        GetText((int)Texts.LoadingMessageText).text = message;
    }
}
