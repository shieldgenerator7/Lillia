using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll2d)
    {
        PlayerController pc = coll2d.GetComponent<PlayerController>();
        if (pc)
        {
            OnPlayerEntered?.Invoke();
        }
    }
    public delegate void TriggerEvent();
    public event TriggerEvent OnPlayerEntered;
}
