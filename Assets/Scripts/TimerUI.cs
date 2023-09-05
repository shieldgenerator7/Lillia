using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TMP_Text text;

    public float duration = 0.00f;
    public float bestTime = 999.99f;
    public int collectables = 0;
    public int collectableCount = 0;

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
    public void update()
    {
        text.text = $"{duration:N2}\n{bestTime:N2}\n\n{collectables} / {collectableCount}";
    }
}
