using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree : Hittable
{
    public Transform trunk;
    public Collider2D hitColl2D;

    public float fallDuration = 3;
    public float endAngle = -90;

    private float startAngle;
    private float fallStartTime;

    public enum State
    {
        STANDING,
        FALLING,
        LYING,
    }
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        onHit += () =>
        {
            hitColl2D.enabled = false;
            setFalling(true);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.FALLING)
        {
            float ratio = (Time.time - fallStartTime) / fallDuration;
            ratio = Mathf.Min(ratio, 1);
            float angle = (endAngle - startAngle) * ratio + startAngle;
            setAngle(angle);
            if (ratio == 1)
            {
                setFalling(false);
            }
        }
    }

    private void setFalling(bool falling)
    {
        if (falling)
        {
            if (state == State.STANDING)
            {
                state = State.FALLING;
                fallStartTime = Time.time;
            }
        }
        else
        {
            if (state == State.FALLING)
            {
                state = State.LYING;
            }
        }
    }

    private void setAngle(float angle)
    {
        Vector3 angles = trunk.eulerAngles;
        angles.z = angle;
        trunk.eulerAngles = angles;
    }

    public override void recordInitialState()
    {
        startAngle = trunk.rotation.eulerAngles.z;
    }
    public override void reset()
    {
        state = State.STANDING;
        setAngle(startAngle);
    }
}
