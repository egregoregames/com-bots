using UnityEngine;

namespace ComBots.Game
{
    public class GlobalConfig : MonoBehaviour
    {
        private static GlobalConfig _instance;
        public static GlobalConfig I
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GlobalConfig>();
                }
                return _instance;
            }
        }

        [Header("Input")]
        public InputSO InputSO;
        public UISo UISo;

        void OnDestroy()
        {
            _instance = null;
        }
    }
}