# Socialyte API

All changes will automatically reflect in the Socialyte App

## Add

Add a contact in the Socialyte app.

```csharp
PersistentGameData.AddSocialyteConnection([npcId]);
```

This method will throw a warning if the user already exists. Check the list
in persistent game data first.

```csharp
var pgd = await PersistentGameData.GetInstanceAsync();
bool exists = pgd.PlayerNpcConnections.Contains(npcId);
```

### Notes

- You can subscribe to `PersistentGameData.GameEvents.OnSocialyteProfileAdded`, which
fires when a new contact is added and passes the added NPC ProfileID.