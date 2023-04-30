using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAnimator : MonoBehaviour
{
    public List<Sprite> sprites;

    public PlayerController playerController;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerController.onPlayerStateChanged += updateFlowers;
        sr.sprite = sprites[0];
    }

    void updateFlowers(PlayerState playerState)
    {
        sr.sprite = sprites[playerState.stacks];
    }
}
