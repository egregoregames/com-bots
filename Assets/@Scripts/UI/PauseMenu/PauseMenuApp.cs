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

    [field: SerializeField]
    private AudioClip AudioClipLeaveMenu { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            Inputs.UI_Cancel(_ => Close()),
            Inputs.UI_OpenMenu(_ => Close())
        );
    }

    private void Close()
    {
        if (!gameObject.activeInHierarchy) return;
        AudioManager.PlaySoundEffect(AudioClipLeaveMenu);
        gameObject.SetActive(false);
    }

    private new void OnEnable()
    {
        base.OnEnable();
        _openMenus.Add(this);
        _onMenuOpened?.Invoke();
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        _openMenus.Remove(this);
        _onMenuClosed?.Invoke();
    }
}