using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "dataHolderItem", menuName = "Scriptable Objects/dataHolderItem")]
public class dataHolderItem : ScriptableObject
{
	[field: SerializeField]
	public int ItemId { get; private set; }

	[field: SerializeField, FormerlySerializedAs("itemNameSingular")]
	public string ItemName { get; private set; }

    [field: SerializeField, FormerlySerializedAs("itemNamePlural")]
    public string ItemNamePlural { get; private set; }

    [field: SerializeField, FormerlySerializedAs("itemType")]
    public ItemType Type { get; private set; }

    [field: SerializeField, FormerlySerializedAs("itemValue")]
    public int Value { get; private set; }

    /// <summary>
    /// The maximum amount of this item that can exist in the player's 
    /// inventory/backpack. Most key items should be 1. Most other items 
    /// should be 999
    /// </summary>
    [field: SerializeField]
    public int MaxQuantity { get; private set; }

    [field: SerializeField, FormerlySerializedAs("sellable")]
    public bool IsSellable { get; private set; }

    [field: SerializeField, FormerlySerializedAs("usableInOverworld")]
	public bool IsUsableInOverworld { get; private set; }

    [field: SerializeField, FormerlySerializedAs("targetRequired")]
    public bool NeedsTarget { get; private set; }

    [field: SerializeField, FormerlySerializedAs("fromLitter")]
    public bool FromLitter { get; private set; }

    [field: SerializeField, FormerlySerializedAs("fromBrokenPart")]
    public bool FromBrokenPart { get; private set; }

    [field: SerializeField, FormerlySerializedAs("fromCorruptDisc")]
    public bool FromCorruptDisc { get; private set; }

    [field: SerializeField, FormerlySerializedAs("flavorText"), TextArea(3, 10)]
    public string FlavorText { get; private set; }

    [field: SerializeField, FormerlySerializedAs("image")]
    public Texture2D Image { get; private set; }
}