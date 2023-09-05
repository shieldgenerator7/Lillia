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
        if (playerState.usingBloomingBlows != active)
        {
            Active = playerState.usingBloomingBlows;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hittable hittable = collision.GetComponent<Hittable>();
        if (hittable)
        {
            onHitSomething?.Invoke(hittable);
            return;
        }
            if (collision.isTrigger)
            {
                return;
            }
            float wallOffDirection = Mathf.Sign(
                playerController.PlayerState.lookDirection.x
                - collision.gameObject.transform.position.x
                );
            float moveDirection = Mathf.Sign(playerController.moveDirection.x);
            if (wallOffDirection == moveDirection)
            {
                playerController.WallBounce();
            }
    }
    public delegate void OnHitSomething(Hittable hittable);
    public event OnHitSomething onHitSomething;
}
