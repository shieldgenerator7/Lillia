using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
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
        float moveX = playerState.moveDirection
            * (attributes.walkSpeed + attributes.runSpeedPerStack * playerState.stacks);
        //Wall bounce
        if (playerState.wallBouncing && playerState.lastWallBounceTime == Time.time)
        {
            //Reverse direction
            vel.x *= -1;
            //Increase speed
            vel.x *= attributes.wallBounceSpeedUpFactor;
            //"Jump"
            vel.y = attributes.wallBounceJumpForce;
        }
        //Momentum dampening
        if (moveX != vel.x)
        {
            vel.x = Mathf.Lerp(vel.x, moveX, Time.deltaTime * attributes.momentumChangeFactor);
            if (Mathf.Abs(moveX - vel.x) < attributes.momentumInstantSnapThreshold)
            {
                vel.x = moveX;
            }
        }
        //Jumping
        if (playerState.jumping && Time.time == playerState.lastJumpTime)
        {
            vel.y = attributes.jumpForce;
            if (playerState.airJumpsUsed > 0 && !playerState.grounded)
            {
                vel.y += playerState.stacks * attributes.jumpForcePerStack;
                vel.x = 0;
            }
        }
        //(Elective) Falling
        else if (!playerState.jumping && !playerState.grounded && !playerState.wallBouncing)
        {
            if (vel.y > 0)
            {
                vel.y = 0;
            }
        }
        //Assign velocity
        rb2d.velocity = vel;
    }
}
