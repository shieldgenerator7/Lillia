using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputState
{
    public Vector2 movementDirection;
    public bool jump;
    public bool bloomingblows;
    public bool interact;
    public bool ability1;
    public bool ability2;

    public override string ToString()
    {
        return $"move: {movementDirection}, jump: {jump}, bloomingblows: {bloomingblows},";
    }
}
