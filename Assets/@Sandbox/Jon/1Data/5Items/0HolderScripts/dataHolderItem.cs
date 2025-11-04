using UnityEngine;

public enum itemType
{
	keyItem,
	battleItem,
	generalItem,
	hardware,
	part
}

[CreateAssetMenu(fileName = "dataHolderItem", menuName = "Scriptable Objects/dataHolderItem")]

public class dataHolderItem : ScriptableObject
{
	public string itemNameSingular;
	public string itemNamePlural;
	public itemType itemType;
	public int itemValue;
	public bool sellable;
	public bool usableInOverworld;
	public bool targetRequired;
	public bool fromLitter;
	public bool fromBrokenPart;
	public bool fromCorruptDisc;
	[TextArea(3, 10)]
	public string flavorText;
	public Texture2D image;	
}
