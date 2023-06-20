using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject hitEffect;

    public Transform target;

    public float speed;
    public float yForce;

    public bool isHoming;
    public bool ignoreWalls;
    public bool arcLikeTravel;

    internal Rigidbody2D rb;

    private float startTime;
    private float ignoreOnDelay;

    private bool ignoreDelayComplete;
    private bool stopMoving;

    private void Start()
    {
        startTime = Time.time;

        if (!isHoming)
        {
            rb = GetComponent<Rigidbody2D>();
            transform.right = target.position - transform.position;
            Vector2 move = (target.transform.position - transform.position).normalized * speed;
            rb.velocity = new Vector2(move.x, move.y);
        }

        if (!ignoreWalls)
        {
            ignoreOnDelay = Time.time;
            ignoreDelayComplete = false;
        }

        if (arcLikeTravel)
        {
            rb.AddForce(new Vector2(0, yForce));
        }
    }

    private void Update()
    {
        if (isHoming && !stopMoving)
        {
            transform.right = new Vector2(target.position.x, target.position.y) - 
                new Vector2(transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position,
                speed * Time.deltaTime); 
        }

        if (Time.time > startTime + 5f && !isHoming)
        {
            LoadHitEffect();
        }

        if (Time.time > startTime + 10f && isHoming)
        {
            LoadHitEffect();
        }

        if (!ignoreWalls && Time.time >= ignoreOnDelay + 0.05f)
        {
            ignoreDelayComplete = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" || collision.gameObject.tag == "Player")
        {
            LoadHitEffect();
        }
        else if (!ignoreWalls && collision.gameObject.tag == "Wall" && ignoreDelayComplete)
        {
            LoadHitEffect();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" || collision.gameObject.tag == "Player")
        {
            LoadHitEffect();
        }
        else if (!ignoreWalls && collision.gameObject.tag == "Wall" && ignoreDelayComplete)
        {
            LoadHitEffect();
        }
    }

    public void LoadHitEffect()
    {
        if (hitEffect != null)
        {
            hitEffect.transform.position = transform.position;
            hitEffect = Instantiate(hitEffect);
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<Animator>().SetTrigger("complete");
            GetComponent<EnemyAttackController>().enabled = false;
            GetComponent<Selfdestruct>().isEnabled = true;
            rb.velocity = new Vector2(0, 0);
            stopMoving = true;
        }
    }
}
