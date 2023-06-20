using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-3, 0);
    }

    private void Update()
    {
        if (rb.velocity.x != 0)
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 
                transform.rotation.z, transform.rotation.w);
        }
    }

    public void KickBall(string kickDirection)
    {
        if (kickDirection == "L")
        {
            rb.velocity = new Vector2(-3, 0);
        }
        else if (kickDirection == "R")
        {
            rb.velocity = new Vector2(3, 0);
        }
    }
}
