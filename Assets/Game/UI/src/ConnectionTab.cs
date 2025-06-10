using UnityEngine;

public class ConnectionTab : MenuTab
{
    public NpcSo connection;
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
