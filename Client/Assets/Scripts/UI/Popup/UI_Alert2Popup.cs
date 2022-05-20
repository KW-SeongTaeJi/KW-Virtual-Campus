using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_Alert2Popup : UI_Popup
{
    // each name of components to bind
    enum Texts
    {
        MessageText
    }
    enum Buttons
    {
        ConfirmButton,
        CancelButton
    }

    public bool CloasAll { get; set; } = false;
    public bool Quit { get; set; } = false;

    public Action OnConfirmFunction { get; set; }

    bool _initEnter = true;


    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton, Define.UIEvent.Click);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelButton, Define.UIEvent.Click);
    }

    public void OnClickConfirmButton(PointerEventData evt)
    {
        if (OnConfirmFunction != null)
            OnConfirmFunction.Invoke();

        if (Quit)
            Application.Quit();

        if (CloasAll)
            CloseAllPopup();
        else
            ClosePopup();
    }

    public void OnClickCancelButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void SetMessageText(string message)
    {
        Get<TextMeshProUGUI>((int)Texts.MessageText).text = message;
    }

    public void HandleKeyEvent(bool enter)
    {
        if ((_initEnter == false) && enter)
            OnClickConfirmButton(null);
        if (enter == false)
            _initEnter = false;
    }
}
