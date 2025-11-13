using System;
using System.Linq;
using System.Threading.Tasks;
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
    public int ItemId { get; set; }

    /// <summary>
    /// Key items will have a max inventory quantity of 1
    /// </summary>
    [field: SerializeField]
    public int Quantity { get; set; }

    public async Task<StaticItemDatum> GetStaticDataAsync()
    {
        return (await StaticGameData.GetInstanceAsync()).ItemData
            .First(x => x.ItemId == ItemId);
    }

    /// <summary>
    /// Make sure <see cref="StaticGameData.Instance"/> is not null or 
    /// use <see cref="GetStaticDataAsync"/>
    /// </summary>
    /// <returns></returns>
    public StaticItemDatum GetStaticData()
    {
        return StaticGameData.Instance.ItemData
            .First(x => x.ItemId == ItemId);
    }
}