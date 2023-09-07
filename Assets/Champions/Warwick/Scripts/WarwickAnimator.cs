using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarwickAnimator : MonoBehaviour
{
    public float fearDelaySize = 0.1f;
    [Tooltip("Local pos")]
    public Vector2 fearDelayPos;

    public Animator animator;

    public WarwickAttributes attr;
    public WarwickState state;

    public SpriteRenderer fearSR;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Fear
        bool fear = Time.time <= state.lastFearTime + attr.fearDelay + attr.fearDuration;
        fearSR.enabled = fear;
        if (fear)
        {
            bool delay = Time.time <= state.lastFearTime + attr.fearDelay;
            fearSR.gameObject.transform.localScale = Vector3.one * (
                (delay) ? fearDelaySize : 1
                );
            fearSR.gameObject.transform.localPosition = 
                (delay) ? fearDelayPos : Vector2.zero;
        }
    }

    public void processState(WarwickState state, bool asleep)
    {
        this.state = state;
        //Warwick
        animator.SetBool("howling", !asleep && state.phase == WarwickState.Phase.HOWLING);
        animator.SetBool("running", !asleep && state.phase == WarwickState.Phase.CHASING);
        animator.SetBool("sleeping", asleep);
    }
}
