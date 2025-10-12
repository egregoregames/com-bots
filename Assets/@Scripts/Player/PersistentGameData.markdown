# Persistent Game Data

All game data that is saved/loaded can be accessed through this singleton, which will automatically spawn in the scene if it does not exist. It is not guaranteed to be available on Awake as it uses RunTimeInitializeOnLoad, so it is recommended to await Instance != null before getting variables. Use GetInstanceAsync to conveniently await the instance before retrieving data.

The singleton automatically hooks into save/load events on initialization. Any changes to the data will be automatically written to disk when said events are invoked.

## Testing

You can supply a singleton in the scene (and supply your own data) for testing. Just create a GameObject with this script on it, and fill in the variables as needed. You could also manually edit any generated save file, as they are encoded in JSON.

# API Reference
## Properties

### `string PlayerName { get; set; }`
- **Description**: The player's name, entered at the start of a new game. Defaults to "Player".
- **Type**: `string`
- **Access**: Read/Write

### `string PlayerStudentId { get; set; }`
- **Description**: The player's unique alphanumeric Student ID in the format `A00-A00-A00-A00`, where `A` is a letter (except 'I') and `0` is a number from 1 to 9. Generated automatically for new games.
- **Type**: `string`
- **Access**: Read/Write

### `int PlayerRankExperience { get; private set; }`
- **Description**: The player's rank experience, used to calculate their rank. Starts at 0 and increases with gameplay.
- **Type**: `int`
- **Access**: Read-only

### `Term CurrentTerm { get; set; }`
- **Description**: The player's current term, which changes after winning Promotion Battles.
- **Type**: `Term` (enum)
- **Access**: Read/Write

### `string CurrentLocationName { get; set; }`
- **Description**: The player's current location, displayed on the pause menu's homescreen and in the Socialyte App's profile. Updated when starting the game or passing through a door.
- **Type**: `string`
- **Access**: Read/Write

### `int PromotionBattleVictoryCount { get; set; }`
- **Description**: Tracks the number of Promotion Battle victories (0 to 5). Displayed in the Socialyte App's profile if the player has the "The Academy Trial" quest.
- **Type**: `int`
- **Access**: Read/Write

### `int PlayerCredits { get; set; }`
- **Description**: The total credits earned from completing quests. Displayed on the pause menu's homescreen with a '.0' suffix. Required to schedule Promotion Battles.
- **Type**: `int`
- **Access**: Read/Write

### `int PlayerMoney { get; set; }`
- **Description**: The player's in-game currency (Cybers). Starts at 0, increases to 500 upon receiving the Bank Card Key Item, and changes with purchases, sales, or fees (e.g., Omnifix, OmniRide). Displayed on the pause menu and during interactions with Shopkeepers or services.
- **Type**: `int`
- **Access**: Read/Write

### `int PlayerBattlePoints { get; set; }`
- **Description**: Currency used exclusively for purchasing items at the Arena. Starts at 0, increases with Arena battle wins, and decreases with purchases. Displayed only when interacting with Arena Shopkeepers.
- **Type**: `int`
- **Access**: Read/Write

### `List<string> PlayerNpcTeamMembers { get; private set; }`
- **Description**: A list of unique NPC IDs currently in the player's team (maximum of 2).
- **Type**: `List<string>`
- **Access**: Read-only

### `List<string> PlayerNpcConnections { get; private set; }`
- **Description**: A list of unique NPC IDs connected with the player on the Socialyte App.
- **Type**: `List<string>`
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

## Methods

### `async Task<PersistentGameData> GetInstanceAsync()`
- **Description**: Awaits the instance not being null, then returns the instanced singleton of this class
- **Parameters**: None
- **Returns**: The instanced singleton of this class

### `void SetTimeOfDay(int hour, int minute, int second = 0)`
- **Description**: Sets the in-game time of day.
- **Parameters**:
  - `hour` (`int`): Hour of the day (0 to 23).
  - `minute` (`int`): Minute of the hour (0 to 59).
  - `second` (`int`): Second of the minute (0 to 59, default is 0).
- **Returns**: None

### `void SetDayOfWeek(DayOfWeek dayOfWeek)`
- **Description**: Sets the in-game day of the week.
- **Parameters**:
  - `dayOfWeek` (`DayOfWeek`): The desired day of the week (enum).
- **Returns**: None

### `DateTime GetCurrentDateTime()`
- **Description**: Retrieves the current in-game DateTime.
- **Parameters**: None
- **Returns**: `DateTime` - The stored in-game DateTime.

### `void AddPlayerRankExperience(int amount)`
- **Description**: Adds the specified amount to the player's rank experience and saves the updated value.
- **Parameters**:
  - `amount` (`int`): The amount of experience to add.
- **Returns**: None