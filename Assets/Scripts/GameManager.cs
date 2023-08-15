using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerController playerController;
    public CheckpointManager checkpointManager;
    public LevelManager levelManager;
    public TimerManager timerManager;

    public TimerUI timerUI;

    private Timer gameTimer;

    private void Awake()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        playerInput.onReset += onReset;
        checkpointManager.OnEndCheckpointReached += onEndCheckpointReached;
        checkpointManager.OnCheckpointRecalling += onCheckpointRecalling;
        //
        gameTimer = timerManager.startTimer();
        timerUI.init(gameTimer);
        //
        onSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        gameTimer.reset(Time.time);
        gameTimer.start();
        checkpointManager.registerCheckpointDelegates();
        FindObjectsByType<Hazard>(FindObjectsSortMode.None).ToList()
            .ForEach(hazard => hazard.onPlayerHit += onHazardHit);
    }

    void onReset()
    {
        onHazardHit(null);
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
        levelManager.nextLevel();
        gameTimer.stop();
    }

    void onCheckpointRecalling(Checkpoint checkpoint)
    {
        playerController.resetState(checkpoint.transform.position);
    }
}
