using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SleepState 
{
    public int stacks;
    public float lastStackAddTime;
    public float lastStackDecayTime;
    public float drowsyStartTime;
    public enum Phase
    {
        AWAKE,
        DROWSY,
        SLEEPING,
    }
    public Phase phase;
}
