using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarwickController : Resettable
{
    public WarwickAttributes attr;

    public WarwickAnimator animator;
    public StaticHazard hazard;
    public Collider2D fearColl2D;
    public Hittable hittable;

    private Rigidbody2D rb2d;

    private Vector2 startPos;
    private WarwickState state;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hittable.onHit += fear;
        recordInitialState();
        reset();
        animator.processState(state);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float fixedTime = Time.fixedTime;
        if (state.phase == WarwickState.Phase.HOWLING)
        {
            if (fixedTime >= state.phaseStartTime + attr.howlDuration)
            {
                state.phaseStartTime = fixedTime;
                state.phase = WarwickState.Phase.CHASING;
                animator.processState(state);
            }
        }
        if (state.phase == WarwickState.Phase.CHASING)
        {
        Vector2 vel = Vector2.right * state.moveSpeed;
        vel.y = rb2d.velocity.y;
        rb2d.velocity = vel;
        state.moveSpeed += attr.moveSpeedIncrease * Time.fixedDeltaTime;
        }

        float fearEndTime = state.lastFearTime + attr.fearDelay + attr.fearDuration;
        bool fearing = fixedTime >= state.lastFearTime + attr.fearDelay
             && fixedTime <= fearEndTime;
        hazard.onTrigger = fearing;
        fearColl2D.enabled = fearing;
        if (fixedTime > fearEndTime && fixedTime - Time.fixedDeltaTime <= fearEndTime)
        {
            state.moveSpeed += attr.postFearMoveIncrease;
            animator.processState(state);
        }

    }

    private void fear()
    {
        if (Time.time >= state.lastFearTime + attr.fearDelay + attr.fearDuration)
        {
            state.moveSpeed += attr.onHitMoveIncrease;
            state.moveSpeed = Mathf.Max(state.moveSpeed, 0);
            state.lastFearTime = Time.fixedTime;
            animator.processState(state);
        }
    }

    public override void recordInitialState()
    {
        startPos = transform.position;
    }

    public override void reset()
    {
        transform.position = startPos;
        rb2d.velocity = Vector2.zero;
        //
        state = new()
        {
            phase = WarwickState.Phase.IDLE,
            moveSpeed = attr.moveSpeedInitial,
            lastFearTime = (attr.fearDelay + attr.fearDuration) * -2,
        };
        animator.processState(state);
    }

    public override bool reactsToPlayerStart => true;
    public override void levelStart()
    {
        state.phase = WarwickState.Phase.HOWLING;
        state.phaseStartTime = Time.fixedTime;
        animator.processState(state);
    }
}
