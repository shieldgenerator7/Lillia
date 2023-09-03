using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarwickController : Hittable
{
    public WarwickAttributes attr;

    public WarwickAnimator animator;
    public StaticHazard hazard;

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

        float fixedTime = Time.fixedTime;
        hazard.onTrigger = fixedTime >= state.lastFearTime + attr.fearDelay
             && fixedTime <= state.lastFearTime + attr.fearDelay + attr.fearDuration;
    }

    private void fear()
    {
        state.lastFearTime = Time.fixedDeltaTime;
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
        state.moveSpeed = attr.moveSpeedInitial;
        state.lastFearTime = (attr.fearDelay + attr.fearDuration) * -2;
        animator.processState(state);
    }
}
