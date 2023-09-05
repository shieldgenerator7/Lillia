using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            onCheckPointReached?.Invoke(this);
        }
    }
    public delegate void OnCheckPointReached(Checkpoint cp);
    public event OnCheckPointReached onCheckPointReached;
}
