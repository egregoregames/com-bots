# ComBots Save System 

This static class handles saving and loading game data in the ComBots game. See the PersistentGameData.cs script for a great example of how attributes and events are easily used to save and load game data.

For the most part, only the PersistentGameData singleton will have properties that will be written to disk. Other scripts, especially UI and game event related, will call Save, Autosave and Load as needed.

High level game logic and UI display scripts will reference PersistentGameData, rather than this script, when variables are needed.

# API Reference
## Properties

### `bool IsOperationInProgress { get; private set; }`
- **Description**: Indicates whether a save or load operation is currently in progress. Should be checked before calling `Save(string)` or by listeners of `OnSaveComplete(Action)`.
- **Type**: `bool`
- **Access**: Read-only

## Methods

### `IDisposable OnWillSave(Action x)`
- **Description**: Subscribes an action to be called when a save is explicitly requested or an auto-save is triggered.
- **Parameters**:
  - `x` (`Action`): The action to invoke when the event occurs.
- **Returns**: `IDisposable` - A disposable object for unsubscribing from the event.

### `IDisposable OnSaveComplete(Action x)`
- **Description**: Subscribes an action to be called after a save operation completes, whether successful or not. Useful for re-enabling UI elements or controls disabled during saving.
- **Parameters**:
  - `x` (`Action`): The action to invoke when the event occurs.
- **Returns**: `IDisposable` - A disposable object for unsubscribing from the event.

### `IDisposable OnLoadStarted(Action x)`
- **Description**: Subscribes an action to be called when a load operation is initiated, before any data is read. Useful for disabling UI elements or controls during loading.
- **Parameters**:
  - `x` (`Action`): The action to invoke when the event occurs.
- **Returns**: `IDisposable` - A disposable object for unsubscribing from the event.

### `IDisposable OnLoadSuccess(Action x)`
- **Description**: Subscribes an action to be called after a successful load operation. Not called if the load fails, to preserve existing data.
- **Parameters**:
  - `x` (`Action`): The action to invoke when the event occurs.
- **Returns**: `IDisposable` - A disposable object for unsubscribing from the event.

### `void LoadSavedData<T>(string key, ref T value, object defaultValue = null)`
- **Description**: Retrieves saved data for a specific key and assigns it to the provided reference. Should be called after a successful load operation.
- **Parameters**:
  - `key` (`string`): The unique key for the data, matching the key used in `SaveData(string, object)`.
  - `value` (`ref T`): The variable to store the loaded data.
  - `defaultValue` (`object`, optional): The value to use if the key does not exist (e.g., for older game versions).
- **Returns**: None

### `T LoadSavedData<T>(string key, T defaultValue)`
- **Description**: Retrieves saved data for a specific key, returning the value or a default if the key does not exist. Should be called after a successful load operation.
- **Parameters**:
  - `key` (`string`): The unique key for the data, matching the key used in `SaveData(string, object)`.
  - `defaultValue` (`T`): The value to return if the key does not exist.
- **Returns**: `T` - The loaded value or the default value if the key is not found.

### `async Task<OperationResult> Load(string name)`
- **Description**: Loads a save file from disk. Should only be invoked explicitly by UI.
- **Parameters**:
  - `name` (`string`): The name of the file to load, matching the name used in `Save(string)`.
- **Returns**: `Task<OperationResult>` - The result of the load operation, indicating success or failure with a message.
- **Exceptions**:
  - `Exception`: Thrown if a save or load operation is already in progress. Check `IsOperationInProgress` first.

### `async Task<OperationResult> AutoSave()`
- **Description**: Automatically saves game data to a numbered autosave file (up to a maximum of 5). Invoked by in-game events.
- **Parameters**: None
- **Returns**: `Task<OperationResult>` - The result of the save operation, indicating success or failure with a message.

### `async Task<OperationResult> Save(string name)`
- **Description**: Saves game data to a specified file on disk. Should only be invoked explicitly by UI.
- **Parameters**:
  - `name` (`string`): The file name for the save. Must not contain invalid characters (use `IsValidFileName(string)` to check).
- **Returns**: `Task<OperationResult>` - The result of the save operation, indicating success or failure with a message.
- **Exceptions**:
  - `ArgumentException`: Thrown if the file name contains invalid characters.
  - `Exception`: Thrown if a save or load operation is already in progress. Check `IsOperationInProgress` first.

### `void SaveData(string key, object value)`
- **Description**: Saves data with a unique key for later retrieval.
- **Parameters**:
  - `key` (`string`): The unique key for the data.
  - `value` (`object`): The data to save.
- **Returns**: None

### `bool IsValidFileName(string name)`
- **Description**: Checks if a file name is valid for saving, ensuring it does not contain invalid characters.
- **Parameters**:
  - `name` (`string`): The file name to validate.
- **Returns**: `bool` - True if the file name is valid, false otherwise.

### `void LoadData(Type targetType, object instance)`
- **Description**: Automatically loads saved data into fields and properties of the specified instance or static class. Members must be marked with `ComBotsSaveAttribute`.
- **Parameters**:
  - `targetType` (`Type`): The type of the instance (use `typeof`).
  - `instance` (`object`): The instance itself (use `this`).
- **Returns**: None
- **Exceptions**:
  - `Exception`: Thrown if there are issues with the save ID or data loading.

### `void SaveData(Type targetType, object instance)`
- **Description**: Automatically saves data from fields and properties of the specified instance or static class. Members must be marked with `ComBotsSaveAttribute`.
- **Parameters**:
  - `targetType` (`Type`): The type of the instance (use `typeof`).
  - `instance` (`object`): The instance itself (use `this`).
- **Returns**: None