using ComBots.Sandbox.Global.UI.Menu;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescriptionPanel : MonoBehaviourR3
{
    public TextMeshProUGUI menuPanelName;
    public TextMeshProUGUI menuPanelDescription;
    public Image background;

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
        background.sprite = info.DescriptionBoxBackgroundSprite;
    }
}
