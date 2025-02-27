using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UI So", menuName = "Game/UI")]
public class UISo : ScriptableObject
{
    [SerializeField] StageDoorTransitions stageDoorTransitions;
    
    public Action<string[]> OnPushDialogue;
    
    public Action<Room[], Action<int>> OnSelectionPortal;

    public Action<Action, string> AreaSelected;
    
    public Action<AudioClip> SoundSelected;

    public Action<Room> OnRoomTransition;


}
