using UnityEngine;

[System.Serializable]
public class PlayerSoftwareOwnershipDatum
{
    [field: SerializeField]
    public int SoftwareId { get; set; }

    [field: SerializeField]
    public SoftwareOwnershipStatus Status { get; set; }
}