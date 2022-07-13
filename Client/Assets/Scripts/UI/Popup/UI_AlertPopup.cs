using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_AlertPopup : UI_Popup
{
    // UI component name to bind
    enum Texts
    {
        MessageText
    }
    enum Buttons
    {
        ConfirmButton
    }

    bool _initEnter = true;

    public bool CloasAll { get; set; } = false;
    public bool Quit { get; set; } = false;


    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
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
