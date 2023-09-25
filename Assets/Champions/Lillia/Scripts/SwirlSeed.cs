using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SwirlSeed : MonoBehaviour
{
    public PlayerAttributes playerAttributes;
    public Spawner spawner;

    public void updatePlayerState(PlayerState playerState)
    {
        if (playerState.usingSwirlSeed && playerState.lastSwirlSeedTime == Time.time)
        {
            launch(playerState.lookDirection.x);
        }
    }

    public void launch(float dirX)
    {
        SwirlSeedController swirlSeed =
            spawner.SpawnObject<SwirlSeedController>(transform.position);
        swirlSeed.onHitSomething += (hittable) => onHitSomething?.Invoke(hittable);
        swirlSeed.OnPhaseChanged += (phase) =>
        {
            if (phase == SwirlSeedState.Phase.STOPPED)
            {
                spawner.DestroyObject(swirlSeed.gameObject);
            }
        };
        swirlSeed.Throw(true, dirX);
    }


    public event Action<Hittable> onHitSomething;
}
