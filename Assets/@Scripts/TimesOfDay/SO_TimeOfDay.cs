using UnityEngine;

namespace ComBots.TimesOfDay
{
    [CreateAssetMenu(fileName = "TimesOfDayConfig", menuName = "ComBots/TimeOfDay/TimesOfDayConfig")]
    public class TimesOfDay_Config : ScriptableObject
    {
        public TimeOfDay_Config[] TimeOfDayConfigs;
    }

    [System.Serializable]
    public struct TimeOfDay_Config
    {
        public TimeOfDay TimeOfDay;
        public float StartHour;
    }
}