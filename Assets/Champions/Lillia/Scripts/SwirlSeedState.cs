using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SwirlSeedState 
{
    public float velX;
    public enum Phase
    {
        FLYING,
        ROLLING,
        STOPPED,
    }
    public Phase phase;
}
