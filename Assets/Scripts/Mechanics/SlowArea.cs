using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        checkSlow(coll, true);
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        checkSlow(coll, true);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        checkSlow(coll, false);
    }

    private void checkSlow(Collider2D coll, bool slow)
    {
        PlayerManager playerManager = coll.gameObject.GetComponent<PlayerManager>();
        if (playerManager)
        {
            playerManager.Slow = slow;
        }
    }
}
