using System;
using UnityEngine;
using UnityEngine.Serialization;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class PlayerInputReader : MonoBehaviour
	{
		[SerializeField] InputSO inputSo;
		[SerializeField] PlayerInput playerInput;

		private void Awake()
		{
			inputSo.switchToPlayerInput += () => playerInput.SwitchCurrentActionMap("Player");
			inputSo.switchToUIInput += () => playerInput.SwitchCurrentActionMap("UI");

		}

		void OnMove(InputValue value)
		{ 
			inputSo.move = value.Get<Vector2>();
		}
		//yield return new WaitUntil(() => playerInput.actions["Submit"].WasPressedThisFrame());

		public void OnLook(InputValue value)
		{
			if(inputSo.cursorInputForLook)
			{
				inputSo.look = value.Get<Vector2>();
			}
		}

		public void OnJump(InputValue value)
		{
			inputSo.jump = value.isPressed;
		}

		public void OnSprint(InputValue value)
		{
			inputSo.sprint = value.isPressed;
		}
		// public void OnSubmit(InputValue value)
		// {
		// 	inputSo.submit = value.isPressed;
		// }

		private void Update()
		{
			inputSo.submit = playerInput.actions["Submit"].WasPressedThisFrame();
			inputSo.up = playerInput.actions["Up"].WasPressedThisFrame();
			inputSo.down = playerInput.actions["Down"].WasPressedThisFrame();
			inputSo.sprint = playerInput.actions["Sprint"].IsPressed();
			if (inputSo.down)
			{
				inputSo.OnDown.Invoke();
			}
			if (inputSo.submit)
			{
				inputSo.OnSubmit.Invoke();
			}
			if (inputSo.up)
			{
				inputSo.OnUp.Invoke();
			}

		}

		// public void OnUp(InputValue value)
		// {
		// 	inputSo.up = value.isPressed;
		// }
		// public void OnDown(InputValue value)
		// {
		// 	inputSo.down = value.isPressed;
		// }
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(inputSo.cursorLocked);
		}
		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}