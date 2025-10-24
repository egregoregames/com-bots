using System;
using System.Threading.Tasks;
using ComBots.Logs;
using UnityEngine;
using UnityEngine.Events;

namespace ComBots.TimesOfDay
{
    public class TimesOfDay_Manager : MonoBehaviour
    {
        // ============ Singleton ============ //
        public static TimesOfDay_Manager Instance { get; private set; }
        [Tooltip("ScriptableObject Configuration for the times of day.")]
        [SerializeField] private TimesOfDay_Config _timesOfDayConfig;

        // ============ Event Updates ============ //
        [Header("Event Updates")]
        [SerializeField] private float _timeUpdateInterval = 60f;
        private readonly UnityEvent<TimeOfDay> _onTimeOfDayChanged = new();
        private float _updateTimer;

        // ============ Cache ============ //
        private TimeOfDay _cachedTimeOfDay;
        private float _nextStartHour;

        #region Unity Methods
        // ----------------------------------------
        // Unity Methods 
        // ----------------------------------------

        void Awake()
        {
            // Make sure TimeOfDayConfigs are sorted by StartHour
            Array.Sort(_timesOfDayConfig.TimeOfDayConfigs, (a, b) => a.StartHour.CompareTo(b.StartHour));
            _nextStartHour = _timesOfDayConfig.TimeOfDayConfigs[0].StartHour;
            UpdateTimeOfDay();
            Instance = this;
        }

        void Update()
        {
            _updateTimer += Time.deltaTime;
            if (_updateTimer >= _timeUpdateInterval)
            {
                _updateTimer = 0f;

                UpdateTimeOfDay();
            }
        }

        #endregion

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        public static async Task<TimesOfDay_Manager> Async_GetInstance()
        {
            while (Instance == null)
            {
                await Task.Yield();
            }
            return Instance;
        }

        /// <summary>
        /// Subscribe to time of day changes and immediately invoke the callback with the current time of day.
        /// </summary>
        /// <param name="callback"></param>
        public static async void Async_SubscribeToTimeOfDayChange(UnityAction<TimeOfDay> callback)
        {
            await Async_GetInstance();
            Instance._onTimeOfDayChanged.AddListener(callback);
            callback.Invoke(Instance.GetTimeOfDay());
        }

        /// <summary>
        /// Unsubscribe from time of day changes.
        /// </summary>
        /// <param name="callback"></param>
        public static async void Async_UnsubscribeFromTimeOfDayChange(UnityAction<TimeOfDay> callback)
        {
            await Async_GetInstance();
            Instance._onTimeOfDayChanged.RemoveListener(callback);
        }

        /// <summary>
        /// Get the current cached time of day.
        /// </summary>
        /// <returns></returns>
        public TimeOfDay GetTimeOfDay()
        {
            return _cachedTimeOfDay;
        }

        #endregion

        #region Private Methods
        // ----------------------------------------
        // Private Methods
        // ----------------------------------------

        private void UpdateTimeOfDay()
        {
            float currentHour = DateTime.Now.Hour + DateTime.Now.Minute / 60f;

            TimeOfDay newTimeOfDay = _cachedTimeOfDay;
            float nextStart = _nextStartHour;

            // Find the current time period by checking which period we're in
            for (int i = _timesOfDayConfig.TimeOfDayConfigs.Length - 1; i >= 0; i--)
            {
                float startHour = _timesOfDayConfig.TimeOfDayConfigs[i].StartHour;

                // Check if current time is at or past this period's start
                if (currentHour >= startHour)
                {
                    newTimeOfDay = _timesOfDayConfig.TimeOfDayConfigs[i].TimeOfDay;

                    // Calculate next period start (wrap around if needed)
                    int nextIndex = (i + 1) % _timesOfDayConfig.TimeOfDayConfigs.Length;
                    nextStart = _timesOfDayConfig.TimeOfDayConfigs[nextIndex].StartHour;

                    // If next period wraps to next day, add 24 hours for comparison
                    if (nextStart <= startHour)
                    {
                        nextStart += 24f;
                    }

                    break;
                }
            }

            // Check if time of day changed
            if (newTimeOfDay != _cachedTimeOfDay)
            {
                _cachedTimeOfDay = newTimeOfDay;
                _nextStartHour = nextStart;
                _onTimeOfDayChanged.Invoke(_cachedTimeOfDay);
                MyLogger<TimesOfDay_Manager>.StaticLog($"Time of Day changed to {_cachedTimeOfDay}");
            }
            else
            {
                _nextStartHour = nextStart;
            }
        }

        #endregion
    }
}