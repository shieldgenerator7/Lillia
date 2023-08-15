using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TMP_Text text;

    Timer timer;
    public void init(Timer timer)
    {
        this.timer = timer;
        timer.onTimerTicked += onTimerTicked;
    }

    void onTimerTicked(float duration)
    {
        text.text = $"{duration:N2}";
    }
}
