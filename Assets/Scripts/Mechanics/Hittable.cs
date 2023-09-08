using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Hittable : Resettable
{
    [Tooltip("How many prance stacks hitting this grants")]
    public int stacksGranted = 1;
    [Tooltip("True if this hittable counts towards this level's collectable count")]
    public bool collectable = true;
    [Tooltip("True if it can be 'hit' just by walking into it")]
    public bool passiveable = true;

    [Tooltip("When it gets hit, it hides this collider")]
    public Collider2D hideCollider;

    public float lastHitTime { get; private set; }

    public void hit()
    {
        if (!enabled) { return; }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (passiveable)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.ProcessHittable(this);
            }
        }
    }

}
