using UnityEngine;

[System.Serializable]
public class PlayerSoftwareOwnershipDatum
{
    [field: SerializeField]
    public string SoftwareId { get; set; }

    [field: SerializeField]
    public SoftwareOwnershipStatus Status { get; set; }
}