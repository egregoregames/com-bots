using System;
using System.Collections;
using DependencyInjection;
using ComBots.src;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalCanvas : MonoBehaviour, IDependencyProvider
{
    public GlobalCanvas globalCanvas;
    public ChatBubble chatBubble;
    public float fadeSpeed;
    [SerializeField] GameObject sceneDescriptionBackground;
    [SerializeField] TextMeshProUGUI sceneDescriptionText;

    [Provide]
    public GlobalCanvas ProvideCanvas()
    {
        return globalCanvas;
    }
}