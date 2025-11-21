using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Logic for the description box that appears above the 
/// OmniApp pause menu buttons.
/// </summary>
public class MenuDescriptionPanel : MonoBehaviourR3
{
    public TextMeshProUGUI menuPanelName;
    public TextMeshProUGUI menuPanelDescription;

    [field: SerializeField]
    private Image ImageIcon { get; set; }

    [field: SerializeField]
    private UIGradientR3 UIGradient { get; set; }

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            PauseMenu_AppButton.OnSelected(SetDescription),
            PauseMenu.OnButtonsVisible(() => gameObject.SetActive(true)),
            PauseMenu.OnButtonsMinimized(() => gameObject.SetActive(false)));
    }
    
    private void SetDescription(PauseMenu_AppButton info)
    {
        menuPanelName.text = info.DescriptionBoxTitle;
        menuPanelDescription.text = info.DescriptionBoxText;
        ImageIcon.sprite = info.Icon;
        UIGradient.SetGradient(info.DescriptionBoxBackgroundGradient);
    }
}
