using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public List<Timer> timers = new List<Timer>();


    // Update is called once per frame
    void Update()
    {
        if (timers.Count > 0)
        {
            float time = Time.time;
            timers.ForEach(timer => timer.update(time));
        }
    }

    public Timer startTimer()
    {
        Timer timer = new Timer(Time.time);
        timers.Add(timer);
        return timer;
    }

    public void stopTimer(Timer timer)
    {
        timers.Remove(timer);
    }
}
