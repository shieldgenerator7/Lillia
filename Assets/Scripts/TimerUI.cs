using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TMP_Text text;

    public float bestTime = 999.99f;

    Timer timer;
    public void init(Timer timer)
    {
        this.timer = timer;
        timer.onTimerTicked += onTimerTicked;
    }

    void onTimerTicked(float duration)
    {
        text.text = $"{duration:N2}\n{bestTime:N2}";
    }
}
