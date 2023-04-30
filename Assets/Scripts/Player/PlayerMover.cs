using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public PlayerAttributes attributesAnthro;
    public PlayerAttributes attributesFeral;

    private PlayerAttributes attributes;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void processPlayerState(PlayerState playerState)
    {
        //Transform
        if (playerState.grounded)
        {
            if (playerState.form == PlayerState.Form.ANTHRO)
            {
                attributes = attributesAnthro;
            }
            if (playerState.form == PlayerState.Form.FERAL)
            {
                attributes = attributesFeral;
            }
        }
        //Movement
        Vector2 vel = rb2d.velocity;
        vel.x = playerState.moveDirection
            * ((playerState.running) ? attributes.runSpeed : attributes.walkSpeed);
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
