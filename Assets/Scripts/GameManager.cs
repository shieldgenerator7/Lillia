using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("How long to wait after a level is finished before going to next level")]
    public float nextLevelDelay = 3;
    [Tooltip("How long to pause the game after getting hit by a hazard")]
    public float hitResetDelay = 1;

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
    public Image imgPaused;

    private Timer gameTimer;
    private float lastHitTime = -1;

    private bool gameActive = true;//true: not paused

    private void Awake()
    {
        statisticsManager.init(fileManager.load() ?? new Statistics());
        timerUI.bestTime = statisticsManager.bestRun.duration;
        //
        levelManager.onLevelLoaded += onLevelLoaded;
        playerInput.onReset += onReset;
        playerInput.onPause += () =>
        {
            gameActive = !gameActive;
            Time.timeScale = (gameActive) ? 1 : 0;
            imgPaused.gameObject.SetActive(!gameActive);
        };
        checkpointManager.OnEndCheckpointReached += onEndCheckpointReached;
        resetTrigger.OnPlayerEntered += () =>
        {
            onReset();
        };
        nextLevelTrigger.OnPlayerEntered += () =>
        {
            ResetRun();
            levelManager.nextLevel();
        };
        //
        gameTimer = timerManager.startTimer();
        gameTimer.onTimerTicked += (duration) =>
        {
            statisticsManager.updateRun(duration);
            timerUI.updateTime(duration);
        };
        gameTimer.stop();
        statisticsManager.onBestTimeChanged += timerUI.updateBestTime;
        //
        Application.quitting += () =>
        {
            fileManager.save(statisticsManager.stats);
        };
    }

    #region Delegate Handlers

    void onLevelLoaded(LevelInfo levelInfo)
    {
        checkpointManager.registerCheckpointDelegates();
        FindObjectsByType<Hazard>(FindObjectsSortMode.None).ToList()
            .ForEach(hazard => hazard.onPlayerHit += onHazardHit);
        if (checkpointManager.End)
        {
            playerInput.onInputStateChanged -= onReset_Input;
            playerInput.onInputStateChanged += onReset_Input;
            statisticsManager.startRun(levelManager.LevelId);
            timerUI.bestTime = statisticsManager.bestRun.duration;
        }
        playerController.transform.position = levelInfo.startPos;
        FindObjectsByType<Resettable>(FindObjectsSortMode.None).ToList()
            .ForEach(rst => rst.recordInitialState());
    }

    void onReset()
    {
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
        //
        FindObjectsByType<Resettable>(FindObjectsSortMode.None).ToList()
            .FindAll(rst => rst.reactsToPlayerStart)
            .ForEach(rst => rst.levelStart());
    }

    void onHazardHit(Hazard hazard)
    {
        Time.timeScale = 0;
        lastHitTime = Time.unscaledTime;
    }
    private void Update()
    {
        if (lastHitTime > 0)
        {
            if (Time.unscaledTime >= lastHitTime + hitResetDelay)
            {
                onHazardHitDelayEnd();
            }
        }
    }
    void onHazardHitDelayEnd()
    {
        Time.timeScale = 1;
        lastHitTime = -1;
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

    #endregion

    #region Game Commands

    public void StartRun()
    {
        gameTimer.reset(Time.time);
        gameTimer.start();
        statisticsManager.startRun(levelManager.LevelId);
    }

    public void ResetRun()
    {
        //Reset fruits & other mechanics
        FindObjectsByType<Resettable>(FindObjectsSortMode.None).ToList()
            .ForEach(rst => rst.reset());
        //Reset time
        gameTimer.reset(Time.time);
        gameTimer.stop();
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
