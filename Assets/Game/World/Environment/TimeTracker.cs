using System;
using Pinwheel.Jupiter;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    public bool syncWithRealTime = true;
    public TimeEvent timeEvent;
    public AnimationCurve ambientLightCurve;
    
    JDayNightCycle _jDayNightCycle;
    
    [Range(0f, 24f)]
    public float t;
    
    double prevHour = -1;

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
        
        DateTime now = DateTime.Now;
        

        if (prevHour < 20 && _jDayNightCycle.Time >= 20)
        {
            Debug.Log("Night begins");
            timeEvent.IsNightTime();
        }
        else if (prevHour < 6 && _jDayNightCycle.Time >= 6)
        {
            Debug.Log("Day begins");
            timeEvent.IsDayTime();
        }

        prevHour = _jDayNightCycle.Time;

        // if (_jDayNightCycle.Time >= 20 && _jDayNightCycle.Time < 6 && !timeEvent.isNight)
        // {
        //     Debug.Log("isNight");
        //     
        // }
        // if (_jDayNightCycle.Time >= 6 && _jDayNightCycle.Time < 20 && !timeEvent.isDay)
        // {
        //     Debug.Log("isDay");
        //     timeEvent.IsDayTime();
        // }
    }
}
