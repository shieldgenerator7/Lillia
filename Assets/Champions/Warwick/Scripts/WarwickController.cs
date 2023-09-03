using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarwickController : Resettable
{
    public float moveSpeed;

    public Animator animator;

    private Rigidbody2D rb2d;

    private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator.SetBool("running", true);
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = Vector2.right * moveSpeed;
    }

    public override void recordInitialState()
    {
        startPos = transform.position;
    }

    public override void reset()
    {
        transform.position = startPos;
    }
}
