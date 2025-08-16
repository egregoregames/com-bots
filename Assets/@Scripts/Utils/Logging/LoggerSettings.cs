using UnityEngine;

namespace ComBots.Logs
{
    /// <summary>
    /// ScriptableObject to hold global logger settings, like default colors.
    /// </summary>
    [CreateAssetMenu(fileName = "LoggerSettings", menuName = "ComBots/Utilities/Logger Settings", order = 1)]
    public class LoggerSettings : ScriptableObject
    {
        [Header("Default Colors (Name or Hex)")]
        [Tooltip("Default color for Warning messages. Used if the logger instance doesn't have a specific color set.")]
        public string WarningColor = "yellow";

        [Tooltip("Default color for Error messages. Used if the logger instance doesn't have a specific color set.")]
        public string ErrorColor = "red";

        // Optional: Add default LogLevel, etc.

        public static LoggerSettings I { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            I = Resources.Load<LoggerSettings>("LoggerSettings");

            if (I == null)
            {
                Debug.LogError("LoggerSettings asset not found in Resources folder. Please create one via Assets > Create > NeoRaptors > Utilities > Logger Settings and place it in any Resources folder.");
            }
        }
    }
}
