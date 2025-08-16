using System;
using ComBots.Game.Worlds.Rooms;
using UnityEngine;

[CreateAssetMenu(fileName = "UI So", menuName = "Game/UI")]
public class UISo : ScriptableObject
{
    [SerializeField] StageDoorTransitions stageDoorTransitions;
    
    public Action<string[]> OnPushDialogue;
    
    /// <summary>
    /// Invoked to trigger a transition when the player changes areas.
    /// The first Action parameter is the action to perform after the transition,
    /// the second Action is to release player movement, and the string parameter is the area name.
    /// </summary>
    public Action<Action, Action, string> TriggerAreaChangeTransition;
    
    /// <summary>
    /// Invoked when background music needs to be changed.
    /// The AudioClip parameter is the new background music clip to play.
    /// </summary>
    public Action<AudioClip> OnBackgroundMusicSelected;

    public Action<bool> OnCameraTransition;
}
