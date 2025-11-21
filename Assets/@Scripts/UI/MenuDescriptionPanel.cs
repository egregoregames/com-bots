using ComBots.Sandbox.Global.UI.Menu;
using OccaSoftware.UIGradient.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescriptionPanel : MonoBehaviourR3
{
    public TextMeshProUGUI menuPanelName;
    public TextMeshProUGUI menuPanelDescription;

    [field: SerializeField]
    private Image ImageIcon { get; set; }

    [field: SerializeField]
    private UIGradient UIGradient { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            PauseMenu_AppButton.OnSelected(SetDescription),
            PauseMenu.OnButtonsVisible(() => gameObject.SetActive(true)),
            PauseMenu.OnButtonsMinimized(() => gameObject.SetActive(false)));

        gameObject.SetActive(false);
    }
    
    private void SetDescription(PauseMenu_AppButton info)
    {
        
        menuPanelName.text = info.DescriptionBoxTitle;
        menuPanelDescription.text = info.DescriptionBoxText;
        ImageIcon.sprite = info.Icon;
        UIGradient.gradient = info.DescriptionBoxBackgroundGradient;
        UIGradient.Recreate();
    }
}
