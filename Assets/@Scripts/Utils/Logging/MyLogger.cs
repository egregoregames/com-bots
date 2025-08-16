using System;
using ComBots.Utils.Texts;
using UnityEngine;

namespace ComBots.Logs
{
    /// <summary>
    /// Provides methods for logging messages with automatic, colored class name prefixes
    /// and level-based filtering. Embed this serializable class in your MonoBehaviour
    /// and call Initialize() in Awake().
    /// </summary>
    [System.Serializable]
    public class MyLogger<T>
    {
        public static LoggerSettings LoggerSettings => LoggerSettings.I;

        // --- Inspector Settings ---
        [Tooltip("Controls the verbosity of logs from this instance.")]
        public LogLevel level = LogLevel.INFO_ERRORS_WARNINGS;

        [Tooltip("The color used for standard Log() messages. Warnings/Errors use global settings.")]
        [SerializeField] private Color logColor = Color.white;

        // --- Runtime Data (Not serialized) ---
        private Type callingType;
        private string formattedPrefix;
        private bool isInitialized = false;

        /// <summary>
        /// Initializes the logger instance with its owner type.
        /// MUST be called from the owner MonoBehaviour's Awake() or Start().
        /// </summary>
        public void TryInit()
        {
            if (isInitialized) return;
            callingType = typeof(T);
            UpdateFormattedPrefix();
            isInitialized = true;
        }

        /// <summary>
        /// Sets the color used for standard Log messages and updates the prefix.
        /// Can be called at runtime to change the color.
        /// </summary>
        /// <param name="newColor">The new color for logs.</param>
        public void SetLogColor(Color newColor)
        {
            logColor = newColor;
            if (isInitialized)
            {
                UpdateFormattedPrefix();
            }
        }

        /// <summary>
        /// Logs a standard message if the current LogLevel allows it (VERBOSE).
        /// Uses the instance-specific log color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            if (!isInitialized) { Debug.LogError("Logger not initialized! Call Initialize() in Awake()."); return; }
            if (level > LogLevel.INFO_ERRORS_WARNINGS) return;
            Debug.Log($"{formattedPrefix} {message}");
        }

        /// <summary>
        /// Logs a warning message if the current LogLevel allows it (VERBOSE or WARNINGS_ERRORS).
        /// Uses the WarningColor defined in LoggerSettings.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogWarning(string message)
        {
            if (!isInitialized) { Debug.LogError("Logger not initialized! Call Initialize() in Awake()."); return; }
            if (level > LogLevel.WARNINGS_ERRORS) return;
            string warningPrefix = RichTextFormatter.BoldColor($"[{callingType.Name}]", LoggerSettings.WarningColor);
            Debug.Log($"{warningPrefix} {message}");
        }

        /// <summary>
        /// Logs an error message if the current LogLevel allows it (VERBOSE, WARNINGS_ERRORS, or ERRORS).
        /// Uses the ErrorColor defined in LoggerSettings.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogError(string message)
        {
            if (!isInitialized) { Debug.LogError("Logger not initialized! Call Initialize() in Awake()."); return; }
            if (level > LogLevel.ERRORS) return;
            string errorPrefix = RichTextFormatter.BoldColor($"[{callingType.Name}]", LoggerSettings.ErrorColor);
            Debug.LogError($"{errorPrefix} {message}");
        }

        /// <summary>
        /// Updates the cached formatted prefix string based on the current log color.
        /// </summary>
        private void UpdateFormattedPrefix()
        {
            // Convert Color to hex format for rich text
            string colorHex = ColorUtility.ToHtmlStringRGB(logColor);
            formattedPrefix = RichTextFormatter.BoldColor($"[{callingType.Name}]", $"#{colorHex}");
        }

        public static void StaticLog(string message)
        {
            string formattedPrefix = RichTextFormatter.Bold($"[{typeof(T).Name}]");
            Debug.Log($"{formattedPrefix} {message}");
        }

        public static void StaticLogError(string message)
        {
            string errorPrefix = RichTextFormatter.BoldColor($"[{typeof(T).Name}]", LoggerSettings.ErrorColor);
            Debug.LogError($"{errorPrefix} {message}");
        }

        public static void StaticLogWarning(string message)
        {
            string errorPrefix = RichTextFormatter.BoldColor($"[{typeof(T).Name}]", LoggerSettings.WarningColor);
            Debug.Log($"{errorPrefix} {message}");
        }
    }
}