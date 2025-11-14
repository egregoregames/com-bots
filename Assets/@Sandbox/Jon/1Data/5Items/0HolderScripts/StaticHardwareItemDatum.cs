using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Contains immutable data for hardware items. Accessible as 
/// <see cref="StaticItemDatum"/> in <see cref="StaticGameData"/> 
/// (must be cast)
/// </summary>
[CreateAssetMenu(
    fileName = "StaticHardwareItemDatum", 
    menuName = "Scriptable Objects/Static Hardware Item Datum")]
public class StaticHardwareItemDatum : StaticItemDatum
{
    [field: SerializeField, FormerlySerializedAs("hardwareType")]
    public HardwareType HardwareType { get; private set; }

    [field: SerializeField, FormerlySerializedAs("craftable")]
    public bool IsCraftable { get; private set; }

    [field: SerializeField, FormerlySerializedAs("weight")]
	public int Weight { get; private set; }
}