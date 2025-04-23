using System;
using DependencyInjection;
using Febucci.UI;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Wrappers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        [SerializeField] NpcSo npcSo;
        [SerializeField] private GameEventRelay gameEventRelay;
        public UISo uiSo;

        public GameObject player;
        [ContextMenu("Go")]
        public void GoToPlayer()
        {
            GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        }
        
        public void Interact(GameObject interactor)
        {
            DialogueManager.StartConversation("New Conversation 1", interactor.transform, transform);
            //uiSo.OnPushDialogue?.Invoke(npcSo.dialogue);
            //gameEventRelay.ConnectionMade?.Invoke(npcSo);
            Debug.Log("Hello! Im an NPC!");
        }

        public void OnHoverStay()
        {
            talkPrompt.SetActive(true);
        }

        public void OnHoverEnter()
        {
        }

        public void OnHoverExit()
        {
            talkPrompt.SetActive(false);
        }
    }
}
