using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Hittable : Resettable
{
    public int stacksGranted = 1;
    public bool collectable = true;

    [Tooltip("When it gets hit, it hides this collider")]
    public Collider2D hideCollider;

    public float lastHitTime { get; private set; }

    public void hit()
    {
        lastHitTime = Time.time;
        if (hideCollider)
        {
            hideCollider.enabled = false;
        }
        onHit?.Invoke();
    }
    public delegate void OnHit();
    public event OnHit onHit;

    public override void recordInitialState()
    {
        lastHitTime = -1;
    }

    public override void reset()
    {
        lastHitTime = -1;
        if (hideCollider)
        {
            hideCollider.enabled = true;
        }
    }

}
