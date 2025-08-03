using UnityEngine;

namespace ComBots.Global.UI.Menu
{
    [CreateAssetMenu(fileName = "NavigationItemConfig", menuName = "ComBots/Global/UI/Menu/NavigationItemConfig")]
    public class NavigationItemConfig : ScriptableObject
    {
        public Texture2D background;
        public string description;
        public string header;
    }
}