using System;
using UnityEngine;

/// <summary>
/// DTO used for saving/loading. A list of this is stored in 
/// <see cref="PersistentGameData"/> and defines the player's inventory 
/// with index == slot number. When adding to inventory, the list should be 
/// searched to see if the item already exists and increment the quantity.
/// </summary>
[Serializable]
public class InventoryItemDatum
{
    /// <summary>
    /// Refers to a unique Id that defines the item. Is not unique to this instance
    /// </summary>
    [field: SerializeField]
    public string ItemId { get; set; }

    /// <summary>
    /// Key items will have a max inventory quantity of 1
    /// </summary>
    [field: SerializeField]
    public int Quantity { get; set; }
}