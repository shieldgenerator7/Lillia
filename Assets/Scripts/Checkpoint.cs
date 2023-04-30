using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool startingCheckpoint = false;

    public static Checkpoint current;

    private void Start()
    {
        if (startingCheckpoint)
        {
            current = this;
            FindObjectOfType<PlayerController>().transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            current = this;
        }
    }
}
