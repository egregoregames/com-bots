using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Input", menuName = "Game/Input")]
public class InputSO : ScriptableObject
{
    public Action switchToPlayerInput;
    public Action switchToUIInput;

    public void SwitchToPlayerInput()
    {
        switchToPlayerInput?.Invoke();
    }

    public void SwitchToUIInput()
    {
        switchToUIInput?.Invoke();
    }
    
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool submit;
    public bool up;
    public bool down;

    public Action OnUp;
    public Action OnDown;
    public Action OnSubmit;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
}
