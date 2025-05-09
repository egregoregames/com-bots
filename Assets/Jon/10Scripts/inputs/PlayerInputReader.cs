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
		void OnMove(InputValue value)
		{ 
			inputSo.move = value.Get<Vector2>();
		}

		void OnInteract(InputValue value)
		{
			inputSo.interact = value.isPressed;
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
			inputSo.interact = playerInput.actions["Interact"].WasPressedThisFrame();
			inputSo.up = playerInput.actions["Up"].WasPressedThisFrame();
			inputSo.down = playerInput.actions["Down"].WasPressedThisFrame();
			inputSo.sprint = playerInput.actions["Sprint"].IsPressed();
			inputSo.openMenu = playerInput.actions["Open Menu"].WasPressedThisFrame();
			inputSo.cancel = playerInput.actions["Cancel"].WasPressedThisFrame();
			inputSo.left = playerInput.actions["Left"].WasPressedThisFrame();
			inputSo.right = playerInput.actions["Right"].WasPressedThisFrame();

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
				inputSo.OnCancel?.Invoke();
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