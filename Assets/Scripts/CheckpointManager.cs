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


    public delegate void CheckpointEvent(Checkpoint cp);

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
                OnEndCheckpointReached?.Invoke(end);
            };
        }
    }
    public event CheckpointEvent OnEndCheckpointReached;

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
        OnCheckpointReached?.Invoke(current);
    }
    public event CheckpointEvent OnCheckpointReached;

    public void teleportToStart()
    {
        switchCheckPoint(start);
        recallPlayer(start);
    }

    public void teleportToCurrent()
    {
        recallPlayer(current);
    }

    private void recallPlayer(Checkpoint cp)
    {
        OnCheckpointRecalling(cp);
    }
    public event CheckpointEvent OnCheckpointRecalling;
}
