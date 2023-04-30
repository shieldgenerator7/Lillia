using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool startingCheckpoint = false;
    public Color activeColor = Color.white;
    public Color inactiveColor = Color.white;

    public static Checkpoint current;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = inactiveColor;
        if (startingCheckpoint)
        {
            switchCheckPoint(this);
            FindObjectOfType<PlayerController>().transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            switchCheckPoint(this);
        }
    }

    private void switchCheckPoint(Checkpoint newCurrent)
    {
        if (current)
        {
            current.sr.color = inactiveColor;
        }
        current = newCurrent;
        if (current)
        {
            current.sr.color = activeColor;
        }
    }
}
