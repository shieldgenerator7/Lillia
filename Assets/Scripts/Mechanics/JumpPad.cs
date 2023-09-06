using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float launchForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb2d = collision.GetComponent<Rigidbody2D>();
        if (rb2d)
        {
            rb2d.velocity = Vector2.up * launchForce;
        }
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController)
        {
            playerController.WallBounce(false);
        }
    }
}
