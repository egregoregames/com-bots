using System;
using UnityEngine;

[Serializable]
public class NpcConnectionDatum
{
    [field: SerializeField]
    public int NpcId { get; set; }

    [field: SerializeField]
    public bool HasNewUpdates { get; set; } = true;
}
