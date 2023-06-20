using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerPlayer : MonoBehaviour
{
    private Animator animator;
    private GameObject ball;

    private float startTime;
    private float kickDelay;

    private bool startDelay;

    private void Start()
    {
        animator = GetComponent<Animator>();
        startDelay = false;
        kickDelay = 0.25f;
    }

    private void Update()
    {
        if (startDelay && Time.time >= startTime + kickDelay)
        {
            string kickDirection = "";

            if (transform.position.x > ball.transform.position.x)
            {
                kickDirection = "L";
            }
            else if (transform.position.x < ball.transform.position.x)
            {
                kickDirection = "R";
            }

            ball.GetComponent<SoccerBall>().KickBall(kickDirection);

            startDelay = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            startDelay = true;
            ball = collision.gameObject;
            startTime = Time.time;
            animator.SetTrigger("kick");
        }
    }
}
