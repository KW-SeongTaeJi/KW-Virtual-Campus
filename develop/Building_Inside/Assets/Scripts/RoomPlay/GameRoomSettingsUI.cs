using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomSettingsUI : Settings_UI
{
    public void ExitGameRoom()
    {
        var manager = RoomManager.singleton;
        if(manager.mode == Mirror.NetworkManagerMode.Host)
        {
            manager.StopHost();
        }
        else if(manager.mode==Mirror.NetworkManagerMode.ClientOnly)
        {
            manager.StopClient();
        }
    }
}
