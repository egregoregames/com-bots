using System;
using UnityEngine;

[CreateAssetMenu(menuName = "TimeEvent")]
public class TimeEvent : ScriptableObject
{
    public bool isNight;
    public bool isDay;

    public Action nightTime;
    public Action dayTime;

    public void IsNightTime()
    {
        isNight = true;
        isDay = false;
        nightTime?.Invoke();
    }

    public void IsDayTime()
    {
        isNight = false;
        isDay = true;
        dayTime?.Invoke();
    }

}
