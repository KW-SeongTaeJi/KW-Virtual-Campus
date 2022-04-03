using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AlertPopup : UI_Popup
{
    public bool CloasAll { get; set; } = false;
    
    // each name of components to bind
    enum Texts
    {
        MessageText
    }
    enum Buttons
    {
        ConfirmButton
    }


    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton, Define.UIEvent.Click);
    }

    public void OnClickConfirmButton(PointerEventData evt)
    {
        if (CloasAll)
            CloseAllPopup();
        else
            ClosePopup();
    }

    public void SetMessageText(string message)
    {
        GetText((int)Texts.MessageText).text = message;
    }
}
