using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to track how much time has passed
/// </summary>
public class Timer
{
    float startTime;

    float currentTime;

    public float Duration => currentTime - startTime;

    private bool active = true;

    public Timer(float startTime)
    {
        this.startTime = startTime;
    }

    public void update(float time)
    {
        if (active)
        {
            currentTime = time;
            onTimerTicked?.Invoke(Duration);
        }
    }
    public delegate void TimerEvent(float time);
    public event TimerEvent onTimerTicked;

    public void start()
    {
        active = true;
    }

    public void stop()
    {
        active = false;
    }

    public void reset(float time)
    {
        this.startTime = time;
    }
}
