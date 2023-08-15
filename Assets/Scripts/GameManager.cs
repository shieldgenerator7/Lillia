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

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        gameTimer.reset(Time.time);
        gameTimer.start();
        nextLevelTimer.stop();
        checkpointManager.registerCheckpointDelegates();
        FindObjectsByType<Hazard>(FindObjectsSortMode.None).ToList()
            .ForEach(hazard => hazard.onPlayerHit += onHazardHit);
    }

    void onReset()
    {
        onHazardHit(null);
        nextLevelTimer.stop();
    }

    void onHazardHit(Hazard hazard)
    {
        //Teleport player
        checkpointManager.teleportToCurrent();
        //Reset fruits
        FindObjectsByType<Fruit>(FindObjectsSortMode.None).ToList()
            .ForEach(fruit => fruit.Available = true);
        //Reset time
        gameTimer.reset(Time.time);
        gameTimer.start();
    }

    void onEndCheckpointReached(Checkpoint cp)
    {
        gameTimer.stop();
        nextLevelTimer.reset(Time.time);
        nextLevelTimer.start();
    }

    void onCheckpointRecalling(Checkpoint checkpoint)
    {
        playerController.resetState(checkpoint.transform.position);
    }
}
