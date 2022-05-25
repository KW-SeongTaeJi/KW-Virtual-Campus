using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        IndoorBima,
        IndoorHanwool,
        IndoorHwado,
        IndoorLibrary,
        IndoorOgui,
        IndoorSaebit
    }

    public enum UIEvent
    {
        Click,
        Enter,
        Exit
    }
}
