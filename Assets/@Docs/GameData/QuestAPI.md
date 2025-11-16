# Quests

The entire Quest API is two simple methods. Both calls will automatically
add the quest to the player's tracked quests if it's not already there.

All changes will automatically reflect in the Planner App.

## Update

Update a quest's current step.

```csharp
PersistentGameData.UpdateQuest([questId], [currentStep]);
```

Set the currentStep to update the quest description in the Planner App. See
StaticGameData and the individual Quest data scriptable objects for reference.

Setting currentStep to 100 will complete the quest. A different incomplete quest will be 
automatically made active, going through Requirements first, in order of quest 
ID, then Electives.

## Force Active

Force a quest to be the active quest.

```csharp
PersistentGameData.QuestForceActive([questId]);
```

Sets this quest as the active quest in the Planner App. This should be called
specifically if you need to make a quest active without the player's input.

This should NOT be used when the player is manually setting a quest active
in the Planner App (there is already separate code for that).

This should NOT be used when adding the very first quest to the player's
tracked quests (that will happen automatically).

This should NOT be used immediately after a quest has been completed and you
want to set a new active quest (this happens automatically).

### Notes

- Quest IDs between 0 and 999 are Requirements, while Quest IDs greater than or
equal to 1000 are Electives

- Keep in mind that both API requests in this document are asynchronous in nature,
though it would be vanishingly rare if not impossible for race conditions to arise
as a result of these calls. You can treat them as synchronous requests

- You can subscribe to `PersistentGameData.GameEvents.OnQuestUpdated`, which
fires when either API call completes and passes the updated `QuestTrackingDatum`