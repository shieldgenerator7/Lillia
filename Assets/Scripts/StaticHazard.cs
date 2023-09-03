using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticHazard : Hazard
{
    public bool onCollide = false;
    public bool onTrigger = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!onCollide) { return; }
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            killPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!onTrigger) { return; }
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            killPlayer();
        }
    }
}
