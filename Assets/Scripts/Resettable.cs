using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resettable : MonoBehaviour
{
    public abstract void recordInitialState();

    public abstract void reset();


    /// <summary>
    /// True to get notified when player starts the level timer
    /// </summary>
    public virtual bool reactsToPlayerStart => false;
    public virtual void levelStart()
    {
        throw new NotImplementedException($"levelStart not implemented {this.GetType()}");
    }
}
