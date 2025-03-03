using UnityEngine;

public class ConnectionTab : MenuTab
{
    public NpcSo connection;
    public MenuDescriptionPanel descriptionPanel;
    protected override void SelectEffect()
    {
        base.SelectEffect();
        
        ShowConnectionDisplay();
    }

    void ShowConnectionDisplay()
    {
        
    }
}
