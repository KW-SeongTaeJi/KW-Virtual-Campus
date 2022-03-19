using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        // Set ordered Popup UI Canvas
        Managers.UI.SetCanvas(gameObject, sort: true);
    }

    public virtual void ClosePopup()
    {
        Managers.UI.ClosePopupUI();
    }
}
