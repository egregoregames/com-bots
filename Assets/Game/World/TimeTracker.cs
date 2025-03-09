using System;
using Pinwheel.Jupiter;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    void Start()
    {
        int hour = DateTime.Now.Hour;
        GetComponent<JDayNightCycle>().Time = hour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
