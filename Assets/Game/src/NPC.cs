using System;
using DependencyInjection;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        
        [SerializeField] NpcSo npcSo;
        
        public UISo uiSo;

        public GameObject player;
        [ContextMenu("Go")]
        public void GoToPlayer()
        {
            GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        }
        
        public void Interact()
        {
            uiSo.OnPushDialogue?.Invoke(npcSo.dialogue);
            
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
