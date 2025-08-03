using System;
using System.Collections;
using Febucci.UI;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ComBots.src
{
    public class ChatBubble : MonoBehaviour, IDialogueUI
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

        public event EventHandler<SelectedResponseEventArgs> SelectedResponseHandler;
        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void ShowSubtitle(Subtitle subtitle)
        {
            throw new NotImplementedException();
        }

        public void HideSubtitle(Subtitle subtitle)
        {
            throw new NotImplementedException();
        }

        public void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
        {
            throw new NotImplementedException();
        }

        public void HideResponses()
        {
            throw new NotImplementedException();
        }

        public void ShowQTEIndicator(int index)
        {
            throw new NotImplementedException();
        }

        public void HideQTEIndicator(int index)
        {
            throw new NotImplementedException();
        }

        public void ShowAlert(string message, float duration)
        {
            throw new NotImplementedException();
        }

        public void HideAlert()
        {
            throw new NotImplementedException();
        }
    }
}
