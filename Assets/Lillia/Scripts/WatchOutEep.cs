using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchOutEep : Resettable
{
    public PlayerAttributes playerAttributes;
    public Trigger goDive;
    public Trigger goSlam;

    public enum Phase
    {
        INACTIVE,
        DIVE,
        SLAM,
    }
    public Phase phase;

    public bool Diving => phase == Phase.DIVE;

    private PlayerState playerState;

    public void setPhase(Phase phase)
    {
        this.phase = phase;
        updateSprite();
    }

    private void Start()
    {
        goDive.OnTriggered += OnTriggerEnter2D;
        goSlam.OnTriggered += OnTriggerEnter2D;
        setPhase(Phase.INACTIVE);
    }

    private void Update()
    {
        switch (phase)
        {
            case Phase.INACTIVE: break;
            case Phase.DIVE: break;
            case Phase.SLAM:
                goSlam.transform.position = playerState.slamPos;
                break;
            default: throw new NotImplementedException();
        }
    }

    private void updateSprite()
    {
        goDive.gameObject.SetActive(phase == Phase.DIVE);
        goSlam.gameObject.SetActive(phase == Phase.SLAM);
    }

    public void updatePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
        if (playerState.usingWatchOutEep)
        {
            if (phase == Phase.INACTIVE)
            {
                setPhase(Phase.DIVE);
            }
        }
        if (playerState.grounded)
        {
            if (phase == Phase.DIVE)
            {
                setPhase(Phase.SLAM);
            }
        }
        if (playerState.usingSlam
            && Time.time <= playerAttributes.slamDuration + playerState.lastSlamTime
            )
        {
            setPhase(Phase.SLAM);

        }
        else if (phase== Phase.SLAM)
        {
            setPhase(Phase.INACTIVE);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hittable hittable = collision.GetComponent<Hittable>();
        if (hittable)
        {
            OnHitSomething?.Invoke(hittable);
        }
    }
    public event Action<Hittable> OnHitSomething;

    public override void recordInitialState()
    {
    }

    public override void reset()
    {
        setPhase(Phase.INACTIVE);
    }
}
