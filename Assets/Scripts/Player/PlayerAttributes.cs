using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Player/PlayerAttributes")]
public class PlayerAttributes : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
}
