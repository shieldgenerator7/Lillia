using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggered?.Invoke(collision);
    }
    public event Action<Collider2D> OnTriggered;
}
