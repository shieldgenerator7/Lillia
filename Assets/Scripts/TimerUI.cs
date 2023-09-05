using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TMP_Text text;

    public RunStats currentRun;
    public RunStats fastestRun;
    public RunStats bestRun;
    public int collectableCount = 0;

    public void update()
    {
        if (fastestRun.duration == 0)
        {
            fastestRun.fruitCount = 0;
            fastestRun.duration = 999.99f;
        }
        if (bestRun.duration == 0)
        {
            bestRun.fruitCount = collectableCount; 
            bestRun.duration = 999.99f;
        }
        text.text =
            $"{currentRun} / {collectableCount}\n\n" +
            $"{fastestRun} / {collectableCount}\n" +
            $"{bestRun} / {collectableCount}\n" +
            "";
    }
}
