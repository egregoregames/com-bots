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
    public int rank  = 1;
    public int requiredRankExperience = 50;
    public int currentRankExperience = 12;
    public string playerName = "Player";
    public string termName = "Early Fall";
    public float termCredits = 10.5f;
    public int currency = 1000;
    public string currentLocation = "Beach";
}
