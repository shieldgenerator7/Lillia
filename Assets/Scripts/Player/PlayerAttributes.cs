using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Player/PlayerAttributes")]
public class PlayerAttributes : ScriptableObject
{
    public float walkSpeed;
    public float runSpeedPerStack;
    public int maxStacks;
    public float jumpForce;
    public float coyoteTime = 0.1f;
    public float stackDecayDelay;
    public float stackDecayDelayPerStack;
    public int maxAirBloomingBlows;
    public float airBloomingBlowsJumpForce;
    public int maxAirJumps;
    public float momentumChangeFactor;
    public float momentumInstantSnapThreshold;
    public float jumpForcePerStack;
    public float wallBounceSpeedUpFactor;
    public float wallBounceJumpFactor;
    public float wallBounceDuration;
}
