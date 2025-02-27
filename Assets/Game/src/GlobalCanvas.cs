using System;
using System.Collections;
using DependencyInjection;
using Game.src;
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

    private void Start()
    {
        // var img = sceneDescriptionBackground.GetComponent<Image>();
        //
        // var logoColor = img.color;
        //
        // logoColor.a = 0;
        // img.color = logoColor;
        //
        // var tColor = sceneDescriptionText.color;
        //
        // tColor.a = 0;
        // sceneDescriptionText.color = tColor;
        // StartCoroutine(SceneIntroCoroutine());
    }

    public void SceneIntro()
    {
    }

    IEnumerator SceneIntroCoroutine()
    {
        var img = sceneDescriptionBackground.GetComponent<Image>();
        yield return new WaitForEndOfFrame();
        while (img.color.a < 1)
        {
            var dColor = img.color;
            dColor.a += Time.deltaTime * fadeSpeed;
            img.color = dColor;
            
            var tColor = sceneDescriptionText.color;
            tColor.a += Time.deltaTime * fadeSpeed;
            sceneDescriptionText.color = tColor;
            yield return null;
        }
            
        yield return new WaitForEndOfFrame();
        while (img.color.a > 0)
        {
            var dColor = img.color;
            dColor.a -= Time.deltaTime * fadeSpeed;
            img.color = dColor;
            
            var tColor = sceneDescriptionText.color;
            tColor.a -= Time.deltaTime * fadeSpeed;
            sceneDescriptionText.color = tColor;
            
            yield return null;
        }
    }


    
    















}
