using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerController playerController;
    public PlayerMover playerMover;
    public SwirlSeedController swirlSeedController;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput.onInputStateChanged += playerController.processInputState;
        playerController.onPlayerStateChanged += playerMover.updatePlayerState;
        playerInput.onInputStateChanged += swirlSeedController.processInputState;
        playerController.onPlayerStateChanged += swirlSeedController.updatePlayerState;
        swirlSeedController.onHitSomething += 
            (hittable) => playerController.BloomingBlowsHitSomething(true, false, hittable.stacksGranted);
    }
}
