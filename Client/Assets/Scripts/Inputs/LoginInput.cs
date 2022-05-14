using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoginInput : MonoBehaviour
{
    // Keyboard Input Valus
    public bool Tap { get; set; }
    public bool Enter { get; set; }


    #region Input Events
    public void OnTap(InputAction.CallbackContext value)
    {
        TapInput(value.action.triggered);
    }
    public void OnEnter(InputAction.CallbackContext value)
    {
        EnterInput(value.action.triggered);
    }
    #endregion

    public void TapInput(bool newTapState)
    {
        Tap = newTapState;
    }
    public void EnterInput(bool newEnterState)
    {
        Enter = newEnterState;
    }
}
