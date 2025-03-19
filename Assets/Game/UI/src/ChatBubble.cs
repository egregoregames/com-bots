using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.src
{
    public class ChatBubble : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI chatText;
        [SerializeField] TypewriterByCharacter typewriter;
        [SerializeField] UISo uiSo;
        [FormerlySerializedAs("inputSO")] [SerializeField] InputSO inputSo;
        [SerializeField] GameObject chatBubbleGameObject;
        

        public void SendDialogue(string[] dialogue)
        {
            chatBubbleGameObject.SetActive(true);
            StartCoroutine(TypeCoro(dialogue));
        }

        IEnumerator TypeCoro(string[] dialogue)
        {
            inputSo.switchToUIInput?.Invoke();
        
            typewriter.SetTypewriterSpeed(0.01f);
        
            for (int i = 0; i < dialogue.Length; i++)
            {
                typewriter.ShowText(dialogue[i]);

                yield return new WaitUntil(() => inputSo.submit);

                yield return new WaitUntil(() => inputSo.submit);
            }
        
        
            inputSo.switchToPlayerInput?.Invoke();

            chatBubbleGameObject.SetActive(false);
        }
    }
}
