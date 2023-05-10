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
        FindObjectOfType<PlayerController>().transform.position = start.transform.position;
    }

    public void teleportToCurrent()
    {
        FindObjectOfType<PlayerController>().transform.position = current.transform.position;
    }
}
