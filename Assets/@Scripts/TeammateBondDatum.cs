using UnityEngine;

[System.Serializable]
public class TeammateBondDatum
{
    [field: SerializeField]
    public int NpcId { get; set; }

    [field: SerializeField]
    public TeammateBond TeammateBond { get; set; }
}