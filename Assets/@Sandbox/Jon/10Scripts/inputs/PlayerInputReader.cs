	using System;
using UnityEngine;
using UnityEngine.Serialization;
using ComBots.Logs;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class PlayerInputReader : MonoBehaviour
	{
		[SerializeField] InputSO inputSo;
		[SerializeField] PlayerInput playerInput;

		private const string INPUT_SUBMIT = "Submit";
		private const string INPUT_INTERACT = "Interact";
		private const string INPUT_UP = "Up";
		private const string INPUT_DOWN = "Down";
		private const string INPUT_SPRINT = "Sprint";
		private const string INPUT_OPEN_MENU = "Open Menu";
		private const string INPUT_CANCEL = "Cancel";
		private const string INPUT_LEFT = "Left";
		private const string INPUT_RIGHT = "Right";
		private const string INPUT_MOVE = "Move";
		private const string INPUT_JUMP = "Jump";
		private const string INPUT_NAVIGATE = "Navigate";
		private const string INPUT_LOOK = "Look";
		

		private void Awake()
		{
			inputSo.switchToPlayerInput += () => playerInput.SwitchCurrentActionMap("Player");
			inputSo.switchToPlayerInput += () => playerInput.actions.FindActionMap("UI").Disable();


			inputSo.switchToUIInput += () => playerInput.SwitchCurrentActionMap("UI");
			inputSo.switchToUIInput += () => playerInput.actions.FindActionMap("Player").Disable();
			//inputSo.switchToUIInput += () => Debug.Log("Current Action Map: " + playerInput.currentActionMap.name);
		}

		private void Start()
		{
			SetInputToPlayer();
		}

		[ContextMenu("Set Input to player only")]
		public void SetInputToPlayer()
		{
			playerInput.SwitchCurrentActionMap("Player");
			playerInput.actions.FindActionMap("UI").Disable();
		}

		void OnOpenMenu(InputValue value)
		{
			inputSo.openMenu = value.isPressed;
		}
		
		void OnCancel(InputValue value)
		{
			inputSo.cancel = value.isPressed;
		}

		void OnInteract(InputValue value)
		{
			inputSo.interact = value.isPressed;
		}
		//yield return new WaitUntil(() => playerInput.actions["Submit"].WasPressedThisFrame());

		public void OnJump(InputValue value)
		{
			inputSo.jump = value.isPressed;
		}

		public void OnSprint(InputValue value)
		{
			inputSo.sprint = value.isPressed;
		}

		public void OnNavigate(InputValue value)
		{
			inputSo.navigate = value.Get<Vector2>();
		}

		// public void OnSubmit(InputValue value)
		// {
		// 	inputSo.submit = value.isPressed;
		// }

		private void Update()
		{
			inputSo.submit = playerInput.actions[INPUT_SUBMIT].WasPressedThisFrame();
			inputSo.interact = playerInput.actions[INPUT_INTERACT].WasPressedThisFrame();
			inputSo.up = playerInput.actions[INPUT_UP].WasPressedThisFrame();
			inputSo.down = playerInput.actions[INPUT_DOWN].WasPressedThisFrame();
			inputSo.sprint = playerInput.actions[INPUT_SPRINT].IsPressed();
			inputSo.openMenu = playerInput.actions[INPUT_OPEN_MENU].WasPressedThisFrame();
			inputSo.cancel = playerInput.actions[INPUT_CANCEL].WasPressedThisFrame();
			inputSo.left = playerInput.actions[INPUT_LEFT].WasPressedThisFrame();
			inputSo.right = playerInput.actions[INPUT_RIGHT].WasPressedThisFrame();
			inputSo.Move = playerInput.actions[INPUT_MOVE].ReadValue<Vector2>();

			if (inputSo.cursorInputForLook)
			{
				inputSo.look = playerInput.actions[INPUT_LOOK].ReadValue<Vector2>();
			}

			if (inputSo.openMenu)
			{
				inputSo.OnOpenMenu?.Invoke();
			}
			if (inputSo.down)
			{
				inputSo.OnDown?.Invoke();
			}
			if (inputSo.submit)
			{
				inputSo.OnSubmit?.Invoke();
			}
			if (inputSo.up)
			{
				inputSo.OnUp?.Invoke();
			}
			if (inputSo.interact)
			{
				inputSo.OnInteract?.Invoke();
			}
			if (inputSo.cancel)
			{
				if (inputSo.AltCancel == null)
				{
					inputSo.OnCancel?.Invoke();
				}
				else
				{
					inputSo.AltCancel.Invoke();
				}
			}
			if (inputSo.left)
			{
				inputSo.OnLeft?.Invoke();
			}
			if (inputSo.right)
			{
				inputSo.OnRight?.Invoke();
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