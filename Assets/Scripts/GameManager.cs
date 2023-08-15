using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController;
    public CheckpointManager checkpointManager;
    public LevelManager levelManager;

    private void Awake()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        checkpointManager.OnEndCheckpointReached += onEndCheckpointReached;
        checkpointManager.OnCheckpointRecalling += onCheckpointRecalling;
        //
        onSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        checkpointManager.registerCheckpointDelegates();
        FindObjectsByType<Hazard>(FindObjectsSortMode.None).ToList()
            .ForEach(hazard => hazard.onPlayerHit += onHazardHit);
    }

    void onHazardHit(Hazard hazard)
    {
        //Teleport player
        checkpointManager.teleportToCurrent();
        //Reset fruits
        FindObjectsByType<Fruit>(FindObjectsSortMode.None).ToList()
            .ForEach(fruit => fruit.Available = true);
    }

    void onEndCheckpointReached(Checkpoint cp)
    {
        levelManager.nextLevel();
    }

    void onCheckpointRecalling(Checkpoint checkpoint)
    {
        playerController.resetState(checkpoint.transform.position);
    }
}
