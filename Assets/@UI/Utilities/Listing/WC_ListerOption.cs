using UnityEngine;

public class WC_ListerOption : MonoBehaviour
{
    public bool IsHighlighted { get; private set; }
    
    public virtual void SetIsHighlighted(bool highlighted)
    {
        IsHighlighted = highlighted;
    }
}