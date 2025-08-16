using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ComBots.UI.src.MapUI
{
    public class MapPanel : MenuPanel
    {
        [SerializeField] MapCursor mapCursor; 
        [SerializeField] List<RectTransform> locationRects;

        public override void OpenMenu()
        {
            base.OpenMenu();
            EventSystem.current.SetSelectedGameObject(mapCursor.gameObject);
        }

        void CheckLocationTriggers()
        {
            foreach (var location in locationRects)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(location, mapCursor.transform.position))
                {
                    Debug.Log("Entered location: " + location.name);
                    // Trigger highlight or info popup
                }
            }
        }

    }
}
