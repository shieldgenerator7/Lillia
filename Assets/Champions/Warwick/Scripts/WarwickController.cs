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
    public Sleepable sleepable;
    public FlowerAnimator flowerAnimator;
    public Hittable dreamHittable;

    private Rigidbody2D rb2d;

    private Vector2 startPos;
    private WarwickState state;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hittable.onHit += checkFear;
        sleepable.onStacksChanged += updateSleepStacks;
        sleepable.onPhaseChanged += checkSleep;
        recordInitialState();
        reset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sleepable.Asleep) { return; }
        float fixedTime = Time.fixedTime;
        if (state.phase == WarwickState.Phase.HOWLING)
        {
            if (fixedTime >= state.phaseStartTime + attr.howlDuration)
            {
                state.phaseStartTime = fixedTime;
                state.phase = WarwickState.Phase.CHASING;
                animator.processState(state, sleepable.Asleep);
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
            animator.processState(state, sleepable.Asleep);
        }
    }

    private void checkFear()
    {
        if (sleepable.Asleep) { return; }
        if (Time.time >= state.lastFearTime + attr.fearDelay + attr.fearDuration)
        {
            fear();
        }
    }
    private void fear()
    {
        state.moveSpeed += attr.onHitMoveIncrease;
        state.moveSpeed = Mathf.Max(state.moveSpeed, 0);
        state.lastFearTime = Time.fixedTime;
        animator.processState(state, sleepable.Asleep);
    }

    private void checkSleep(SleepState.Phase phase)
    {
        sleep(phase == SleepState.Phase.SLEEPING);
    }

    private void updateSleepStacks(int stacks)
    {
        if (sleepable.Asleep)
        {
            stacks++;
        }
        flowerAnimator.updateFlowers(stacks);
    }

    private void sleep(bool asleep)
    {
        hazard.enabled = !asleep;
        if (asleep)
        {
            rb2d.velocity = Vector2.zero;
        }
        dreamHittable.enabled = asleep;
        dreamHittable.hideCollider.enabled = asleep;
        hittable.enabled = !asleep;
        animator.processState(state, asleep);
        updateSleepStacks(sleepable.Stacks);
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
        updateSleepStacks(0);
        checkSleep(SleepState.Phase.AWAKE);
    }

    public override bool reactsToPlayerStart => true;
    public override void levelStart()
    {
        state.phase = WarwickState.Phase.HOWLING;
        state.phaseStartTime = Time.fixedTime;
        animator.processState(state, sleepable.Asleep);
    }
}
