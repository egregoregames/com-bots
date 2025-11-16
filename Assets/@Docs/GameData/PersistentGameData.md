# Persistent Game Data

All game data that is saved/loaded can be accessed through this singleton, which will automatically spawn in the scene if it does not exist. It is not guaranteed to be available on Awake as it uses RunTimeInitializeOnLoad, so it is recommended to await Instance != null before getting variables. Use GetInstanceAsync to conveniently await the instance before retrieving data.

The singleton automatically hooks into save/load events on initialization. Any changes to the data will be automatically written to disk when said events are invoked.

## Testing

You can supply a singleton in the scene (and supply your own data) for testing. Just create a GameObject with this script on it, and fill in the variables as needed.

# API Reference
## Properties

### `static PersistentGameData Instance { get; private set; }`
- **Description**: Provides access to the singleton instance of `PersistentGameData`.
- **Type**: `PersistentGameData`
- **Access**: Read-only

### `string PlayerName { get; set; }`
- **Description**: The player's name, entered at the start of a new game. Defaults to "Player".
- **Type**: `string`
- **Access**: Read/Write

### `string PlayerStudentId { get; set; }`
- **Description**: The player's unique alphanumeric Student ID in the format `A00-A00-A00-A00`, where `A` is a letter (except 'I') and `0` is a number from 1 to 9. Generated automatically for new games.
- **Type**: `string`
- **Access**: Read/Write

### `int PlayerRankExperience { get; private set; }`
- **Description**: The player's rank experience, used to calculate their rank. Starts at 64.
- **Type**: `int`
- **Access**: Read-only

### `Term CurrentTerm { get; private set; }`
- **Description**: The player's current term, which changes after winning Promotion Battles.
- **Type**: `Term` (enum)
- **Access**: Read-only

### `string CurrentLocationName { get; private set; }`
- **Description**: The player's current location, displayed on the pause menu's homescreen and in the Socialyte App's profile. Updated when starting the game or passing through a door.
- **Type**: `string`
- **Access**: Read-only

### `int PromotionBattleVictoryCount { get; set; }`
- **Description**: Tracks the number of Promotion Battle victories (0 to 5). Displayed in the Socialyte App's profile if the player has the "The Academy Trial" quest.
- **Type**: `int`
- **Access**: Read/Write

### `int PlayerCredits { get; private set; }`
- **Description**: The total credits earned from completing quests. Displayed on the pause menu's homescreen with a '.0' suffix. Required to schedule Promotion Battles.
- **Type**: `int`
- **Access**: Read-only

### `int PlayerMoney { get; private set; }`
- **Description**: The player's in-game currency (Cybers). Starts at 0, increases to 500 upon receiving the Bank Card Key Item, and changes with purchases, sales, or fees (e.g., Omnifix, OmniRide). Displayed on the pause menu and during interactions with Shopkeepers or services.
- **Type**: `int`
- **Access**: Read-only

### `int PlayerBattlePoints { get; set; }`
- **Description**: Currency used exclusively for purchasing items at the Arena. Starts at 0, increases with Arena battle wins, and decreases with purchases. Displayed only when interacting with Arena Shopkeepers.
- **Type**: `int`
- **Access**: Read/Write

### `List<int> PlayerNpcTeamMembers { get; private set; }`
- **Description**: A list of NPC unique IDs currently in the player's team (maximum of 2).
- **Type**: `List<int>`
- **Access**: Read-only

### `List<int> PlayerNpcConnections { get; private set; }`
- **Description**: A list of NPC unique IDs connected with the player on the Socialyte App.
- **Type**: `List<int>`
- **Access**: Read-only

### `List<InventoryItemDatum> PlayerInventoryItemData { get; private set; }`
- **Description**: The player's inventory, containing Item IDs and their quantities.
- **Type**: `List<InventoryItemDatum>`
- **Access**: Read-only

### `List<QuestTrackingDatum> PlayerQuestTrackingData { get; private set; }`
- **Description**: Tracks quests the player has accepted and their current status.
- **Type**: `List<QuestTrackingDatum>`
- **Access**: Read-only

### `List<TeammateBondDatum> PlayerTeammateBonds { get; private set; }`
- **Description**: Tracks the bond levels of current or former NPC teammates. NPCs not in this list have a bond level of 0.
- **Type**: `List<TeammateBondDatum>`
- **Access**: Read-only

### `List<PlayerBlueprintDatum> PlayerBlueprintData { get; private set; }`
- **Description**: Tracks blueprints the player has seen or not seen and their status. Unlisted blueprints are considered unseen.
- **Type**: `List<PlayerBlueprintDatum>`
- **Access**: Read-only

