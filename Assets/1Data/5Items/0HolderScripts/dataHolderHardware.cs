using UnityEngine;

public enum hardwareType
{
	internalHardware,
	headgearHardware,
	armHardware,
	armorHardware,
	bootHardware
}

[CreateAssetMenu(fileName = "dataHolderHardware", menuName = "Scriptable Objects/dataHolderHardware")]

public class dataHolderHardware : ScriptableObject
{
	public string hardwareNameSingular;
	public string hardwareNamePlural;
	public hardwareType hardwareType;
	public bool craftable;
	public int weight;
	public string flavorText;
	public GameObject image;	
}
