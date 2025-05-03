using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimplePortal : Portal
{
    public Portal nextPortal;
    public UISo uiSo;
    public AudioClip clip;
    public string areaName = "AREA NAME NOT SET";
    protected override void OnPlayerEnter(GameObject playerThatEntered)
    {
        OnTriggerPortal(0);
    }
    void ReleasePlayerMovement()
    {
        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<ThirdPersonController>().enabled = true;
    }
    private void OnTriggerPortal(int roomIndex)
    {
        // player.GetComponent<Animator>().enabled = false;
        // player.GetComponent<ThirdPersonController>().enabled = false;
        uiSo.TriggerAreaChangeTransition?.Invoke(TeleportPlayer, ReleasePlayerMovement, areaName);
    }
    private void TeleportPlayer()
    {
        
        nextPortal.player = player;
        
        nextPortal.SpawnPlayerAtPortal();
        
        uiSo.SoundSelected?.Invoke(clip);
    }
}