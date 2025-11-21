using UnityEngine;
using UnityEngine.UI;

namespace OccaSoftware.UIGradient.Runtime
{
  [ExecuteAlways]
  [RequireComponent(typeof(Image))]
  [RequireComponent(typeof(RectTransform))]
  public class UIGradient : MonoBehaviour
  {
    [Header("Gradient")]
    public Gradient gradient = new Gradient();
    public Direction direction = Direction.TopRight;
    public TextureWrapMode wrapMode = TextureWrapMode.Clamp;

    [Header("Transform")]
    public Vector2 translate = Vector2.zero;
    public float rotate;
    public Vector2 scale = Vector2.one;

    [Header("Rounding")]
    public float radius;

    private Texture2D gradientTexture;

    public enum Direction
    {
      Top = 0,
      TopRight = 315,
      Right = 270,
      BottomRight = 225,
      Bottom = 180,
      BottomLeft = 135,
      Left = 90,
      TopLeft = 45
    }

    private Material m;
    private RectTransform rectTransform;
    private Image img;

    private void Reset()
    {
      gradient = new Gradient();
      var colors = new GradientColorKey[2];
      colors[0] = new GradientColorKey(Color.red, 0.0f);
      colors[1] = new GradientColorKey(Color.blue, 1.0f);

      var alphas = new GradientAlphaKey[1];
      alphas[0] = new GradientAlphaKey(1.0f, 0.0f);

      gradient.SetKeys(colors, alphas);
    }

    private void OnEnable()
    {
      Shader s = Shader.Find("OccaSoftware/UI/Gradient");
      m = new Material(s);
      m.name = m.name + "_" + Random.Range(0, 1000);

      img = GetComponent<Image>();
      img.material = m;

      rectTransform = GetComponent<RectTransform>();
      Recreate();
    }

    private void OnValidate()
    {
      Recreate();
    }

    public void Recreate()
    {
      gradientTexture = GenerateGradientTexture();
      if (m)
      {
        m.SetTexture("_Gradient", gradientTexture);
        m.SetFloat("_Rotate", (float)direction + rotate);
        m.SetVector("_Translate", translate);
        m.SetVector("_Scale", scale);

        m.SetVector("_RectDimensions", rectTransform.sizeDelta);
        m.SetFloat("_Radius", radius);
      }
    }

    private Texture2D GenerateGradientTexture(int width = 256, int height = 1)
    {
      Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

      for (int i = 0; i < width; i++)
      {
        Color color = gradient.Evaluate(i / (float)(width - 1));
        texture.SetPixel(i, 0, color);
      }

      texture.Apply();
      texture.wrapMode = wrapMode;
      return texture;
    }

    private void OnDisable()
    {
      m = null;
    }
  }
}
