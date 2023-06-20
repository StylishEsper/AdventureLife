using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstormSpell : MonoBehaviour
{
    public ParticleSystem cloud;
    public CollisionDetection projectileDetection;
    public CollisionDetection otherDetection;

    public float speed;
    public float length;
    public float attackDelay;

    internal new Rigidbody2D rigidbody2D;
    internal new Collider2D collider;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Selfdestruct selfdestruct;

    private float startTime;
    private float attackTime;

    private bool strikeable;
    private bool attackStarted;
    private bool windingUp;
    private bool attackTimeSet;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        selfdestruct = GetComponent<Selfdestruct>();
        collider = GetComponent<Collider2D>();

        collider.enabled = false;

        startTime = Time.time;

        attackTimeSet = false;
        attackStarted = false;
        windingUp = false;
        selfdestruct.isEnabled = false;
    }

    private void Update()
    {
        if (!selfdestruct.isEnabled)
        {
            if (otherDetection.collisionDetected || projectileDetection.collisionDetected)
            {
                strikeable = true;
            }
            else if (!otherDetection.collisionDetected && !projectileDetection.collisionDetected)
            {
                strikeable = false;
            }

            if (strikeable)
            {
                SetAttackTime();

                if (!attackStarted)
                {
                    if (Time.time > attackTime + (attackDelay - 0.4) && !windingUp)
                    {
                        windingUp = true;
                        animator.SetTrigger("strike");
                    }
                }
            }

            if (Time.time > attackTime + attackDelay && windingUp)
            {
                attackStarted = true;
                collider.enabled = true;
                windingUp = false;
            }

            if (Time.time > attackTime + (attackDelay + 0.5) && attackStarted)
            {
                collider.enabled = false;
                attackStarted = false;
                attackTimeSet = false;
                attackTime = Time.time;
            }
        }

        if (spriteRenderer.flipX)
        {
            rigidbody2D.velocity = new Vector2(-speed, 0);
        }
        else
        {
            rigidbody2D.velocity = new Vector2(speed, 0);
        }

        if (Time.time > startTime + length && !attackStarted && !windingUp)
        {
            spriteRenderer.enabled = false;
            selfdestruct.isEnabled = true;
            cloud.Stop();
        }
    }

    public void SetAttackTime()
    {
        if (!attackTimeSet)
        {
            attackTime = Time.time;
            attackTimeSet = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" || collision.tag == "Barrel")
        {
            GameObject paralyze = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/Paralyze"));
            paralyze.GetComponent<ParalysisController>().paralyzedTarget = collision.gameObject;
        }
    }
}
