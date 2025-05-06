using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UI So", menuName = "Game/UI")]
public class UISo : ScriptableObject
{
    [SerializeField] StageDoorTransitions stageDoorTransitions;
    
    public Action<string[]> OnPushDialogue;
    
    public Action<Room[], Action<Room>, string> PlayerEnteredRoomSelector;

    public Action<Action, Action, string> TriggerAreaChangeTransition;
    
    public Action<AudioClip> SoundSelected;

    public Action<bool> OnPauseStateChanged;

}
