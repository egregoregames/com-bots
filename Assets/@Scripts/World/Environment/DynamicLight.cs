using UnityEngine;

public class DynamicLight : MonoBehaviour
{
    Light _light;
    public TimeEvent timeEvent;
    void Start()
    {
        _light = GetComponent<Light>();
        
        timeEvent.dayTime += () => { _light.enabled = false; };
        timeEvent.nightTime += () => { _light.enabled = true; };

    }

    
}
