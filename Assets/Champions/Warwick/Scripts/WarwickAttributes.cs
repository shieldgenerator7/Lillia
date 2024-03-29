using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WarwickAttr", menuName = "Champions/WarwickAttr")]
public class WarwickAttributes : ScriptableObject
{
    public float moveSpeedInitial;
    public float moveSpeedIncrease;
    public float moveSpeedMax;
    public float fearDelay;
    public float fearDuration;
    public float onHitMoveIncrease;
    public float postFearMoveIncrease;
    public float howlDuration;
}
