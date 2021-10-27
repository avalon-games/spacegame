using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private TimeBody[] timeBodies;

    private void Start()
    {
        timeBodies = FindObjectsOfType<TimeBody>();
    }

    public void OnTimeLoopClicked()
    {
        foreach (TimeBody timeBody in timeBodies)
        {
            timeBody.StartRecording();
        }
    }

    public void OnTimeLoopStartClicked()
    {
        foreach (TimeBody timeBody in timeBodies)
        {
            timeBody.StopRecording();
            timeBody.StartTimeLoop();
        }
    }
}
