using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    protected float lastHitTime;

    public void hit()
    {
        lastHitTime = Time.time;
        onHit?.Invoke();
    }
    public delegate void OnHit();
    public event OnHit onHit;
}
