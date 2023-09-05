using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    protected void killPlayer()
    {
        onPlayerHit?.Invoke(this);
    }
    public delegate void OnPlayerHit(Hazard hazard);
    public event OnPlayerHit onPlayerHit;
}
