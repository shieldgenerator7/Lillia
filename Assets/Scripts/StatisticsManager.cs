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
        bestRun = stats.OrderBy(run => run.duration).First();
    }

    public void startRun()
    {
        currentRun = new RunStats();
    }

    public void finishRun()
    {
        stats.Add(currentRun);
        bestRun = stats.OrderBy(run => run.duration).First();
    }

    public void updateRun(float duration)
    {
        currentRun.duration = duration;
    }
}
