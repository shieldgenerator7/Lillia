using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public List<RunStats> stats = new List<RunStats>();

    RunStats currentRun;
    public RunStats bestRun { get; private set; }

    public void init(List<RunStats> stats)
    {
        this.stats = stats;
        if (stats.Count > 0)
        {
            bestRun = stats.OrderBy(run => run.duration).First();
        }
        else
        {
            RunStats run = new RunStats();
            run.duration = 999.99f;
            bestRun = run;
        }
    }

    public void startRun()
    {
        currentRun = new RunStats();
    }

    public void finishRun()
    {
        Debug.Log($"Adding run: time: {currentRun.duration}");
        if (stats.Contains(currentRun))
        {
            Debug.LogError($"Trying to add run a 2nd time!: duration: {currentRun.duration}");
            return;
        }
        stats.Add(currentRun);
        bestRun = stats.OrderBy(run => run.duration).First();
    }

    public void updateRun(float duration)
    {
        currentRun.duration = duration;
    }
}
