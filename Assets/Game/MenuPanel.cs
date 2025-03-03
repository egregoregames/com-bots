using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public string description;

    [SerializeField] private MenuDescriptionPanel descriptionPanel;
    [SerializeField] private Button menuButton;
    public List<Button> buttons;
    private void Awake()
    {
        
    }

    void OnMenuSelected()
    {
        
    }
}
