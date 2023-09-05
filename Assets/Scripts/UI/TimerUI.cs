using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TMP_Text text;

    private RunStats currentRun;
    private RunStats fastestRun;
    private RunStats bestRun;
    private int collectableCount = 0;

    public void update(StatisticsManager stats)
    {
        this.currentRun = stats.CurrentRun;
        this.fastestRun = stats.fastRun;
        this.bestRun = stats.bestRun;
        this.collectableCount = stats.collectableCount;
        _update();
    }

    private void _update()
    {
        text.text =
            $"{currentRun} / {collectableCount}\n\n" +
            $"{fastestRun} / {collectableCount}\n" +
            $"{bestRun} / {collectableCount}\n" +
            "";
    }
}
