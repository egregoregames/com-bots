using System;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Portal : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    [SerializeField] private Transform portalExit;
    protected virtual void OnPlayerEnter(GameObject player){}
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            var player = other.transform.parent.gameObject;
            this.player = player;
            OnPlayerEnter(player);
        }
            
    }
    public void SpawnPlayerAtPortal()
    {
        
        //player.transform.position =  transform.position + (this.player.transform.forward * 1.5f);
        player.transform.position = portalExit.position;
        player.transform.rotation = portalExit.transform.rotation;
    }
}
