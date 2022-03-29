using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInput : MonoBehaviour
{
	// Character Input Values
	public Vector2 Move { get; set; }
	public Vector2 Look { get; set; }
	public bool Jump { get; set; }
	public bool Sprint { get; set; }

	// Mouse Cursor Settings
	public bool CursorLocked { get; set; } = true;
	public bool CursorInputForLook { get; set; } = true;


    #region Input Events
    public void OnMove(InputAction.CallbackContext value)
	{
		MoveInput(value.ReadValue<Vector2>());
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
		JumpInput(value.action.triggered);
	}

	public void OnSprint(InputAction.CallbackContext value)
	{
		SprintInput(value.action.ReadValue<float>() == 1);
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

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(CursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
