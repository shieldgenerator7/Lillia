using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    public float coyoteTime = 0.1f;

    private PlayerState playerState;
    public delegate void OnPlayerStateChanged(PlayerState playerState);
    public event OnPlayerStateChanged onPlayerStateChanged;

    private Rigidbody2D rb2d;

    public void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (rb2d.velocity.y <= 0 && playerState.jumping)
        {
            playerState.jumping = false;
            playerState.falling = !playerState.grounded;
            onPlayerStateChanged?.Invoke(playerState);
        }
    }

    public void processInputState(InputState inputState)
    {
        //Movement
        playerState.moveDirection = inputState.movementDirection.x;
        //Blooming Blows
        playerState.usingBloomingBlows = inputState.bloomingblows;
        //Look Direction
        playerState.lookDirection = inputState.lookDirection;
        //Jumping
        if (playerState.jumping != inputState.jump)
        {
            if (!playerState.jumping && inputState.jump
                && (playerState.grounded || Time.time <= playerState.lastGroundTime + coyoteTime)
                && !playerState.jumpConsumed
                )
            {
                playerState.jumping = true;
                playerState.jumpConsumed = true;
                playerState.falling = false;
                //playerState.grounded = true;
            }
            else if (playerState.jumping && !inputState.jump)
            {
                playerState.jumping = false;
                if (!playerState.grounded)
                {
                    playerState.falling = true;
                }
            }
        }
        if (!inputState.jump)
        {
            playerState.jumpConsumed = false;
        }
        //Ability
        playerState.ability1 = inputState.ability1;
        playerState.ability2 = inputState.ability2;
        //Delegate
        onPlayerStateChanged?.Invoke(playerState);
    }


    ///TODO: move to some other script, perhaps the environment state updater one
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].point.y < transform.position.y)
        {
            playerState.grounded = true;
            playerState.lastGroundTime = Time.time;
            onPlayerStateChanged?.Invoke(playerState);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length == 0 || collision.contacts[0].point.y < transform.position.y)
        {
            playerState.grounded = false;
            playerState.lastGroundTime = Time.time;
            onPlayerStateChanged?.Invoke(playerState);
        }
    }

    public void BloomingBlowsHitSomething(bool hittable, bool wall)
    {
        if (hittable)
        {
            playerState.stacks++;
        }
        playerState.running = playerState.stacks > 0;
        onPlayerStateChanged?.Invoke(playerState);
    }
}
