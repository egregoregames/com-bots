using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class SetContinueButtonLabel : MonoBehaviour
{
    [SerializeField] Sprite continueSprite;
    [SerializeField] Sprite endConversationSprite;
    
    void OnEnable()
    {
        if (!DialogueManager.isConversationActive) return;
        var image = GetComponentInChildren<Image>();
        var spriteTag = DialogueManager.currentConversationState.hasAnyResponses ? continueSprite : endConversationSprite; 
        image.sprite = spriteTag;
    }
}