using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Handles saving and loading of ComBots game data. Not to be confused
/// with Pixel Crusher's Save System, which will not be used in this project.
/// </summary>
public static partial class ComBotsSaveSystem
{
    private const string _operationInProgressMessage = 
        "A save/load operation is already in progress. " +
        "Check IsOperationInProgress before calling this method";

    private const string _autoSavePlayerPrefKey = "ComBotsAutoSave";

    // TODO: Make this configurable in settings
    private const int _maxAutoSaves = 5;

    private static UnityEventR3 _onWillSave = new();
    /// <summary>
    /// Called when the user explicity requests a save or when an auto-save is triggered.
    /// </summary>
    /// <param name="x"></param>
    /// <returns>A disposable meant for use in observable patterns</returns>
    public static IDisposable OnWillSave(Action x) => _onWillSave.Subscribe(x);

    private static UnityEventR3 _onSaveComplete = new();
    /// <summary>
    /// Called after the save operation has completed, even if it failed. 
    /// This event is useful for re-enabling UI elements or controls that were 
    /// disabled during the save process. See <see cref="IsOperationInProgress"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <returns>A disposable meant for use in observable patterns</returns>
    public static IDisposable OnSaveComplete(Action x) => _onSaveComplete.Subscribe(x);

    private static UnityEventR3 _onLoadStarted = new();
    /// <summary>
    /// Called when a load operation is initiated, before any data is read. 
    /// This event is useful for disabling UI elements or controls during the loading operation.
    /// </summary>
    /// <param name="x"></param>
    /// <returns>A disposable meant for use in observable patterns</returns>
    public static IDisposable OnLoadStarted(Action x) => _onLoadStarted.Subscribe(x);

    private static UnityEventR3 _onLoadSuccess = new();
    /// <summary>
    /// Called after a successful load operation. Note that this event is not 
    /// called if the load operation fails, unlike <see cref="OnSaveComplete(Action)"/>. 
    /// This is to preserve existing data for listeners of this event.
    /// </summary>
    /// <param name="x"></param>
    /// <returns>A disposable meant for use in observable patterns</returns>
    public static IDisposable OnLoadSuccess(Action x) => _onLoadSuccess.Subscribe(x);

    /// <summary>
    /// True if a save operation is currently in progress. Should be checked 
    /// by listeners of <see cref="OnSaveComplete(Action)"/>, and should also be
    /// checked before calling <see cref="Save(bool, string)"/>.
    /// </summary>
    public static bool IsOperationInProgress { get; private set; } = false;

    private static string SavePath => 
        Path.Combine(Application.persistentDataPath, "Saves");

    private static string BackupSavesPath => 
        Path.Combine(SavePath, "Backups");

    private static Dictionary<string, string> _incomingSaveData = new();
    private static Dictionary<string, string> _loadedSaveData = new();

    /// <summary>
    /// Should be called after a successful load operation to retrieve data. 
    /// Uses ref pattern for convenient one-liners.
    /// </summary>
    /// 
    /// <typeparam name="T">Should be a primitive type</typeparam>
    /// 
    /// <param name="key">
    /// Unique key for this data that should match the key used in 
    /// <see cref="SendSaveData(string, object)"/>
    /// </param>
    /// 
    /// <param name="value">Should be a primitive type</param>
    /// 
    /// <param name="defaultValue">
    /// The value to use if the saved data key 
    /// does not exist. Useful when loading a save from an older game version 
    /// that did not support this key yet
    /// </param>
    public static void LoadSavedData<T>(string key, ref T value, object defaultValue = null)
    {
        if (_loadedSaveData.ContainsKey(key))
        {
            value = (T)Convert.ChangeType(_loadedSaveData[key], typeof(T));
        }
        else if (defaultValue != null)
        {
            value = (T)defaultValue;
        }
    }

    /// <summary>
    /// Loads a save from disk. Should only ever be excplicitly invoked by UI
    /// </summary>
    /// <param name="name">
    /// The name of the file to load. Must match the file name that was used when <see cref="Save(bool, string)"/> was called
    /// </param>
    /// <returns></returns>
    /// 
    /// <exception cref="Exception">
    /// Thrown if an operation was already in progress. 
    /// Check <see cref="IsOperationInProgress"/> first
    /// </exception>
    public static async Task<OperationResult> Load(string name)
    {
        if (IsOperationInProgress)
        {
            throw new Exception(_operationInProgressMessage);
        }

        string path = Path.Combine(SavePath, name);

        if (!File.Exists(path))
        {
            return new OperationResult(
                ResultType.Fail, "Save file does not exist");
        }

        byte[] bytes;

        try
        {
            bytes = await File.ReadAllBytesAsync(path);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return new OperationResult(ResultType.Fail, 
                "Failed to read save file. See log for details");
        }

        try
        {
            _loadedSaveData = JsonSerializer
                .Deserialize<Dictionary<string, string>>(bytes);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return new OperationResult(ResultType.Fail,
                "Failed to parse save file. See log for details");
        }

        return new OperationResult(ResultType.Success);
    }

