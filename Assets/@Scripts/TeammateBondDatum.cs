using UnityEngine;

[System.Serializable]
public class TeammateBondDatum
{
    [field: SerializeField]
    public string TeammateNpcId { get; set; }

    [field: SerializeField]
    public TeammateBond TeammateBond { get; set; }
}