using UnityEngine;

[System.Serializable]
public class PlayerBlueprintDatum
{
    /// <summary>
    /// Unique ID of the blueprint
    /// </summary>
    [field: SerializeField]
    public string BlueprintId { get; set; }

    [field: SerializeField]
    public BlueprintStatus Status { get; set; }
}