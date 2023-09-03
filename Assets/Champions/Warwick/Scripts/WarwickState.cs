using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WarwickState 
{
    public enum Phase
    {
        IDLE,
        HOWLING,
        CHASING,
    }
    public Phase phase;

    public float moveSpeed;

    public float lastFearTime;
}
