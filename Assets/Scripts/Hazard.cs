using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    protected void killPlayer()
    {
        FindObjectOfType<CheckpointManager>().teleportToCurrent();
    }
}
