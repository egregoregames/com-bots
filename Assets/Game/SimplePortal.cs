using System;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SimplePortal : Portal
{
    public Portal nextPortal;
    public UISo uiSo;
    public AudioClip clip;
    protected override void OnPlayerEnter(GameObject playerThatEntered)
    {
        uiSo.AreaSelected?.Invoke(HandlePlayerChangeArea, SceneManager.GetActiveScene().name);
    }
    private void HandlePlayerChangeArea()
    {
        
        nextPortal.player = player;
        
        nextPortal.SpawnPlayerAtPortal();
        
        uiSo.SoundSelected?.Invoke(clip);
    }
}