using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Resettable
{
    public float hideAlpha = 0.5f;
    public Hittable hittable;

    private bool available = true;
    public bool Available
    {
        get => available;
        set
        {
            available = value;
            sr.color = sr.color.setAlpha((available) ? 1 : hideAlpha);
        }
    }

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hittable.onHit += () =>
        {
            Available = false;
        };
    }

    public override void recordInitialState()
    {
    }
    public override void reset()
    {
        Available = true;
    }
}
