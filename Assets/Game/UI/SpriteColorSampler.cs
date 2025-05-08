using UnityEngine;

namespace Game.UI
{
    public class SpriteColorSampler
    {
        public Color spriteColor;

        public SpriteColorSampler(Sprite sprite)
        {
            // Get the sprite's texture
            Texture2D texture = sprite.texture;

            // Convert the sprite's rect position to texture-space coordinates
            Rect rect = sprite.textureRect;
        
            // Convert top-left in sprite rect to texture pixel coordinates
            int x = Mathf.FloorToInt(rect.x);
            int y = Mathf.FloorToInt(rect.y + rect.height - 1); // Top-left (Y is inverted in Unity textures)
            
            // Read the color at that pixel
            spriteColor = texture.GetPixel(x, y);

            Debug.Log("Cached color: " + spriteColor);
        }
    }
}
