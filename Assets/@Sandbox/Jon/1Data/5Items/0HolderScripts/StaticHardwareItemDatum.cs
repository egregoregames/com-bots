using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(
    fileName = "StaticHardwareItemDatum", 
    menuName = "Scriptable Objects/Static Hardware Item Datum")]
public class StaticHardwareItemDatum : StaticItemDatum
{
    [field: SerializeField, FormerlySerializedAs("hardwareType")]
    public HardwareType HardwareType { get; set; }

    [field: SerializeField, FormerlySerializedAs("craftable")]
    public bool IsCraftable { get; set; }

    [field: SerializeField, FormerlySerializedAs("weight")]
	public int Weight { get; set; }
}