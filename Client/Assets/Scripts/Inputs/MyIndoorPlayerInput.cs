using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyIndoorPlayerInput : MonoBehaviour
{
    // Keyboard Input
    public Vector2 Move { get; set; }
    public bool Enter { get; set; }

    UI_IndoorScene _indoorSceneUI;

    private void Awake()
    {
        _indoorSceneUI = (UI_IndoorScene)Managers.UI.SceneUI;
    }

    #region Input Events
    public void OnMove(InputAction.CallbackContext value)
    {
        if (_indoorSceneUI == null)
            _indoorSceneUI = (UI_IndoorScene)Managers.UI.SceneUI;

        if (_indoorSceneUI.ChatInputField.isFocused == false)
            MoveInput(value.ReadValue<Vector2>());
        else
            MoveInput(new Vector2(0, 0));
    }

    public void OnEnter(InputAction.CallbackContext value)
    {
        EnterInput(value.action.triggered);
    }
    #endregion

    public void MoveInput(Vector2 newMoveDirection)
    {
        Move = newMoveDirection;
    }
    public void EnterInput(bool newEnterState)
    {
        Enter = newEnterState;
    }
}
