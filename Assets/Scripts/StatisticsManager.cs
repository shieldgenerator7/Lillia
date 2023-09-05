using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public Statistics stats = new Statistics();

    private RunStats currentRun;
    public RunStats CurrentRun => currentRun;
    public RunStats fastRun { get; private set; }
    public RunStats bestRun { get; private set; }

    public int collectableCount;

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

    public delegate void OnDurationStatChanged(float duration);

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
            fastRun = runs.OrderBy(run => run.duration).First();
            bestRun = runs.FindAll(run => run.fruitCount == collectableCount)
                .OrderBy(run => run.duration).First();
        }
        else
        {
            //
            RunStats run = new RunStats();
            run.duration = 999.99f;
            run.fruitCount = 0;
            fastRun = run;
            //
            RunStats run2 = new RunStats();
            run2.duration = 999.99f;
            run2.fruitCount = collectableCount;
            bestRun = run2;
        }
        onBestRunChanged?.Invoke(bestRun.duration);
    }
    public event OnDurationStatChanged onBestRunChanged;

    public void recordCollectable()
    {
        currentRun.fruitCount++;
    }
}
