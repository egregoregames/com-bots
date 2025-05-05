using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescriptionPanel : MonoBehaviour
{
    public TextMeshProUGUI menuPanelName;
    public TextMeshProUGUI menuPanelDescription;
    public Image menuPanelIcon;
    public Image background;
    public float backgroundColorAlpha = 0.2f;
    
    public void SetDescription(MenuPanel menuPanel, Color backgroundColor)
    {
        menuPanelName.text = menuPanel.menuName;
        menuPanelDescription.text = menuPanel.description;
        menuPanelIcon.sprite = menuPanel.icon.sprite;
        background.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, backgroundColorAlpha);
    }
    
}
