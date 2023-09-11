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
    private LevelInfo levelInfo;

    public void update(StatisticsManager stats, LevelInfo levelInfo)
    {
        this.currentRun = stats.CurrentRun;
        this.fastestRun = stats.fastRun;
        this.bestRun = stats.bestRun;
        this.levelInfo = levelInfo;
        _update();
    }

    private void _update()
    {
        text.text =
            $"{currentRun} / {levelInfo?.collectibleCount}\n\n" +
            $"{fastestRun} / {levelInfo?.collectibleCount}\n" +
            $"{bestRun} / {levelInfo?.collectibleCount}\n" +
            "";
    }
}
