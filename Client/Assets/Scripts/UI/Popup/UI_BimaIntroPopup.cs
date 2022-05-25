using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BimaIntroPopup : UI_Popup
{
    enum Buttons
    {
        CloseButton,
        OneButton,
        TwoButton,
        ThreeButton,
        FourButton,
        FiveButton,
        SixButton
    }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);
        GetButton((int)Buttons.OneButton).gameObject.BindEvent(OnClickOneButton, Define.UIEvent.Click);
        GetButton((int)Buttons.TwoButton).gameObject.BindEvent(OnClickTwoButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ThreeButton).gameObject.BindEvent(OnClickThreeButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FourButton).gameObject.BindEvent(OnClickFourButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FiveButton).gameObject.BindEvent(OnClickFiveButton, Define.UIEvent.Click);
        GetButton((int)Buttons.SixButton).gameObject.BindEvent(OnClickSixButton, Define.UIEvent.Click);
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnClickOneButton(PointerEventData evt)
    {
        Application.OpenURL("https://ee.kw.ac.kr/");
    }
    public void OnClickTwoButton(PointerEventData evt)
    {
        Application.OpenURL("https://elcomm.kw.ac.kr/");
    }
    public void OnClickThreeButton(PointerEventData evt)
    {
        Application.OpenURL("https://radiowave.kw.ac.kr/");
    }
    public void OnClickFourButton(PointerEventData evt)
    {
        Application.OpenURL("https://electric.kw.ac.kr/");
    }
    public void OnClickFiveButton(PointerEventData evt)
    {
        Application.OpenURL("https://snme.kw.ac.kr/");
    }
    public void OnClickSixButton(PointerEventData evt)
    {
        Application.OpenURL("https://cni.kw.ac.kr/");
    }
}

