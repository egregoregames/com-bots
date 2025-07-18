using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src
{
    public class QuestIconUpdater : MonoBehaviour
    {
        Image _image;
        readonly Queue<Sprite> _icons = new();
        
        void Awake()
        {
            _image = GetComponent<Image>();
        }

        void OnEnable()
        {
            if (_image != null && _icons.Count > 0)
            {
                var color = _image.color;
                color.a = 1;
                _image.color = color;
                _image.sprite = _icons.Dequeue();
            }
            else
            {
                _image.sprite = null;
            }
        }

        void OnDisable()
        {
            if (_image != null)
            {
                // Clear the sprite
                _image.sprite = null;

                // Set alpha to 0 (fully transparent)
                var color = _image.color;
                color.a = 0f;
                _image.color = color;
            }
        }

        /// <summary>
        /// Enqueues the icon so if there's an enqueue alert it works as expected.
        /// </summary>
        /// <param name="sprite"></param>
        public void EnqueueIcon(Sprite sprite)
        {
            _icons.Enqueue(sprite);
        }
    }
}