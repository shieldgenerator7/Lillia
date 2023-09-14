using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Resettable
{
    [Header("Attributes")]
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
            watchOutEep.playerAttributes = playerAttributes;
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

    [Header("Components")]
    public PlayerInput playerInput;
    public PlayerController playerController;
    public PlayerMover playerMover;
    public BloomingBlows bloomingBlows;
    public WatchOutEep watchOutEep;
    public SwirlSeedController swirlSeedController;
    public PlayerAnimator playerAnimator;
    public FlowerAnimator flowerAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        Slow = false;
        playerInput.onInputStateChanged += playerController.processInputState;
        playerController.onPlayerStateChanged += playerMover.updatePlayerState;
        playerController.onPlayerStateChanged += playerAnimator.UpdateAnimator;
        playerController.onPlayerStateChanged += flowerAnimator.updateFlowers;
        bloomingBlows.onHitSomething += playerController.ProcessHittable;
        playerController.onPlayerStateChanged += watchOutEep.updatePlayerState;
        watchOutEep.OnHitSomething += playerController.ProcessHittable;
        playerInput.onInputStateChanged += swirlSeedController.processInputState;
        playerController.onPlayerStateChanged += swirlSeedController.updatePlayerState;
        swirlSeedController.onHitSomething += playerController.ProcessHittable;
        swirlSeedController.onAttachedChanged += (attach) =>
        {
            if (attach)
            {
                playerController.ResetCooldowns();
            }
        };
    }

    public override void recordInitialState()
    {
    }

    public override void reset()
    {
        Slow = false;
    }
}
