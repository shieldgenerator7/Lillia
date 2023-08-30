using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public Statistics stats = new Statistics();

    RunStats currentRun;
    public RunStats bestRun { get; private set; }

    public void init(Statistics stats)
    {
        this.stats = stats;
        _updateBestRun();
    }

    public void startRun(string levelId)
    {
        currentRun = new RunStats();
        currentRun.levelId = levelId;
        _updateBestRun();
    }

    public void finishRun()
    {
        Debug.Log($"Adding run: time: {currentRun.duration}");
        if (stats.runStats.Contains(currentRun))
        {
            Debug.LogError($"Trying to add run a 2nd time!: duration: {currentRun.duration}");
            return;
        }
        if (Mathf.Approximately(currentRun.duration, 0))
        {
            Debug.LogError($"Trying to add invalid run!: duration: {currentRun.duration}");
            return;
        }
        stats.runStats.Add(currentRun);
        _updateBestRun();
    }

    public void updateRun(float duration)
    {
        currentRun.duration = duration;
    }

    private void _updateBestRun()
    {
        List<RunStats> runs = stats.runStats.FindAll(run => run.levelId == currentRun.levelId).ToList();
        if (runs.Count > 0)
        {
            bestRun = runs.OrderBy(run => run.duration).First();
        }
        else
        {
            RunStats run = new RunStats();
            run.duration = 999.99f;
            bestRun = run;
        }
    }
}
