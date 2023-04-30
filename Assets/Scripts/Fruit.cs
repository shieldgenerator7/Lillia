using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Hittable
{
    public float hideDuration = 4;
    [Range(0, 1)]
    public float hideAlpha = 0.5f;

    private bool available = true;
    public bool Available
    {
        get => available;
        set
        {
            available = value;
            sr.color = sr.color.setAlpha((available) ? 1 : hideAlpha);
            coll2d.enabled = available;
        }
    }

    private float lastAvailableTime;

    public Collider2D coll2d;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        onHit += () =>
        {
            Available = false;
            lastAvailableTime = Time.time;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!available)
        {
            if (Time.time > lastAvailableTime + hideDuration)
            {
                Available = true;
            }
        }
    }
}
