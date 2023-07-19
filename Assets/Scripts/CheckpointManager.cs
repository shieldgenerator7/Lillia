using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint start;
    public Checkpoint end;

    private Checkpoint current;

    [SerializeField]
    private List<Checkpoint> checkpoints;

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>().ToList();
        checkpoints.ForEach(cp =>
        {
            cp.markCurrent(false);
            cp.onCheckPointReached += switchCheckPoint;
        });

        teleportToStart();
        if (end)
        {
            end.onCheckPointReached += (cp) =>
            {
                FindObjectOfType<LevelManager>().nextLevel();
            };
        }
    }

    private void switchCheckPoint(Checkpoint newCurrent)
    {
        if (current)
        {
            current.markCurrent(false);
        }
        current = newCurrent;
        if (current)
        {
            current.markCurrent(true);
        }
    }

    public void teleportToStart()
    {
        switchCheckPoint(start);
        teleportPlayer(start.transform.position);
    }

    public void teleportToCurrent()
    {
        teleportPlayer(current.transform.position);
    }

    public void teleportPlayer(Vector2 pos)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.transform.position = pos;
        playerController.stop();
    }
}
