using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_HwadoIntroPopup : UI_Popup
{
    enum Buttons
    {
        CloseButton,
        OneButton,
        TwoButton,
        ThreeButton,
        FourButton
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
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnClickOneButton(PointerEventData evt)
    {
        Application.OpenURL("http://archi.kw.ac.kr/");
    }
    public void OnClickTwoButton(PointerEventData evt)
    {
        Application.OpenURL("http://chemng.kw.ac.kr/");
    }
    public void OnClickThreeButton(PointerEventData evt)
    {
        Application.OpenURL("http://env.kw.ac.kr/");
    }
    public void OnClickFourButton(PointerEventData evt)
    {
        Application.OpenURL("https://www.kwuarchitecture.com/");
    }
}
