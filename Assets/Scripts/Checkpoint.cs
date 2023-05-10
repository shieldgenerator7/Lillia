using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Color activeColor = Color.white;
    public Color inactiveColor = Color.white;

    private SpriteRenderer sr;

    public void markCurrent(bool current = true)
    {
        sr.color = (current) ? activeColor : inactiveColor;
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

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
