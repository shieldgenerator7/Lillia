using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : Resettable
{
    public Transform bottom;

    public PlayerAttributes playerAttributes;

    private PlayerState playerState;
    public PlayerState PlayerState => playerState;
    public delegate void OnPlayerStateChanged(PlayerState playerState);
    public event OnPlayerStateChanged onPlayerStateChanged;

    private Rigidbody2D rb2d;
    private HashSet<GameObject> grounds = new HashSet<GameObject>();
    private Vector2 origPos;

    private InputState prevInputState;

    public void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        bool playerStateChanged = false;
        float fixedTime = Time.fixedTime;
        //Buffered Input
        if (playerState.nextBufferCheckTime > 0 && fixedTime >= playerState.nextBufferCheckTime)
        {
            processInputState(prevInputState);
        }
        //Blooming Blows
        if (playerState.usingBloomingBlows &&
            fixedTime > playerState.lastBloomingBlowTime + playerAttributes.bloomingBlowsDuration)
        {
            playerState.usingBloomingBlows = false;
            playerStateChanged = true;
        }
        //Slam
        if (playerState.usingSlam && fixedTime > playerState.lastSlamTime + playerAttributes.slamDuration)
        {
            playerState.usingSlam = false;
            playerStateChanged = true;
        }
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
            if (fixedTime >= playerState.lastStackAddTime + playerAttributes.stackDecayDelay)
            {
                if (fixedTime >= playerState.lastStackDecayTime + playerAttributes.stackDecayDelayPerStack)
                {
                    playerState.lastStackDecayTime = fixedTime;
                    setStacks(playerState.stacks - 1);
                    playerStateChanged = true;
                }
            }
        }
        //Wall Bounce expiring
        if (playerState.wallBouncing)
        {
            if (fixedTime > playerState.lastWallBounceTime + playerAttributes.wallBounceDuration)
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
        playerState.nextBufferCheckTime = -1;
        this.prevInputState = inputState;
        //Movement
        playerState.moveDirection = inputState.movementDirection.x;
        //Blooming Blows
        if (inputState.bloomingblows)
        {
            if (!playerState.usedBloomingBlows)
            {
                if (Time.time >= playerState.nextBloomingBlowTime)
                {
                    playerState.usedBloomingBlows = true;
                    playerState.usingBloomingBlows = true;
                    playerState.lastBloomingBlowTime = Time.time;
                    playerState.nextBloomingBlowTime = Time.time + playerAttributes.bloomingBlowsCooldown;
                    playerState.usingWatchOutEep = false;
                    if (!playerState.grounded)
                    {
                        playerState.airBloomingBlowsUsed++;
                    }
                }
                else
                {
                    //buffer the input until it comes off cooldown
                    setBufferTime(playerState.nextBloomingBlowTime);
                }
            }
        }
        else
        {
            playerState.usedBloomingBlows = false;
            playerState.usingBloomingBlows = false;
        }
        //Look Direction
        if (inputState.movementDirection.x != 0)
        {
            playerState.lookDirection = inputState.movementDirection;
        }
        else if (playerState.lookDirection.x == 0)
        {
            playerState.lookDirection.x = 1;
        }
        //Jumping
        bool prevJumpConsumed = playerState.jumpConsumed;
        if (playerState.jumping != inputState.jump)
        {
            bool grounded = playerState.grounded;
            if (grounded)
            {
                playerState.airJumpsUsed = 0;
                playerState.airBloomingBlowsUsed = 0;
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
                playerState.usingWatchOutEep = false;
                if (!grounded && !coyoteTime)
                {
                    playerState.airJumpsUsed++;
                }
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
        if (prevJumpConsumed && !playerState.jumpConsumed
            && playerState.airJumpsUsed > 0 && !playerState.usingBloomingBlows
            )
        {
            playerState.usingWatchOutEep = true;
            playerState.jumping = false;
        }
        //Watch Out Eep!
        if (!playerState.usingWatchOutEep && !playerState.grounded)
        {
            if (inputState.movementDirection.y < 0
                && Mathf.Abs(inputState.movementDirection.x) <= 0.1f)
            {
                playerState.usingWatchOutEep = true;
                playerState.jumping = false;
            }
        }
        //Delegate
        onPlayerStateChanged?.Invoke(playerState);
    }

    private void setBufferTime(float time)
    {
        playerState.nextBufferCheckTime = Mathf.Min(time, playerState.nextBufferCheckTime);
    }

    ///TODO: move to some other script, perhaps the environment state updater one
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Utility.HitFloor(collision))
        {
            playerState.grounded = true;
            playerState.airJumpsUsed = 0;
            playerState.lastGroundTime = Time.time;
            grounds.Add(collision.gameObject);
            //
            if (playerState.usingWatchOutEep)
            {
                playerState.usingWatchOutEep = false;
                playerState.slamPos = bottom.position;
                playerState.usingSlam = true;
                playerState.lastSlamTime = Time.time;
            }
            //
            onPlayerStateChanged?.Invoke(playerState);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length == 0 || Utility.HitFloor(collision))
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

    public void ResetCooldowns()
    {
        //Reset blooming blow cooldown
        playerState.nextBloomingBlowTime = -1;
    }

    public void ProcessHittable(Hittable hittable)
    {
        if (!hittable.Available) { return; }
        hittable.hit();
        setStacks(playerState.stacks + hittable.stacksGranted);
        playerState.lastStackAddTime = Time.fixedTime;
        ResetCooldowns();
        onPlayerStateChanged?.Invoke(playerState);
        if (hittable.collectable)
        {
            onCollectableCollected?.Invoke();
        }
    }
    public delegate void OnCollectableCollected();
    public event OnCollectableCollected onCollectableCollected;

    public void WallBounce(bool changeMoveDir = true)
    {
        //Refresh blooming blows duration
        playerState.lastStackAddTime = Time.fixedTime;
        //Wall bounce
        playerState.wallBouncing = true;
        playerState.lastWallBounceTime = Time.time;
        if (changeMoveDir)
        {
            playerState.moveDirection *= 1;
        }
    }

    private void setStacks(int stacks)
    {
        playerState.stacks = Mathf.Clamp(
            stacks,
            0,
            playerAttributes.maxStacks
            );
        playerState.running = playerState.stacks > 0;
    }

    public Vector2 moveDirection => rb2d.velocity;

    public void stop()
    {
        rb2d.velocity = Vector2.zero;
    }

    public override void recordInitialState()
    {
        origPos = transform.position;
    }

    public override void reset()
    {
        stop();
        playerState = new()
        {
            grounded = true,
        };
        transform.position = origPos;
        ResetCooldowns();
        onPlayerStateChanged?.Invoke(playerState);
    }
}
