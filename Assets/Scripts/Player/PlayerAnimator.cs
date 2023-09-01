using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController.onPlayerStateChanged += UpdateAnimator;
    }

    // Update is called once per frame
    void UpdateAnimator(PlayerState playerState)
    {
        bool moving = playerState.moveDirection != 0;
        //Speed
        animator.SetBool("walking", moving && !playerState.running);
        animator.SetBool("running", moving && playerState.running);
        //Flip X
        float lookDirection = playerState.lookDirection.x;
        if (lookDirection != 0)
        {
            Vector3 scale = playerController.transform.localScale;
            playerController.transform.localScale = scale.setX(
                Mathf.Abs(scale.x) * Mathf.Sign(lookDirection)
                );
        }
    }
}
