using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src
{
    public class AutoCleanerIcon : MonoBehaviour
    {
        private Image image;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        void OnDisable()
        {
            if (image != null)
            {
                // Clear the sprite
                image.sprite = null;

                // Set alpha to 0 (fully transparent)
                var color = image.color;
                color.a = 0f;
                image.color = color;
            }
        }
    }
}