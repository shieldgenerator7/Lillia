using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : Resettable
{
    public Animator animator;
    public Transform playerTransform;
    public SpriteRenderer swirlSeedRenderer;

    // Update is called once per frame
    public void UpdateAnimator(PlayerState playerState)
    {
        bool moving = playerState.moveDirection != 0;
        //Speed
        animator.SetBool("walking", moving && !playerState.running);
        animator.SetBool("running", moving && playerState.running);
        //Flip X
        float lookDirection = playerState.lookDirection.x;
        if (lookDirection != 0)
        {
            flip(lookDirection > 0);
        }
        //Swirlseed
        swirlSeedRenderer.enabled = playerState.swirlSeedAvailable;
    }

    void flip(bool right)
    {
        Vector3 scale = playerTransform.localScale;
        playerTransform.localScale = scale.setX(
            Mathf.Abs(scale.x) * ((right) ? 1 : -1)
            );
    }

    public override void recordInitialState()
    {
    }

    public override void reset()
    {
        flip(true);
    }
}
