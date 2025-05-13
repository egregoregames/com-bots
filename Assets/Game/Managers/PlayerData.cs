using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public List<dataHolderSocialyteProfile> KnownConnections = new();
    public List<dataHolderSoftware> CollectedSoftware = new();
    public int ownedBlueprints = 5;
    public string playerOccupation = "First-Year Student";
    public string rank  = "D";
    public string playerName = "Player";
}
