using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SwirlSeed : MonoBehaviour
{
    public PlayerAttributes playerAttributes;
    public GameObject swirlSeedPrefab;

    public void updatePlayerState(PlayerState playerState)
    {
        if (playerState.usingSwirlSeed)
        {
            launch(playerState.lookDirection.x);
        }
    }

    public void launch(float dirX)
    {
        GameObject swirlSeed = Instantiate(swirlSeedPrefab);
        Rigidbody2D rb2d = swirlSeed.GetComponent<Rigidbody2D>();
        Vector2 vel = playerAttributes.swirlSeedLaunchVector;
        vel.x *= Mathf.Sign(dirX);
        rb2d.velocity = vel;
        swirlSeed.transform.position = transform.position;
        SwirlSeedController ssc = swirlSeed.GetComponent<SwirlSeedController>();
        ssc.onHitSomething += (hittable) => onHitSomething?.Invoke(hittable);
    }


    public event Action<Hittable> onHitSomething;
}
