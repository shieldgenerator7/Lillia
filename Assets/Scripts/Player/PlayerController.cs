using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    public Transform bottom;

    public PlayerAttributes playerAttributes;

    private PlayerState playerState;
    public delegate void OnPlayerStateChanged(PlayerState playerState);
    public event OnPlayerStateChanged onPlayerStateChanged;

    private Rigidbody2D rb2d;
    private HashSet<GameObject> grounds = new HashSet<GameObject>();

    public void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        bool playerStateChanged = false;
        //Falling
        if (rb2d.velocity.y <= 0 && playerState.jumping)
        {
            playerState.jumping = false;
            playerState.falling = !playerState.grounded;
            playerStateChanged = true;
        }
        //Stack Decay
        if (playerState.stacks > 0)
        {
            if (Time.fixedTime >= playerState.lastStackAddTime + playerAttributes.stackDecayDelay)
            {
                if (Time.fixedTime >= playerState.lastStackDecayTime + playerAttributes.stackDecayDelayPerStack)
                {
                    playerState.lastStackDecayTime = Time.fixedTime;
                    setStacks(playerState.stacks - 1);
                    playerState.running = playerState.stacks > 0;
                    playerStateChanged = true;
                }
            }
        }
        //Wall Bounce expiring
        if (playerState.wallBouncing)
        {
            if (Time.fixedTime > playerState.lastWallBounceTime + playerAttributes.wallBounceDuration)
            {
                playerState.wallBouncing = false;
                playerStateChanged = true;
            }
        }
        //
        if (playerStateChanged)
        {
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
            //Check grounded state just to be safe
            playerState.grounded = checkGrounded();
            bool grounded = playerState.grounded;
            if (grounded)
            {
                playerState.airJumpsUsed = 0;
            }
            bool coyoteTime = (Time.time <= playerState.lastGroundTime + playerAttributes.coyoteTime);
            //
            if (!playerState.jumping && inputState.jump
                && (grounded || coyoteTime
                    || playerState.airJumpsUsed < playerAttributes.maxAirJumps
                )
                && !playerState.jumpConsumed
                )
            {
                playerState.jumping = true;
                playerState.jumpConsumed = true;
                playerState.falling = false;
                playerState.lastJumpTime = Time.time;
                if (!grounded && !coyoteTime)
                {
                    playerState.airJumpsUsed++;
                }
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
        if (collision.contacts.Length > 0 && collision.contacts[0].point.y < bottom.position.y)
        {
            playerState.grounded = true;
            playerState.airJumpsUsed = 0;
            playerState.lastGroundTime = Time.time;
            grounds.Add(collision.gameObject);
            onPlayerStateChanged?.Invoke(playerState);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length == 0 || collision.contacts[0].point.y < bottom.position.y)
        {
            grounds.Remove(collision.gameObject);
            if (grounds.Count == 0)
            {
                playerState.grounded = false;
            }
            playerState.lastGroundTime = Time.time;
            onPlayerStateChanged?.Invoke(playerState);
        }
    }

    private bool checkGrounded()
    {
        RaycastHit2D[] rch2ds = Physics2D.BoxCastAll(
            bottom.position,
            new Vector2(0.5f, 0.1f),
            0,
            Vector2.zero
            );
        for (int i = 0; i < rch2ds.Length; i++)
        {
            RaycastHit2D rch2d = rch2ds[i];
            Rigidbody2D rb2d = rch2d.rigidbody;
            if (rb2d)
            {
                //cant land on other things that move 
                //might change in future,
                //right now, its an easy way to prevent
                //player detecting themselves as a ground to stand on
                continue;
            }
            if (rch2d.point.y < bottom.position.y)
            {
                return true;
            }
        }
        return false;
    }

    public void BloomingBlowsHitSomething(bool hittable, bool wall)
    {
        if (hittable)
        {
            setStacks(playerState.stacks + 1);
            playerState.lastStackAddTime = Time.fixedTime;
        }
        if (wall)
        {
            //Refresh blooming blows duration
            playerState.lastStackAddTime = Time.fixedTime;
            //Wall bounce
            playerState.wallBouncing = true;
            playerState.lastWallBounceTime = Time.time;
            playerState.moveDirection *= -1;
        }
        playerState.running = playerState.stacks > 0;
        onPlayerStateChanged?.Invoke(playerState);
    }

    private void setStacks(int stacks)
    {
        playerState.stacks = Mathf.Clamp(
            stacks,
            0,
            playerAttributes.maxStacks
            );
    }
}
