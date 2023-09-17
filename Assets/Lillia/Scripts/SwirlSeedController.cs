using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlSeedController : Resettable
{

    public Transform attachPoint;
    public Rigidbody2D parentRB2D;
    public Collider2D solidColl2D;

    public PlayerAttributes playerAttributes;

    private Rigidbody2D rb2d;
    private Transform parent;

    private SwirlSeedState state = new();
    private PlayerState playerState;

    private bool buttonDown = false;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        parent = transform.parent;
    }

    public void processInputState(InputState inputState)
    {
        if (inputState.swirlseed)
        {
            if (buttonDown) { return; }
            buttonDown = true;
            //If attached,
            if (state.phase == SwirlSeedState.Phase.ATTACHED)
            {
                //Throw
                attach(false);
                Vector2 vel = playerAttributes.swirlSeedLaunchVector;
                vel.x *= Mathf.Sign(playerState.lookDirection.x);
                rb2d.velocity = vel;
            }
        }
        else
        {
            buttonDown = false;
        }
    }
    public void updatePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
    }

    public void attach(bool attach)
    {
        if (attach)
        {
            state.phase = SwirlSeedState.Phase.ATTACHED;
            transform.parent = parent;
            transform.position = attachPoint.transform.position;
            state.velX = 0;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            transform.rotation = Quaternion.identity;
            rb2d.isKinematic = true;
            solidColl2D.enabled = false;
        }
        else
        {
            if (state.phase == SwirlSeedState.Phase.ATTACHED)
            {
                state.phase = SwirlSeedState.Phase.FLYING;
                state.velX = (
                    playerAttributes.swirlSeedRollSpeed * Mathf.Sign(playerState.lookDirection.x)
                    );
            }
            transform.parent = null;
            rb2d.isKinematic = false;
            solidColl2D.enabled = true;
        }
        onAttachedChanged?.Invoke(attach);
    }
    public delegate void OnAttachedChanged(bool attach);
    public event OnAttachedChanged onAttachedChanged;

    private void Update()
    {
        if (state.phase == SwirlSeedState.Phase.ATTACHED) { return; }
        if (state.phase == SwirlSeedState.Phase.ROLLING)
        {
            rb2d.velocity = new Vector2(state.velX, rb2d.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state.phase == SwirlSeedState.Phase.ATTACHED) { return; }        
        switch (state.phase)
        {
            case SwirlSeedState.Phase.ATTACHED: break;
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
                    //Make it stop, teleport back to player
                    attach(true);
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
                attach(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //dont process if attached
        if (state.phase == SwirlSeedState.Phase.ATTACHED) { return; }
        Hittable hittable = collision.GetComponent<Hittable>();
        if (hittable)
        {
            if (!hittable.collectable)
            {
                attach(true);
            }
            onHitSomething?.Invoke(hittable);
            return;
        }
        //Blooming Blows collects Swirlseed
        BloomingBlows bloomingBlows = collision.GetComponent<BloomingBlows>();
        if (bloomingBlows)
        {
            attach(true);
            return;
        }
        //Watch Out Eep collects Swirlseed
        WatchOutEep watchOutEep = collision.transform.GetComponentInParent<WatchOutEep>();
        if (watchOutEep && !watchOutEep.Diving)
        {
            attach(true);
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
        attach(true);
    }
}
