using System;
using UnityEngine.UI;

public class MenuTab : Button
{
    public Action onSelect;
    public bool isSelected;

    public virtual void SelectEffect()
    {
        
    }

    public virtual void DeselectEffect()
    {
        
    }
    
    public virtual void HandleHorizontalInput(int direction)
    {
        
    }

    public virtual void HandleVerticalInput(int direction)
    {
        
    }
}
