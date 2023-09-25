using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlSeedController : Resettable
{
    public PlayerAttributes playerAttributes;
    public Rigidbody2D rb2d;

    private SwirlSeedState state = new();

    private void Start()
    {
                state.phase = SwirlSeedState.Phase.FLYING;
                state.velX = (
                    playerAttributes.swirlSeedRollSpeed * Mathf.Sign(rb2d.velocity.x)
                    );
    }

    private void Update()
    {
        if (state.phase == SwirlSeedState.Phase.ROLLING)
        {
            rb2d.velocity = new Vector2(state.velX, rb2d.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (state.phase)
        {
            case SwirlSeedState.Phase.FLYING:
                //If it lands,
                if (Utility.HitFloor(collision))
                {
                    //Make it roll
                    state.phase = SwirlSeedState.Phase.ROLLING;
                }
                break;
            case SwirlSeedState.Phase.ROLLING:
                //If hit something that would stop it,
                if (Utility.HitWall(collision))
                {
                    throw new NotImplementedException("Need to put this part back in");
                }
                break;
            case SwirlSeedState.Phase.STOPPED: break;
            default: throw new UnityException("Unknown state! " + state.phase);
        }
        Hittable hittable = collision.gameObject.GetComponent<Hittable>();
        if (hittable)
        {
            if (!hittable.collectable)
            {
                onHitSomething?.Invoke(hittable);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //dont process if attached
        Hittable hittable = collision.GetComponent<Hittable>();
        if (hittable)
        {
            if (hittable.collectable)
            {
                onHitSomething?.Invoke(hittable);
            }
            return;
        }
        //Blooming Blows collects Swirlseed
        BloomingBlows bloomingBlows = collision.GetComponent<BloomingBlows>();
        if (bloomingBlows)
        {
            throw new NotImplementedException("Need to put this part back in");
            return;
        }
        //Watch Out Eep collects Swirlseed
        WatchOutEep watchOutEep = collision.transform.GetComponentInParent<WatchOutEep>();
        if (watchOutEep && !watchOutEep.Diving)
        {
            throw new NotImplementedException("Need to put this part back in");
            return;
        }
    }
    public delegate void OnHitSomething(Hittable hittable);
    public event OnHitSomething onHitSomething;

    public override void recordInitialState()
    {
    }
    public override void reset()
    {
        throw new NotImplementedException("Need to put this part back in");
    }
}
