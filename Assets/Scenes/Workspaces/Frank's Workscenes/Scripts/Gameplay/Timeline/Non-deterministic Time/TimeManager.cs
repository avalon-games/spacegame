using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float time;

    public TimeManager()
    {
        TimeDependants = new List<ITimeChanging>();
    }

    public float Time
    {
        get
        {
            return time;
        }
        set
        {
            if (value != time)
            {
                float delta = value - time;
                foreach (ITimeChanging timeDependant in TimeDependants)
                {
                    timeDependant.AddTime(delta);
                }
            }
        }
    }

    public void SetTimeManual(float time)
    {
        this.time = time;
    }

    public IEnumerable<ITimeChanging> TimeDependants { get; set; }
}
