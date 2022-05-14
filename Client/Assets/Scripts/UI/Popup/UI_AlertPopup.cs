using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AlertPopup : UI_Popup
{
    // each name of components to bind
    enum Texts
    {
        MessageText
    }
    enum Buttons
    {
        ConfirmButton
    }

    public bool CloasAll { get; set; } = false;
    public bool Quit { get; set; } = false;

    bool _initEnter = true;


    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton, Define.UIEvent.Click);
    }

    public void OnClickConfirmButton(PointerEventData evt)
    {
        if (Quit)
            Application.Quit();
        else
        {
            if (CloasAll)
                CloseAllPopup();
            else
                ClosePopup();
        }
    }

    public void SetMessageText(string message)
    {
        GetText((int)Texts.MessageText).text = message;
    }

    public void HandleKeyEvent(bool enter)
    {
        if ((_initEnter == false) && enter)
            OnClickConfirmButton(null);
        if (enter == false)
            _initEnter = false;
    }
}
