using System;
using UnityEngine;

[Serializable]
public class InventoryItemDatum
{
    [field: SerializeField]
    public string ItemId { get; set; }

    [field: SerializeField]
    public int Quantity { get; set; }
}