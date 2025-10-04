# ComBotsSaveSystem API Reference

This API reference documents the public members of the `ComBotsSaveSystem` class, which handles saving and loading game data for the ComBots game in Unity. This system is designed to manage save files, including auto-saves and backups, and provides event-based hooks for integration with other game systems.

## Class: ComBotsSaveSystem

### Properties

- **IsOperationInProgress** (`bool`, read-only)
  - Indicates whether a save or load operation is currently in progress. Check this property before initiating save or load operations to avoid conflicts.

### Methods

- **OnWillSave(Action x)**: `IDisposable`
  - Subscribes an action to be called when a save operation (manual or auto-save) is about to start.
  - **Parameters**:
    - `x`: The action to invoke when the save operation is initiated.
  - **Returns**: A disposable object for use in observable patterns to unsubscribe the action.
  - **Remarks**: Useful for preparing game state or data before saving.

- **OnSaveComplete(Action x)**: `IDisposable`
  - Subscribes an action to be called after a save operation completes, regardless of success or failure.
  - **Parameters**:
    - `x`: The action to invoke when the save operation completes.
  - **Returns**: A disposable object for use in observable patterns to unsubscribe the action.
  - **Remarks**: Useful for re-enabling UI elements or controls disabled during the save process. Check `IsOperationInProgress` to confirm operation status.

- **OnLoadStarted(Action x)**: `IDisposable`
  - Subscribes an action to be called when a load operation is initiated, before any data is read.
  - **Parameters**:
    - `x`: The action to invoke when the load operation starts.
  - **Returns**: A disposable object for use in observable patterns to unsubscribe the action.
  - **Remarks**: Useful for disabling UI elements or controls during the loading operation.

- **OnLoadSuccess(Action x)**: `IDisposable`
  - Subscribes an action to be called after a successful load operation.
  - **Parameters**:
    - `x`: The action to invoke when the load operation succeeds.
  - **Returns**: A disposable object for use in observable patterns to unsubscribe the action.
  - **Remarks**: This event is not triggered if the load operation fails, preserving existing data for listeners.

- **LoadSavedData<T>(string key, ref T value, object defaultValue = null)**: `void`
  - Retrieves data from the loaded save file using a key, updating the provided value.
  - **Type Parameters**:
    - `T`: The type of the value to load (should be a primitive type like `int`, `float`, `bool`, or `string`).
  - **Parameters**:
    - `key`: The unique key for the data, matching the key used in `SendSaveData`.
    - `value`: The variable to store the loaded data (passed by reference).
    - `defaultValue`: The value to use if the key does not exist in the save data (e.g., for older save files).
  - **Remarks**: Should be called after a successful load operation to retrieve saved data.

- **Load(string name)**: `Task<OperationResult>`
  - Loads a save file from disk.
  - **Parameters**:
    - `name`: The name of the save file to load (must match the name used in `Save`).
  - **Returns**: A `Task` containing an `OperationResult` indicating success or failure.
  - **Exceptions**:
    - `Exception`: Thrown if `IsOperationInProgress` is `true`.
  - **Remarks**: Typically invoked by UI elements. Returns a failure result if the file does not exist or cannot be read/parsed.

- **AutoSave()**: `Task<OperationResult>`
  - Performs an auto-save operation, cycling through a limited number of auto-save slots.
  - **Returns**: A `Task` containing an `OperationResult` indicating success or failure.
  - **Remarks**: Automatically manages auto-save numbering and invokes `Save` with an appropriate file name.

- **Save(string name)**: `Task<OperationResult>`
  - Saves game data to a file on disk.
  - **Parameters**:
    - `name`: The name of the save file. Must not contain invalid filename characters (use `IsValidFileName` to check).
  - **Returns**: A `Task` containing an `OperationResult` indicating success or failure.
  - **Exceptions**:
    - `ArgumentException`: Thrown if the file name contains invalid characters.
    - `Exception`: Thrown if `IsOperationInProgress` is `true`.
  - **Remarks**: Typically invoked by UI or auto-save mechanisms. Triggers `OnWillSave` to collect data and creates a backup of existing save files.

- **SendSaveData(string key, object value)**: `void`
  - Submits data to be saved during a save operation.
  - **Parameters**:
    - `key`: A unique key for the data.
    - `value`: The data to save (should be a primitive type like `int`, `float`, `bool`, or `string`).
  - **Exceptions**:
    - `Exception`: Thrown if called when `IsOperationInProgress` is `false`.
  - **Remarks**: Should only be called during a save operation (when `IsOperationInProgress` is `true`). Overwrites existing data for the same key with a warning.

- **IsValidFileName(string name)**: `bool`
  - Checks if a file name is valid for saving.
  - **Parameters**:
    - `name`: The file name to validate.
  - **Returns**: `true` if the file name is valid; `false` if it contains invalid characters.
  - **Remarks**: Use before calling `Save` to ensure the file name is valid.

## Class: OperationResult

### Properties

- **ResultType** (`ResultType`, read-only)
  - The type of result (e.g., `Success` or `Fail`) for a save or load operation.
- **Message** (`string`, read-only)
  - A message providing details about the operation result, typically used for error descriptions.

### Constructor

- **OperationResult(ResultType resultType, string message = "")**
  - Initializes a new instance of `OperationResult`.
  - **Parameters**:
    - `resultType`: The type of result (`Success` or `Fail`).
    - `message`: An optional message describing the result (default is an empty string).

## Enum: ResultType

- **Success**
  - Indicates a successful save or load operation.
- **Fail**
  - Indicates a failed save or load operation.