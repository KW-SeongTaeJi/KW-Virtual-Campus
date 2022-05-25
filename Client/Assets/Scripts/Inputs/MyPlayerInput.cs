using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInput : MonoBehaviour
{
	// Keyboard Input Values
	public Vector2 Move { get; set; }
	public Vector2 Look { get; set; }
	public bool Jump { get; set; }
	public bool Sprint { get; set; }
	public bool Enter { get; set; }
	public bool Tap { get; set; }
	public bool One { get; set; }
	public bool Two { get; set; }
	public bool Three { get; set; }
	public bool Four { get; set; }
	public bool Five { get; set; }
	public bool Six { get; set; }


	// Mouse Cursor Settings
	bool _cursorLock;
	public bool CursorLock 
	{ 
		get { return _cursorLock; }
        set
        {
			_cursorLock = value;
			if (_cursorLock == true)
				Cursor.lockState = CursorLockMode.Locked;
			else
				Cursor.lockState = CursorLockMode.None;
		} 
	}

	UI_GameScene _gameSceneUI;


    private void Awake()
    {
		_gameSceneUI = (UI_GameScene)Managers.UI.SceneUI;

		CursorLock = true;
	}

	#region Input Events
	public void OnMove(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.ChatInputField.isFocused == false)
			MoveInput(value.ReadValue<Vector2>());
		else
			MoveInput(new Vector2(0, 0));
	}

	public void OnLook(InputAction.CallbackContext value)
	{
		if (CursorLock && (_gameSceneUI.EmotionPanel.activeSelf == false))
		{
			LookInput(value.ReadValue<Vector2>());
		}
		else
        {
			LookInput(new Vector2(0, 0));
        }
	}

	public void OnJump(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.ChatInputField.isFocused == false)
			JumpInput(value.action.triggered);
		else
			JumpInput(false);
	}

	public void OnSprint(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.ChatInputField.isFocused == false)
			SprintInput(value.action.ReadValue<float>() == 1);
		else
			SprintInput(false);
	}

	public void OnEnter(InputAction.CallbackContext value)
    {
		EnterInput(value.action.triggered);
    }

	public void OnTap(InputAction.CallbackContext value)
	{
		TapInput(value.action.ReadValue<float>() == 1);
	}

	public void OnOne(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.Alert2Popup == null && _gameSceneUI.ChatInputField.isFocused == false)
			OneInput(value.action.triggered);
		else
			OneInput(false);
	}
	public void OnTwo(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.Alert2Popup == null && _gameSceneUI.ChatInputField.isFocused == false)
			TwoInput(value.action.triggered);
		else
			TwoInput(false);
	}
	public void OnThree(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.Alert2Popup == null && _gameSceneUI.ChatInputField.isFocused == false)
			ThreeInput(value.action.triggered);
		else
			ThreeInput(false);
	}
	public void OnFour(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.Alert2Popup == null && _gameSceneUI.ChatInputField.isFocused == false)
			FourInput(value.action.triggered);
		else
			FourInput(false);
	}
	public void OnFive(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.Alert2Popup == null && _gameSceneUI.ChatInputField.isFocused == false)
			FiveInput(value.action.triggered);
		else
			FiveInput(false);
	}
	public void OnSix(InputAction.CallbackContext value)
	{
		if (_gameSceneUI.Alert2Popup == null && _gameSceneUI.ChatInputField.isFocused == false)
			SixInput(value.action.triggered);
		else
			SixInput(false);
	}

	public void OnCursorLock(InputAction.CallbackContext value)
	{
		CursorLockInput(value.action.triggered);
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

	public void TapInput(bool newTapState)
	{
		Tap = newTapState;
	}

	public void OneInput(bool newOneState)
    {
		One = newOneState;
    }
	public void TwoInput(bool newTwoState)
	{
		Two = newTwoState;
	}
	public void ThreeInput(bool newThreeState)
	{
		Three = newThreeState;
	}
	public void FourInput(bool newFourState)
	{
		Four = newFourState;
	}
	public void FiveInput(bool newFiveState)
	{
		Five = newFiveState;
	}
	public void SixInput(bool newSixState)
	{
		Six = newSixState;
	}

	public void CursorLockInput(bool newCursorLockState)
    {
		if (newCursorLockState == true)
        {
			if (Cursor.lockState == CursorLockMode.None)
			{
				CursorLock = true;
			}
			else
			{
				CursorLock = false;
			}
		}
	}

	//public bool CursorLocked { get; set; } = true;
	//public bool CursorInputForLook { get; set; } = true;

	//private void OnApplicationFocus()
	//{
	//}

	//private void SetCursorState(bool newState)
	//{
	//	Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	//}
}
