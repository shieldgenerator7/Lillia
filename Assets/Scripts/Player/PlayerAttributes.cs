using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Player/PlayerAttributes")]
public class PlayerAttributes : ScriptableObject
{
    [Header("Prance")]
    public float walkSpeed;
    public float runSpeedPerStack;
    public int maxStacks;
    public float stackDecayDelay;
    public float stackDecayDelayPerStack;
    [Header("Jump")]
    public float jumpForce;
    public float coyoteTime = 0.1f;
    [Header("Blooming Blows")]
    public float bloomingBlowsDuration = 0.25f;
    public float bloomingBlowsCooldown = 0.5f;
    public int maxAirBloomingBlows;
    public float airBloomingBlowsAntiGravDuration;
    [Header("Watch out! Eep!")]
    public int maxAirJumps;
    public float momentumChangeFactor;
    public float momentumInstantSnapThreshold;
    public float jumpForcePerStack;
    public float slamFallSpeed;
    public float slamDuration;
    [Header("Swirlseed")]
    public Vector2 swirlSeedLaunchVector;
    public float swirlSeedRollSpeed = 5;
    public float swirlSeedPickupRange = 2;
    [Header("Wall Bounce")]
    public float wallBounceSpeedUpFactor;
    public float wallBounceJumpFactor;
    public float wallBounceDuration;
}
