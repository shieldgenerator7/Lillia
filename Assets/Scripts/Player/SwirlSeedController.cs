using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlSeedController : Resettable
{

    public Transform attachPoint;
    public Rigidbody2D parentRB2D;

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
                rb2d.velocity = new Vector2(
                    playerAttributes.swirlSeedLaunchVector.x * Mathf.Sign(playerState.lookDirection.x) + state.velX,
                    playerAttributes.swirlSeedLaunchVector.y
                    );
            }
            //If not attached,
            else
            {
                //And in range
                if (Vector2.Distance(transform.position, parent.position) <= playerAttributes.swirlSeedPickupRange)
                {
                    //Pickup
                    attach(true);
                }
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
            rb2d.isKinematic = true;
        }
        else
        {
            if (state.phase == SwirlSeedState.Phase.ATTACHED)
            {
                state.phase = SwirlSeedState.Phase.FLYING;
                state.velX = parentRB2D.velocity.x;
            }
            transform.parent = null;
            rb2d.isKinematic = false;
        }
    }

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
                if (collision.contacts[0].point.y < transform.position.y)
                {
                    //Make it roll
                    state.phase = SwirlSeedState.Phase.ROLLING;
                }
                break;
            case SwirlSeedState.Phase.ROLLING:
                //If hit something that would stop it,
                if (Mathf.Sign(collision.contacts[0].point.x - transform.position.x) == Mathf.Sign(state.velX))
                {
                    //Make it stop
                    state.phase = SwirlSeedState.Phase.STOPPED;
                }
                break;
            case SwirlSeedState.Phase.STOPPED: break;
            default: throw new UnityException("Unknown state! " + state.phase);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state.phase == SwirlSeedState.Phase.ATTACHED) { return; }
        Hittable hittable = collision.GetComponent<Hittable>();
        if (hittable)
        {
            hittable.hit();
            onHitSomething?.Invoke();
        }
    }
    public delegate void OnHitSomething();
    public event OnHitSomething onHitSomething;

    public override void recordInitialState()
    {
    }
    public override void reset()
    {
        attach(true);
    }
}
