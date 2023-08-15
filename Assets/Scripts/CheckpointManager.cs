using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{

    private Checkpoint current;

    [SerializeField]
    private CheckpointCollection checkpoints;


    public delegate void CheckpointEvent(Checkpoint cp);

    public void registerCheckpointDelegates()
    {
        checkpoints = FindAnyObjectByType<CheckpointCollection>();
        checkpoints.checkPoints.ForEach(cp =>
        {
            cp.markCurrent(false);
            cp.onCheckPointReached += switchCheckPoint;
        });

        teleportToStart();
        if (checkpoints.end)
        {
            checkpoints.end.onCheckPointReached += (cp) =>
            {
                OnEndCheckpointReached?.Invoke(checkpoints.end);
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
            OnCheckpointReached?.Invoke(current);
        }
    }
    public event CheckpointEvent OnCheckpointReached;

    public void teleportToStart()
    {
        switchCheckPoint(checkpoints.start);
        recallPlayer(checkpoints.start);
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
