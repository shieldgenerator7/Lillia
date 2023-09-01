using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resettable : MonoBehaviour
{
    public abstract void recordInitialState();

    public abstract void reset();
}