    /// <summary>
    /// Saves file to disk. Should only ever be invoked automatically by 
    /// certain in-game events. See <see cref="Save(string)"/>
    /// </summary>
    /// <returns></returns>
    public static async Task<OperationResult> AutoSave()
    {
        int autoSaveNumber = 0;

        if (PlayerPrefs.HasKey(_autoSavePlayerPrefKey))
        {
            autoSaveNumber = PlayerPrefs.GetInt(_autoSavePlayerPrefKey);
        }

        if (autoSaveNumber >= _maxAutoSaves)
        {
            autoSaveNumber = 0;
        }

        PlayerPrefs.SetInt(_autoSavePlayerPrefKey, ++autoSaveNumber);

        return await Save($"autosave_{autoSaveNumber}");
    }

    /// <summary>
    /// Saves file to disk. Outside of this script, should only ever be 
    /// excplicitly invoked by UI. See <see cref="AutoSave"/>
    /// </summary>
    /// <param name="name">A file name. Should not contain invalid characters</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<OperationResult> Save(string name)
    {
        if (IsOperationInProgress)
        {
            throw new Exception(_operationInProgressMessage);
        }

        IsOperationInProgress = true;
        _incomingSaveData.Clear();

        _onWillSave?.Invoke();
        // All listeners of OnWillSave will now transmit their data

        string path = Path.Combine(SavePath, name);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(_incomingSaveData);

        if (!TryCreateDirectory(SavePath))
        {
            return FinishSave(ResultType.Fail,
                "Failed to create save directory. See log for details");
        }

        if (!TryCreateBackupSave(path, name, out var onFail))
        {
            return onFail;
        }

        try
        {
            await File.WriteAllBytesAsync(path, bytes);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return FinishSave(ResultType.Fail,
                "Failed to write save file. See log for details");
        }

        return FinishSave(ResultType.Success);
    }

    /// <summary>
    /// Receives save data from various parts of the game during a save 
    /// operation. Should only be called when <see cref="IsOperationInProgress"/> is true.
    /// </summary>
    /// 
    /// <param name="key">Unique key for this saved data</param>
    /// 
    /// <param name="value">
    /// Should be a primitive, such as 
    /// <see cref="long"/>, <see cref="int"/>, <see cref="float"/>, 
    /// <see cref="bool"/> or <see cref="string"/>
    /// </param>
    /// 
    /// <exception cref="Exception">
    /// Thrown if this method is called when 
    /// <see cref="IsOperationInProgress"/> is false
    /// </exception>
    public static void SendSaveData(string key, object value)
    {
        if (!IsOperationInProgress)
        {
            throw new Exception(
                "SendSaveData was called outside of a save operation. " +
                $"Check {nameof(IsOperationInProgress)} before calling this method");
        }

        if (_incomingSaveData.ContainsKey(key))
        {
            Debug.LogWarning($"Overwriting save data for key {key}");
        }

        _incomingSaveData[key] = value.ToString();
    }

    private static bool TryCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
        return true;
    }

    private static OperationResult FinishSave(ResultType resultType, string message = "")
    {
        IsOperationInProgress = false;
        _onSaveComplete?.Invoke();
        return new OperationResult(resultType, message);
    }

    private static bool TryCreateBackupSave(string savePath, string saveName, 
        out OperationResult onFail)
    {
        onFail = null;
        if (File.Exists(savePath))
        {
            if (!TryCreateDirectory(BackupSavesPath))
            {
                onFail = FinishSave(ResultType.Fail,
                    "Failed to create backup save directory. See log for details");
            }
            else
            {
                string backupPath = Path.Combine(
                    BackupSavesPath,
                    $"{saveName}_{DateTime.Now:yyyyMMddHHmmss}.bak");

                try
                {
                    File.Copy(savePath, backupPath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    onFail = FinishSave(ResultType.Fail,
                        "Failed to create backup save file. See log for details");
                }
            }
        }

        return onFail == null;
    }
}
