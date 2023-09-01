using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SwirlSeedState 
{
    public float velX;
    public enum Phase
    {
        ATTACHED,
        FLYING,
        ROLLING,
        STOPPED,
    }
    public Phase phase;
}
