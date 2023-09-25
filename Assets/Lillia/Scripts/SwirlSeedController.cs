using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlSeedController : Resettable
{
    public PlayerAttributes playerAttributes;
    public Rigidbody2D rb2d;

    private SwirlSeedState state = new();

    public SwirlSeedState.Phase Phase
    {
        get => state.phase;
        set
        {
            state.phase = value;
            OnPhaseChanged?.Invoke(state.phase);
        }
    }
    public event Action<SwirlSeedState.Phase> OnPhaseChanged;

    private void Update()
    {
        if (state.phase == SwirlSeedState.Phase.ROLLING)
        {
            rb2d.velocity = new Vector2(state.velX, rb2d.velocity.y);
        }
    }
    public void Throw(bool initial, float dirX)
    {
        Phase = SwirlSeedState.Phase.FLYING;
        Vector2 vel = (initial)
            ? playerAttributes.swirlSeedLaunchVector
            : playerAttributes.swirlSeedBatVector;
        vel.x *= Mathf.Sign(dirX);
        rb2d.velocity = vel;
        state.velX = (initial)
            ? playerAttributes.swirlSeedRollSpeed
            : playerAttributes.swirlSeedRollSpeedFast;
        state.velX *= Mathf.Sign(dirX);
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
                    Phase = SwirlSeedState.Phase.ROLLING;
                }
                break;
            case SwirlSeedState.Phase.ROLLING:
                //If hit something that would stop it,
                if (Utility.HitWall(collision))
                {
                    Phase = SwirlSeedState.Phase.STOPPED;
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
            Throw(false, bloomingBlows.transform.parent.localScale.x);
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
