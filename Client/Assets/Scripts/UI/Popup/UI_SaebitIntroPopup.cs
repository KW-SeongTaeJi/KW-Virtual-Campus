using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SaebitIntroPopup : UI_Popup
{
    enum Buttons
    {
        CloseButton,
        OneButton,
        TwoButton,
        ThreeButton
    }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);
        GetButton((int)Buttons.OneButton).gameObject.BindEvent(OnClickOneButton, Define.UIEvent.Click);
        GetButton((int)Buttons.TwoButton).gameObject.BindEvent(OnClickTwoButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ThreeButton).gameObject.BindEvent(OnClickThreeButton, Define.UIEvent.Click);
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnClickOneButton(PointerEventData evt)
    {
        Application.OpenURL("https://ce.kw.ac.kr:501/main/main.php");
    }
    public void OnClickTwoButton(PointerEventData evt)
    {
        Application.OpenURL("https://cs.kw.ac.kr:501/main/main.php");
    }
    public void OnClickThreeButton(PointerEventData evt)
    {
        Application.OpenURL("https://ic.kw.ac.kr:501/main/main.php");
    }
}
