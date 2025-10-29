using ComBots.World.NPCs;
using UnityEngine;

public class ConnectionTab : MenuTab
{
    public NPC_Config connection;
    public MenuDescriptionPanel descriptionPanel;

    public override void SelectEffect()
    {
        base.SelectEffect();
        
        ShowConnectionDisplay();
    }

    void ShowConnectionDisplay()
    {
        
    }
}
