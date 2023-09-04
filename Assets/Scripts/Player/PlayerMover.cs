using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public PlayerAttributes attributes;

    private Rigidbody2D rb2d;

    private PlayerState playerState = new PlayerState();

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void updatePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
        processPlayerState(playerState);
    }
    private void FixedUpdate()
    {
        processPlayerState(playerState);
    }
    public void processPlayerState(PlayerState playerState)
    {
        //Movement
        Vector2 vel = rb2d.velocity;
        float moveX = playerState.moveDirection
            * (attributes.walkSpeed + attributes.runSpeedPerStack * playerState.stacks);
        //Wall bounce
        if (playerState.wallBouncing)
        {
            if (playerState.lastWallBounceTime == Time.time)
            {
                //"Jump"
                vel.y = Mathf.Abs(vel.x) * attributes.wallBounceJumpFactor;
                //Increase speed
                vel.x *= attributes.wallBounceSpeedUpFactor;
            }
        }
        //Momentum dampening
        else if (moveX != vel.x)
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
        //Air Blooming Blows
        if (playerState.usingBloomingBlows)
        {
            if (!playerState.grounded && Time.time <= playerState.lastBloomingBlowTime + attributes.airBloomingBlowsAntiGravDuration)
            {
                if (vel.y < 0)
                {
                    vel.y = 0;
                }
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
