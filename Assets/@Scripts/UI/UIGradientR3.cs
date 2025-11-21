using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public partial class UIGradientR3 : MonoBehaviourR3
{
    private static Shader _shader;
    private static Dictionary<string, Texture2D> _textureCache = new();

    [Header("Gradient")]
    [field: SerializeField]
    private Gradient Gradient { get; set; } = new();

    [field: SerializeField]
    private Direction GradientDirection { get; set; } = Direction.TopRight;

    [field: SerializeField]
    private TextureWrapMode WrapMode { get; set; } = TextureWrapMode.Clamp;

    [field: SerializeField]
    private int TextureResolution { get; set; } = 256;

    [Header("Transform")]
    public Vector2 translate = Vector2.zero;
    public float rotate;
    public Vector2 scale = Vector2.one;

    [Header("Rounding")]
    public float radius;

    [field: SerializeField, ReadOnly]
    private Material Material { get; set; }

    [field: SerializeField, ReadOnly]
    private RectTransform RectTransform { get; set; }

    [field: SerializeField, ReadOnly]
    private Image Image { get; set; }

    protected override void Initialize()
    {
        base.Initialize();
        Generate();
    }

    private void OnValidate()
    {
        Generate();
    }

    public void Generate()
    {
        if (Material == null)
        {
            Material = new Material(GetShader());
            Material.name = Material.name + "_" + Random.Range(0, 1000);
        }

        if (Image == null)
        {
            Image = GetComponent<Image>();
        }

        if (RectTransform == null)
        {
            RectTransform = GetComponent<RectTransform>();
        }

        if (Image.material != Material)
        {
            Image.material = Material;
        }
            
        Material.SetTexture("_Gradient", GetOrGenerateTexture(TextureResolution, 1));
        Material.SetFloat("_Rotate", (float)GradientDirection + rotate);
        Material.SetVector("_Translate", translate);
        Material.SetVector("_Scale", scale);
        Material.SetVector("_RectDimensions", RectTransform.sizeDelta);
        Material.SetFloat("_Radius", radius);
    }

    private static Shader GetShader()
    {
        if (_shader == null)
        {
            _shader = Shader.Find("OccaSoftware/UI/Gradient");
        }
        
        return _shader;
    }

    public void SetGradient(Gradient gradient)
    {
        Gradient = gradient;
        Generate();
    }

    private Texture2D GetOrGenerateTexture(int width = 256, int height = 1)
    {
        string colorKey = "";
        foreach (var alphaKey in Gradient.alphaKeys)
        {
            colorKey += alphaKey.alpha.ToString() + alphaKey.time.ToString();
        }

        foreach (var colorKeyItem in Gradient.colorKeys)
        {
            colorKey += colorKeyItem.color.GetHashCode().ToString() + colorKeyItem.time.ToString();
        }

        string key = colorKey + WrapMode + width;
        if (_textureCache.TryGetValue(key, out Texture2D cachedTexture))
        {
            Log("Pulling from cache: " + key, LogLevel.Verbose);
            return cachedTexture;
        }
        else
        {             
            Texture2D newTexture = GenerateGradientTexture(width, height);
            _textureCache[key] = newTexture;
            return newTexture;
        }
    }

    private Texture2D GenerateGradientTexture(int width = 256, int height = 1)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int i = 0; i < width; i++)
        {
            Color color = Gradient.Evaluate(i / (float)(width - 1));
            texture.SetPixel(i, 0, color);
        }

        texture.Apply();
        texture.wrapMode = WrapMode;
        return texture;
    }
}