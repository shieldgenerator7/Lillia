using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public PlayerTrigger playerTrigger;

    void Start()
    {
        playerTrigger.OnPlayerEntered += Application.Quit;
    }
}
