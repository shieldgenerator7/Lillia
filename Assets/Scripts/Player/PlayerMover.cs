using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public PlayerAttributes attributes;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void processPlayerState(PlayerState playerState)
    {
        //Movement
        Vector2 vel = rb2d.velocity;
        vel.x = playerState.moveDirection
            * (attributes.walkSpeed + attributes.runSpeedPerStack * playerState.stacks);
        if (playerState.jumping && playerState.grounded)
        {
            vel.y = attributes.jumpForce;
        }
        else if (!playerState.jumping && !playerState.grounded)
        {
            if (vel.y > 0)
            {
                vel.y = 0;
            }
        }
        rb2d.velocity = vel;
    }
}
