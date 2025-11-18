using System;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// Used when an app is closed
    /// </summary>
    [field: SerializeField, 
        Tooltip("Used when an app is closed")]
    private AudioClip AudioClipLeaveMenu { get; set; }

    /// <summary>
    /// Used when navigating through menus in Pause Menu apps
    /// </summary>
    [field: SerializeField, 
        Tooltip("Used when navigating through menus in Pause Menu apps")]
    private AudioClip AudioClipNavigation { get; set; }

    /// <summary>
    /// Used when an action occurs, like setting an active quest
    /// </summary>
    [field: SerializeField, 
        Tooltip("Used when an action occurs, like setting an active quest")]
    private AudioClip AudioClipSubmit { get; set; }

    [field: SerializeField]
    private AudioClip AudioClipMenuOpened { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            Inputs.UI_Cancel(_ => Close(playSound: true)),
            Inputs.UI_OpenMenu(_ => Close(playSound: false))
        );
    }

    public void PlaySoundMenuOpened()
    {
        AudioManager.PlaySoundEffect(AudioClipMenuOpened);
    }

    private void Close(bool playSound)
    {
        if (!gameObject.activeInHierarchy) return;

        if (playSound)
        {
            AudioManager.PlaySoundEffect(AudioClipLeaveMenu);
        }

        gameObject.SetActive(false);
    }

    public void PlaySoundNavigation()
    {
        AudioManager.PlaySoundEffect(AudioClipNavigation);
    }

    public void PlaySoundSubmit()
    {
        AudioManager.PlaySoundEffect(AudioClipSubmit);
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