using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputState
{
    public Vector2 movementDirection;
    public bool jump;
    public bool bloomingblows;
    public bool swirlseed;
    public bool interact;

    public override string ToString()
    {
        return $"move: {movementDirection}, jump: {jump}, bloomingblows: {bloomingblows}, swirlseed: {swirlseed}";
    }
}
