using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Input", menuName = "Game/Input")]
public class InputSO : ScriptableObject
{
    public Action switchToPlayerInput;
    public Action switchToUIInput;
    public Action<bool> setInput;
    public void SwitchToPlayerInput()
    {
        switchToPlayerInput?.Invoke();
    }

    public void SwitchToUIInput()
    {
        
        switchToUIInput?.Invoke();
    }

    public void SetInput(bool status)
    {
        setInput?.Invoke(status);
    }

    public bool CanPlayerMove = true;
    
    [Header("Character Input Values")]
    private Vector2 _move;
    public Vector2 Move
    {
        get => CanPlayerMove ? _move : Vector2.zero;
        set => _move = value;
    }
    
    public Vector2 navigate;
    public Vector2 look;
    public bool jump;
    public bool interact;

    public bool sprint;
    public bool submit;
    public bool up;
    public bool down;
    public bool openMenu;
    public bool cancel;
    public bool left;
    public bool right;

    public Action OnUp;
    public Action OnInteract;
    public Action OnCancel;
    public Action AltCancel;

    public Action OnDown;
    public Action OnSubmit;
    public Action OnOpenMenu;
    
    public Action OnLeft;
    public Action OnRight;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
}
