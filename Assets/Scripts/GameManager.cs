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
    public FileManager fileManager;

    public TimerUI timerUI;
    public PlayerTrigger resetTrigger;
    public PlayerTrigger nextLevelTrigger;

    private Timer gameTimer;

    private void Awake()
    {
        statisticsManager.init(fileManager.load() ?? new Statistics());
        timerUI.bestTime = statisticsManager.bestRun.duration;
        //
        SceneManager.sceneLoaded += onSceneLoaded;
        playerInput.onReset += onReset;
        checkpointManager.OnEndCheckpointReached += onEndCheckpointReached;
        checkpointManager.OnCheckpointRecalling += onCheckpointRecalling;
        resetTrigger.OnPlayerEntered += () =>
        {
            onReset();
        };
        nextLevelTrigger.OnPlayerEntered += () =>
        {
            levelManager.nextLevel();
        };
        //
        gameTimer = timerManager.startTimer();
        gameTimer.onTimerTicked += (duration) =>
        {
            statisticsManager.updateRun(duration);
        };
        timerUI.init(gameTimer);
        gameTimer.stop();
        //
        Application.quitting += () =>
        {
            fileManager.save(statisticsManager.stats);
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
            playerInput.onInputStateChanged -= onReset_Input;
            playerInput.onInputStateChanged += onReset_Input;
        }
    }

    void onReset()
    {
        //Teleport player
        checkpointManager.teleportToStart();
        //Reset
        ResetRun();
        //
        playerInput.onInputStateChanged -= onReset_Input;
        playerInput.onInputStateChanged += onReset_Input;
    }
    void onReset_Input(InputState inputState)
    {
        //Start
        StartRun();
        //
        playerInput.onInputStateChanged -= onReset_Input;
    }

    void onHazardHit(Hazard hazard)
    {
        //Teleport player
        checkpointManager.teleportToCurrent();
        //Reset
        ResetRun();
        //
        playerInput.onInputStateChanged -= onReset_Input;
        playerInput.onInputStateChanged += onReset_Input;
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
        //Reset player
        playerController.resetState();
    }

    public void FinishRun()
    {
        gameTimer.stop();
        statisticsManager.finishRun();
        timerUI.bestTime = statisticsManager.bestRun.duration;
        fileManager.save(statisticsManager.stats);
    }

    #endregion
}
