using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MyPlayerInput : MonoBehaviour
{
	// Keyboard Input Values
	public Vector2 Move { get; set; }
	public Vector2 Look { get; set; }
	public bool Jump { get; set; }
	public bool Sprint { get; set; }
	public bool Enter { get; set; }

	// Mouse Cursor Settings
	public bool CursorLocked { get; set; } = true;
	public bool CursorInputForLook { get; set; } = true;

	TMP_InputField _chatInputField;


    private void Awake()
    {
		_chatInputField = ((UI_GameScene)Managers.UI.SceneUI).ChatInputField;
	}

    #region Input Events
    public void OnMove(InputAction.CallbackContext value)
	{
		if (_chatInputField.isFocused == false)
			MoveInput(value.ReadValue<Vector2>());
		else
			MoveInput(new Vector2(0, 0));
	}

	public void OnLook(InputAction.CallbackContext value)
	{
		if (CursorInputForLook)
		{
			LookInput(value.ReadValue<Vector2>());
		}
	}

	public void OnJump(InputAction.CallbackContext value)
	{
		if (_chatInputField.isFocused == false)
			JumpInput(value.action.triggered);
		else
			JumpInput(false);
	}

	public void OnSprint(InputAction.CallbackContext value)
	{
		if (_chatInputField.isFocused == false)
			SprintInput(value.action.ReadValue<float>() == 1);
		else
			SprintInput(false);
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

	public void LookInput(Vector2 newLookDirection)
	{
		Look = newLookDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		Jump = newJumpState;
	}

	public void SprintInput(bool newSprintState)
	{
		Sprint = newSprintState;
	}

	public void EnterInput(bool newEnterState)
    {
		Enter = newEnterState;
    }

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(CursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
