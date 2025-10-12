using UnityEngine;

/// <summary>
/// DTO used in saving/loading. A list of this is stored in 
/// <see cref="PersistentGameData"/> and defines the player's seen/owned blueprints
/// </summary>
[System.Serializable]
public class PlayerBlueprintDatum
{
    /// <summary>
    /// Refers to a unique Id that defines the blueprint. Is not unique to this instance
    /// </summary>
    [field: SerializeField]
    public string BlueprintId { get; set; }

    [field: SerializeField]
    public BlueprintStatus Status { get; set; }
}