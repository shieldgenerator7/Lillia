using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float dirChangeDelay = 0.2f;

    private float lastDirChangeTime = 0;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastDirChangeTime + dirChangeDelay)
        {
            lastDirChangeTime = Time.time;
            Vector2 moveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            rb2d.velocity = moveDir.normalized * moveSpeed;
        }
    }
}
