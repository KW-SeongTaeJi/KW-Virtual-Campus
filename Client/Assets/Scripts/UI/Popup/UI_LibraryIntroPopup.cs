using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LibraryIntroPopup : UI_Popup
{
    enum Buttons
    {
        CloseButton,
    }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }
}