### `List<PlayerSoftwareOwnershipDatum> PlayerOwnedSoftware { get; private set; }`
- **Description**: Tracks software owned by the player. Unlisted software is considered unowned.
- **Type**: `List<PlayerSoftwareOwnershipDatum>`
- **Access**: Read-only

### `List<BotStatusDatum> PlayerTeamBotStatusData { get; private set; }`
- **Description**: Tracks the status of the player's team bots (up to 3 entries: index 0 for the player's bot, indices 1 and 2 for teammates' bots).
- **Type**: `List<BotStatusDatum>`
- **Access**: Read-only

### `List<string> PlayerUnlockedCybercastChannelIds { get; private set; }`
- **Description**: Tracks unlocked Cybercast App channels. Starts with 3 visible channels; up to 5 more can be unlocked. Additional episodes within channels may also unlock over time.
- **Type**: `List<string>`
- **Access**: Read-only

## Static Methods

### `static async Task<int> GetPlayerRank()`
- **Description**: Calculates and returns the player's current rank based on `PlayerRankExperience`. A new player starts at rank 5.
- **Returns**: `Task<int>` - The player's rank.

### `static async Task<float> GetProgressToNextRank()`
- **Description**: Calculates the progress toward the next rank as a percentage (0 to 1). Used for UI sliders.
- **Returns**: `Task<float>` - Progress value between 0 and 1.

### `static async void SetTerm(Term term)`
- **Description**: Sets the current in-game term and invokes the `OnTermUpdated` event.
- **Parameters**:
  - `term` (`Term`): The new term to set.

### `static async Task<PersistentGameData> GetInstanceAsync()`
- **Description**: Asynchronously waits for and returns the singleton instance of `PersistentGameData`.
- **Returns**: `Task<PersistentGameData>` - The singleton instance.

### `static async void UpdateQuest(int questId, int currentStep)`
- **Description**: Updates or adds a quest with the specified progress step. Step 100 completes the quest. Automatically manages active quest selection.
- **Parameters**:
  - `questId` (`int`): The ID of the quest.
  - `currentStep` (`int`): Progress step (0–100).

### `static async void QuestForceActive(int questId)`
- **Description**: Forces a specific quest to be active in the Planner App, deactivating all others. Does nothing if the quest is completed.
- **Parameters**:
  - `questId` (`int`): The ID of the quest to activate.

### `static async void AddInventoryItem(int itemId, int amount)`
- **Description**: Adds items to the player's inventory. Clamps to max quantity and logs a warning if exceeded.
- **Parameters**:
  - `itemId` (`int`): The item ID.
  - `amount` (`int`): The quantity to add.

### `static async void RemoveInventoryItem(int itemId, int amount)`
- **Description**: Removes items from the player's inventory. Clamps to 0 and logs a warning if underflow occurs.
- **Parameters**:
  - `itemId` (`int`): The item ID.
  - `amount` (`int`): The quantity to remove.

### `static async void AddSocialyteConnection(int npcId)`
- **Description**: Adds an NPC to the player's Socialyte connections. Logs a warning if already connected.
- **Parameters**:
  - `npcId` (`int`): The NPC's profile ID.

### `static int GetInventoryAmount(int itemId)`
- **Description**: Returns the current quantity of a specific item in the player's inventory.
- **Parameters**:
  - `itemId` (`int`): The item ID.
- **Returns**: `int` - The quantity (0 if not in inventory).

## Instance Methods

### `void AddPlayerRankExperience(int amount)`
- **Description**: Adds experience to the player's rank and fires the `OnRankXpUpdated` event.
- **Parameters**:
  - `amount` (`int`): The amount of experience to add.

### `void AddPlayerCredits(int amount)`
- **Description**: Adds credits and fires the `OnCreditsUpdated` event.
- **Parameters**:
  - `amount` (`int`): The amount of credits to add.

### `void DeductPlayerCredits(int amount)`
- **Description**: Deducts credits and fires the `OnCreditsUpdated` event. Should check balance first.
- **Parameters**:
  - `amount` (`int`): The amount of credits to deduct.

### `void AddPlayerMoney(int amount)`
- **Description**: Adds money (Cybers) and fires the `OnMoneyUpdated` event.
- **Parameters**:
  - `amount` (`int`): The amount of money to add.

### `void DeductPlayerMoney(int amount)`
- **Description**: Deducts money (Cybers) and fires the `OnMoneyUpdated` event.
- **Parameters**:
  - `amount` (`int`): The amount of money to deduct.

### `void UpdateLocationName(string locationName)`
- **Description**: Sets the current location name and fires the `OnLocationUpdated` event.
- **Parameters**:
  - `locationName` (`string`): The name of the new location.