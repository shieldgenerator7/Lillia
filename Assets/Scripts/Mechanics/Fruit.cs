using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float hideAlpha = 0.5f;
    public Hittable hittable;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hittable.OnAvailableChanged += (available) =>
            sr.color = sr.color.setAlpha((available) ? 1 : hideAlpha);
    }
}
