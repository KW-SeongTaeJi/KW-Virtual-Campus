using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_FriendListSlot : UI_Base
{
    // UI component name to bind
    enum Texts
    {
        NameText,
        ConnectedText
    }

    FriendInfo _friendInfo;


    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public void SetFriendInfo(FriendInfo info)
    {
        _friendInfo = info;
        Get<TextMeshProUGUI>((int)Texts.NameText).text = _friendInfo.Name;

        // this friend is offline
        if (Managers.Object.FindPlayerByName(info.Name) == null)
            SetOffline();
        // this friend is online
        else
            SetOnline();
    }
    public void SetOffline()
    {
        Get<TextMeshProUGUI>((int)Texts.ConnectedText).text = "固立加";
        Get<TextMeshProUGUI>((int)Texts.ConnectedText).color = Color.red;
    }
    public void SetOnline()
    {   
        Get<TextMeshProUGUI>((int)Texts.ConnectedText).text = "立加吝";
        Get<TextMeshProUGUI>((int)Texts.ConnectedText).color = Color.green;
    }
}
