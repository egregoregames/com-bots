using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.src.BackpackUI
{
    public class BackpackPanel : MenuPanel
    {
        [SerializeField] List<BackpackItemTab> itemTabs;
        List<MenuTab> _itemMenuTabs = new List<MenuTab>();

        protected void Awake()
        {
            _itemMenuTabs.AddRange(itemTabs);
        }
    }
}
