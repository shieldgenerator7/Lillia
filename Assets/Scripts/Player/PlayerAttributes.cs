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
    public int maxAirJumps;
}
