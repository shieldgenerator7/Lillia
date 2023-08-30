using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TMP_Text text;

    public float duration = 0.00f;
    public float bestTime = 999.99f;

    public void updateTime(float duration)
    {
        this.duration = duration;
        update();
    }
    public void updateBestTime(float bestTime)
    {
        this.bestTime = bestTime;
        update();
    }
    private void update()
    {
        text.text = $"{duration:N2}\n{bestTime:N2}";
    }
}
