using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Allows other components (<see cref="PauseMenu"/>) to easily determine if 
/// any app is open
/// </summary>
public class PauseMenuApp : MonoBehaviourR3
{
    private static List<PauseMenuApp> _openMenus = new();
    public static bool IsAnyOpen => _openMenus.Count > 0;

    private static UnityEventR3 _onMenuOpened = new();
    public static IDisposable OnMenuOpened(Action x) => _onMenuOpened.Subscribe(x);

    private static UnityEventR3 _onMenuClosed = new();
    public static IDisposable OnMenuClosed(Action x) => _onMenuClosed.Subscribe(x);

    private InputSystem_Actions Inputs { get; set; }

    protected override void Initialize()
    {
        base.Initialize();
        Inputs = new InputSystem_Actions();

        var onCancelAction = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.UI.Cancel.performed += h,
            h => Inputs.UI.Cancel.performed -= h);

        AddEvents(
            onCancelAction.Subscribe(OnCancelAction_performed)
        );
    }

    private void OnCancelAction_performed(InputAction.CallbackContext context)
    {
        //Debug.Log("PauseMenuApp.OnCancelAction_performed");
        gameObject.SetActive(false);
    }

    private new void OnEnable()
    {
        base.OnEnable();
        Inputs.Enable();
        _openMenus.Add(this);
        _onMenuOpened?.Invoke();
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        Inputs.Disable();
        _openMenus.Remove(this);
        _onMenuClosed?.Invoke();
    }
}