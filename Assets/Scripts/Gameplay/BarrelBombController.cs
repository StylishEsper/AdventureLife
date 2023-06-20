using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBombController : MonoBehaviour
{
    public float tick;
    public float explosionX;
    public float explosionY;
    public float xForce;

    internal new Collider2D collider2D;

    internal new Rigidbody2D rigidbody2D;

    private GameObject explosion;

    private SpriteRenderer spriteRenderer;

    private float throwTime;
    private float explodedTime;
    private float blinkTime;

    private bool hasExploded;
    private bool isRed;

    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        throwTime = Time.time;
        blinkTime = Time.time;
        hasExploded = false;
        isRed = false;

        if (spriteRenderer.flipX)
        {
            rigidbody2D.AddForce(new Vector2(-xForce, 0));
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(xForce, 0));
        }
    }

    void Update()
    {
        if (Time.time > blinkTime + 0.5f)
        {
            blinkTime = Time.time;

            if (isRed)
            {
                spriteRenderer.color = new Color(255, 255, 255, 255);
                isRed = false;
            }
            else
            {
                spriteRenderer.color = new Color(255, 0, 0, 255);
                isRed = true;
            }
        }

        if (Time.time > throwTime + tick && !hasExploded)
        {
            spriteRenderer.enabled = false;

            if (rigidbody2D.velocity.y == 0)
            {
                explosion = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/BarrelBombExplosion"));
            }
            else
            {
                explosion = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/BarrelBombExplosionInAir"));
            }

            if (spriteRenderer.flipX)
            {
                explosion.GetComponent<SpriteRenderer>().flipX = true;
                CapsuleCollider2D cc = explosion.GetComponent<CapsuleCollider2D>();
                cc.offset = new Vector2(-cc.offset.x, cc.offset.y);
                explosion.transform.position = new Vector2(transform.position.x - explosionX, transform.position.y + explosionY);
            }
            else
            {
                explosion.GetComponent<SpriteRenderer>().flipX = false;
                explosion.transform.position = new Vector2(transform.position.x + explosionX, transform.position.y + explosionY);
            }

            explodedTime = Time.time;
            hasExploded = true;
        }

        if (hasExploded && Time.time > explodedTime + 0.6)
        {
            Destroy(explosion);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" && !hasExploded)
        {
            var bombDestroyedEffect = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/BombDestroyedEffect"));
            bombDestroyedEffect.transform.position = new Vector3(transform.position.x, 
                transform.position.y - 0.2f);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" && !hasExploded)
        {
            var bombDestroyedEffect = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/BombDestroyedEffect"));
            bombDestroyedEffect.transform.position = new Vector3(transform.position.x,
                transform.position.y - 0.2f);
            Destroy(this.gameObject);
        }
    }
}
