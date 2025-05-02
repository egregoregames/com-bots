using System;
using Pinwheel.Jupiter;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    public bool syncWithRealTime = true;

    public AnimationCurve ambientLightCurve;
    
    JDayNightCycle _jDayNightCycle;
    
    [Range(0f, 24f)]
    public float t;
    void Start()
    {
        _jDayNightCycle = GetComponent<JDayNightCycle>();
        Debug.Log($"Detected users time is: {DateTime.Now} setting time to hour: {DateTime.Now.Hour}");
    }

    // Update is called once per frame
    void Update()
    {
        if (syncWithRealTime)
        {
            float decimalHour = DateTime.Now.Hour + DateTime.Now.Minute / 60.0f + DateTime.Now.Second / 3600.0f;
            _jDayNightCycle.Time = decimalHour;
        }
        
        var intensity = ambientLightCurve.Evaluate(_jDayNightCycle.Time);
        
        //float hourAsPercentageOfADay = DateTime.Now.Hour * .0416f;
        
        //var computed = Mathf.Lerp(midnightIntensity, .8f, hourAsPercentageOfADay);
        
        RenderSettings.ambientIntensity = intensity;
        RenderSettings.reflectionIntensity = intensity;
    }
}
