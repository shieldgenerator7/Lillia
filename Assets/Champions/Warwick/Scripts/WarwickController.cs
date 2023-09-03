using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarwickController : Hittable
{
    public WarwickAttributes attr;

    public Animator animator;

    private Rigidbody2D rb2d;

    private Vector2 startPos;
    private WarwickState state;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator.SetBool("running", true);
        reset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = Vector2.right * state.moveSpeed;
        state.moveSpeed += attr.moveSpeedIncrease * Time.fixedDeltaTime;
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
    }
}
