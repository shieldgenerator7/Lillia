using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomingBlows : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;
    public Collider2D coll2d;

    private bool active;
    public bool Active
    {
        get => active;
        set
        {
            active = value;
            animator.SetBool("active", active);
            coll2d.enabled = active;
            onActiveChanged?.Invoke(active);
        }
    }
    public delegate void OnActiveChanged(bool active);
    public event OnActiveChanged onActiveChanged;

    // Start is called before the first frame update
    void Start()
    {
        playerController.onPlayerStateChanged += checkForActivation;
        Active = false;
    }

    void checkForActivation(PlayerState playerState)
    {
        Debug.Log($"checkForActivation playerState: {playerState.usingBloomingBlows}");
        if (playerState.usingBloomingBlows != active)
        {
            Active = playerState.usingBloomingBlows;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hittable"))
        {
            hitSomething();
        }
    }

    void hitSomething()
    {
        playerController.BloomingBlowsHitSomething(true, false);
    }
}
