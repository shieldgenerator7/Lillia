using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollection : MonoBehaviour
{

    public Checkpoint start;
    public Checkpoint end;

    public List<Checkpoint> checkPoints;

    public void sort()
    {
        checkPoints.Sort((a, b) => a.name.CompareTo(b.name));
    }
}
