using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("How long to wait after a level is finished before going to next level")]
    public float nextLevelDelay = 3;

    public PlayerInput playerInput;
    public PlayerController playerController;
    public CheckpointManager checkpointManager;
    public LevelManager levelManager;
    public TimerManager timerManager;
    public StatisticsManager statisticsManager;

    public TimerUI timerUI;

    private Timer gameTimer;
    private Timer nextLevelTimer;

    private void Awake()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        playerInput.onReset += onReset;
        checkpointManager.OnEndCheckpointReached += onEndCheckpointReached;
        checkpointManager.OnCheckpointRecalling += onCheckpointRecalling;
        //
        gameTimer = timerManager.startTimer();
        gameTimer.onTimerTicked += (duration) =>
        {
            statisticsManager.updateRun(duration);
        };
        timerUI.init(gameTimer);
        nextLevelTimer = timerManager.startTimer();
        nextLevelTimer.stop();
        nextLevelTimer.onTimerTicked += (duration) =>
        {
            if (duration >= nextLevelDelay)
            {
                levelManager.nextLevel();
                nextLevelTimer.stop();
            }
        };
        //
        onSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    #region Delegate Handlers

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        checkpointManager.registerCheckpointDelegates();
        FindObjectsByType<Hazard>(FindObjectsSortMode.None).ToList()
            .ForEach(hazard => hazard.onPlayerHit += onHazardHit);
        if (checkpointManager.End)
        {
            StartRun();
        }
    }

    void onReset()
    {
        //Teleport player
        checkpointManager.teleportToStart();
        //Reset
        ResetRun();
        //Start
        StartRun();
    }

    void onHazardHit(Hazard hazard)
    {
        //Teleport player
        checkpointManager.teleportToCurrent();
        //Reset
        ResetRun();
        //Start
        StartRun();
    }

    void onEndCheckpointReached(Checkpoint cp)
    {
        FinishRun();
    }

    void onCheckpointRecalling(Checkpoint checkpoint)
    {
        playerController.resetState(checkpoint.transform.position);
    }

    #endregion

    #region Game Commands

    public void StartRun()
    {
        gameTimer.reset(Time.time);
        gameTimer.start();
        nextLevelTimer.stop();
        statisticsManager.startRun();
    }

    public void ResetRun()
    {
        //Reset fruits
        FindObjectsByType<Fruit>(FindObjectsSortMode.None).ToList()
            .ForEach(fruit => fruit.Available = true);
        //Reset time
        gameTimer.reset(Time.time);
        gameTimer.stop();
        nextLevelTimer.reset(Time.time);
        nextLevelTimer.stop();
        //Reset player
        playerController.resetState();
    }

    public void FinishRun()
    {
        gameTimer.stop();
        nextLevelTimer.reset(Time.time);
        nextLevelTimer.start();
        statisticsManager.finishRun();
        timerUI.bestTime = statisticsManager.bestRun.duration;
    }

    #endregion
}
