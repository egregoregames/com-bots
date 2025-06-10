using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescriptionPanel : MonoBehaviour
{
    public List<Sprite> backgroundSprites;
    public TextMeshProUGUI menuPanelName;
    public TextMeshProUGUI menuPanelDescription;
    public Image background;
    
    
    public void SetDescription(MenuPanel menuPanel, int index)
    {
        menuPanelName.text = menuPanel.menuName;
        menuPanelDescription.text = menuPanel.description;
        background.sprite = backgroundSprites[index];
    }
    
}
