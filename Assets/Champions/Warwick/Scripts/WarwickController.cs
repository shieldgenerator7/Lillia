using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarwickController : Hittable
{
    public WarwickAttributes attr;

    public Animator animator;
    public StaticHazard hazard;

    private Rigidbody2D rb2d;

    private Vector2 startPos;
    private WarwickState state;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator.SetBool("running", true);
        onHit += fear;
        recordInitialState();
        reset();
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
    }
}
