using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Resettable
{
    [SerializeField]
    private PlayerAttributes playerAttributes_normal;
    [SerializeField]
    private PlayerAttributes playerAttributes_slow;
    private PlayerAttributes playerAttributes;
    private PlayerAttributes PlayerAttributes
    {
        get => playerAttributes;
        set
        {
            playerAttributes = value;
            playerController.playerAttributes = playerAttributes;
            playerMover.attributes = playerAttributes;
        }
    }
    private bool slow = false;
    public bool Slow
    {
        get => slow;
        set
        {
            slow = value;
            PlayerAttributes = (slow) ? playerAttributes_slow : playerAttributes_normal;
        }
    }

    public PlayerInput playerInput;
    public PlayerController playerController;
    public PlayerMover playerMover;
    public SwirlSeedController swirlSeedController;

    // Start is called before the first frame update
    void Awake()
    {
        Slow = false;
        playerInput.onInputStateChanged += playerController.processInputState;
        playerController.onPlayerStateChanged += playerMover.updatePlayerState;
        playerInput.onInputStateChanged += swirlSeedController.processInputState;
        playerController.onPlayerStateChanged += swirlSeedController.updatePlayerState;
        swirlSeedController.onHitSomething +=
            (hittable) => playerController.BloomingBlowsHitSomething(true, false, hittable.stacksGranted);
    }

    public override void recordInitialState()
    {
    }

    public override void reset()
    {
        Slow = false;
    }
}
