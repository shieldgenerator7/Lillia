using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarwickController : Hittable
{
    public WarwickAttributes attr;

    public WarwickAnimator animator;
    public StaticHazard hazard;
    public Collider2D fearColl2D;

    private Rigidbody2D rb2d;

    private Vector2 startPos;
    private WarwickState state;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        onHit += fear;
        recordInitialState();
        reset();
        animator.processState(state);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = Vector2.right * state.moveSpeed;
        state.moveSpeed += attr.moveSpeedIncrease * Time.fixedDeltaTime;

        if (state.moveSpeed > 1 && state.moveSpeed < 2)
        {
            state.phase = WarwickState.Phase.CHASING;
            animator.processState(state);
        }

        float fixedTime = Time.fixedTime;
        bool fearing  = fixedTime >= state.lastFearTime + attr.fearDelay
             && fixedTime <= state.lastFearTime + attr.fearDelay + attr.fearDuration;
        hazard.onTrigger = fearing;
        fearColl2D.enabled = fearing;

    }

    private void fear()
    {
        state.lastFearTime = Time.fixedTime;
        animator.processState(state);
    }

    public override void recordInitialState()
    {
        startPos = transform.position;
    }

    public override void reset()
    {
        transform.position = startPos;
        //
        state = new()
        {
            moveSpeed = attr.moveSpeedInitial,
            lastFearTime = (attr.fearDelay + attr.fearDuration) * -2
        };
        animator.processState(state);
    }
}
