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
    public Image imgPaused;

    private List<LevelContents> levelContents;
    private LevelInfo levelInfo;

    private Timer gameTimer;
    private float lastHitTime = -1;

    private bool gameActive = true;//true: not paused

    private void Awake()
    {
        statisticsManager.init(fileManager.load() ?? new Statistics());
        timerUI.update(statisticsManager, levelInfo);
        //
        levelManager.onLevelLoaded += onLevelLoaded;
        playerInput.onPause += () =>
        {
            gameActive = !gameActive;
            Time.timeScale = (gameActive) ? 1 : 0;
            imgPaused.gameObject.SetActive(!gameActive);
        };
        playerInput.onReset += onReset;
        playerInput.onPrevLevel += () => switchLevel(-1);
        playerInput.onNextLevel += () => switchLevel(1);
        playerController.onCollectableCollected += () =>
        {
            statisticsManager.recordCollectable();
            timerUI.update(statisticsManager, levelInfo);
        };
        checkpointManager.OnEndCheckpointReached += onEndCheckpointReached;
        //
        gameTimer = timerManager.startTimer();
        gameTimer.onTimerTicked += (duration) =>
        {
            statisticsManager.updateRun(duration);
            timerUI.update(statisticsManager, levelInfo);
        };
        gameTimer.stop();
        statisticsManager.onBestRunChanged += (time) =>
        {
            timerUI.update(statisticsManager, levelInfo);
        };
        //
        Application.quitting += () =>
        {
            fileManager.save(statisticsManager.stats);
        };
    }

    #region Delegate Handlers

    void onLevelLoaded(LevelInfo levelInfo)
    {
        this.levelInfo = levelInfo;
        checkpointManager.registerCheckpointDelegates();
        levelContents = FindObjectsByType<LevelContents>(FindObjectsSortMode.None).ToList();
        levelContents.ForEach(lc => lc.hazards
            .ForEach(hazard => hazard.onPlayerHit += onHazardHit)
            );
        statisticsManager.startRun(levelInfo);
        timerUI.update(statisticsManager, levelInfo);
        playerController.transform.position = levelInfo.startPos;
        levelContents.ForEach(lc => lc.resettables
            .ForEach(rst => rst.recordInitialState())
            );
        if (checkpointManager.End)
        {
            playerInput.onInputStateChanged -= onReset_Input;
            playerInput.onInputStateChanged += onReset_Input;
        }
        FindAnyObjectByType<PlayerTrigger>()
            .OnPlayerEntered += () => switchLevel(1);
    }

    void switchLevel(int dir)
    {
        ResetRun();
        levelManager.switchLevel(dir);
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
        levelContents.ForEach(lc => lc.resettables
            .FindAll(rst => rst.reactsToPlayerStart)
            .ForEach(rst => rst.levelStart())
            );
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
        statisticsManager.startRun(levelInfo);
        timerUI.update(statisticsManager, levelInfo);
    }

    public void ResetRun()
    {
        //Reset fruits & other mechanics
        levelContents.ForEach(lc => lc.resettables
            .ForEach(rst => rst.reset())
            );
        //Reset time
        gameTimer.reset(Time.time);
        gameTimer.stop();
        //
        timerUI.update(statisticsManager, levelInfo);
    }

    public void FinishRun()
    {
        gameTimer.stop();
        statisticsManager.finishRun();
        timerUI.update(statisticsManager, levelInfo);
        fileManager.save(statisticsManager.stats);
    }

    #endregion
}
