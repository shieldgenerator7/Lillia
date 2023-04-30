using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputState
{
    public Vector2 movementDirection;
    public Vector2 lookPosition;
    public Vector2 lookDirection;
    public bool jump;
    public bool run;
    public bool interact;
    public bool ability1;
    public bool ability2;

    public override string ToString()
    {
        return $"move: {movementDirection}, jump: {jump}, run: {run},";
    }
}
