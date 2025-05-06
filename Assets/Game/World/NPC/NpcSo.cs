using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Npc", menuName = "Entity/Npc")]
public class NpcSo : ScriptableObject
{
    public string conversationKey;
    public Image portrait;
    public Image imageOverWorld;
    public bool potentialTeammate;
    public int bond;
    public string occupation;
    public string origin;
    public string bio;
}
