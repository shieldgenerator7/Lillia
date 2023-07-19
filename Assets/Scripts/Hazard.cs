using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    protected void killPlayer()
    {
        FindObjectOfType<CheckpointManager>().teleportToCurrent();
        FindObjectsOfType<Fruit>().ToList().ForEach(fruit => fruit.Available = true);
    }
}
