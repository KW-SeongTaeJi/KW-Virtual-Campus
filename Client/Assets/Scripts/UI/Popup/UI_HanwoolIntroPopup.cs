using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_HanwoolIntroPopup : UI_Popup
{
    enum Buttons
    {
        CloseButton,
        OneButton,
        TwoButton,
        ThreeButton,
        FourButton,
        FiveButton
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
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnClickOneButton(PointerEventData evt)
    {
        Application.OpenURL("https://www.kw.ac.kr/ko/univ/depart_intro.jsp?hpage=college_005_01");
    }
    public void OnClickTwoButton(PointerEventData evt)
    {
        Application.OpenURL("https://english.kw.ac.kr");
    }
    public void OnClickThreeButton(PointerEventData evt)
    {
        Application.OpenURL("https://www.kwmedia.info/");
    }
    public void OnClickFourButton(PointerEventData evt)
    {
        Application.OpenURL("http://psy.kw.ac.kr/");
    }
    public void OnClickFiveButton(PointerEventData evt)
    {
        Application.OpenURL("https://www.kw.ac.kr/ko/univ/depart_intro.jsp?hpage=college_005_08");
    }
}
