using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneSpell : MonoBehaviour
{
    public float speed;
    public float length;

    internal new Rigidbody2D rigidbody2D;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float startTime;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startTime = Time.time;
    }

    void Update()
    {
        if (spriteRenderer.flipX)
        {
            rigidbody2D.velocity = new Vector2(-speed, 0);
        }
        else
        {
            rigidbody2D.velocity = new Vector2(speed, 0);
        }

        if (Time.time > startTime + length)
        {
            animator.SetTrigger("spellComplete");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Monster" || collision.tag == "Barrel" || collision.tag == "Ball"
            || collision.tag == "Item")
        {
            if (transform.position.x > collision.transform.position.x)
            {
                collision.transform.position = new Vector2(collision.transform.position.x + 0.05f,
                    collision.transform.position.y);
            }
            else
            {
                collision.transform.position = new Vector2(collision.transform.position.x - 0.05f,
                    collision.transform.position.y);
            }
        }
    }
}
