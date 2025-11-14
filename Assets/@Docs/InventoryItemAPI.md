# Inventory Items

The entire Inventory API is two simple methods.

All changes will automatically reflect in the BackPack app.

## Add

Add an item to the player's inventory.

```csharp
PersistentGameData.AddInventoryItem([itemId], [amount]);
```

Check the static game data of a particular item to be sure that amount will
not exceed the max quantity. The game will log a warning if you do this, and it
will be a negative player experience and a bug if it happens in a shop setting.

Check the static game data and compare to the user's current amount like so:

```csharp
var max = StaticGameData.GetMaxInventoryItemQuantity([itemId]);
var current = PersistentGameData.GetInventoryAmount([itemId]);

var maxPlayerCanBuy = max - current;
bool canPlayerBuy = current < max;
```

## Remove

Remove an item from the player's inventory.

```csharp
PersistentGameData.RemoveInventoryItem([itemId], [amount]);
```

Check the static game data of a particular item to be sure the final amount will 
not bring the quantity to lower than 0. The game will log a warning if you do this, and it
will be a negative player experience and a bug if it happens in a shop setting.

Check the persistent game data:

```csharp
var current = PersistentGameData.GetInventoryAmount([itemId]);

var maxPlayerCanSell = current;
bool canPlayerSell = current > 0;
```

This method will also log a warning if the item does not exist in the player's
inventory yet (never acquired).

## Get amount

Returns an integer representing how many of an itemId the user has.

```csharp
var amount = PersistentGameData.GetInventoryAmount([itemId]);
```

### Notes

- You can subscribe to `PersistentGameData.GameEvents.OnInventoryItemUpdated`, which
fires when the add or remove API call completes and passes the updated `InventoryItemDatum`