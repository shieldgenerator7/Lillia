using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint End { get; private set; }

    public delegate void CheckpointEvent(Checkpoint cp);

    public void registerCheckpointDelegates()
    {
        End = FindAnyObjectByType<Checkpoint>();
        if (End)
        {
            End.onCheckPointReached += (cp) =>
            {
                OnEndCheckpointReached?.Invoke(End);
            };
        }
    }
    public event CheckpointEvent OnEndCheckpointReached;

}
