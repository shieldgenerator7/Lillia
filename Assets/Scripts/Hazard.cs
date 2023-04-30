using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    protected void killPlayer()
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        pc.transform.position = Checkpoint.current.transform.position;
    }
}
