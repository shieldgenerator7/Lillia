using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hittable : Resettable
{
    public int stacksGranted = 1;

    protected float lastHitTime;

    public void hit()
    {
        lastHitTime = Time.time;
        onHit?.Invoke();
    }
    public delegate void OnHit();
    public event OnHit onHit;
}
