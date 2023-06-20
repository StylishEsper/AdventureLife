using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltSpell : MonoBehaviour
{
    public string boltType;

    public float speed;
    public float length;

    internal new Collider2D collider;
    internal new Rigidbody2D rigidbody2D;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Selfdestruct selfdestruct;

    private List<int> listOfID;

    private float startTime;
    private float hitTime;

    private int enemiesHit;

    private bool isCompleted;
    private bool onlyOnce;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        selfdestruct = GetComponent<Selfdestruct>();
        selfdestruct.isEnabled = false;
        listOfID = new List<int>();
        startTime = Time.time;
        enemiesHit = 0;
    }

    private void Update()
    {
        if (spriteRenderer.flipX)
        {
            rigidbody2D.velocity = new Vector2(-speed, 0);
        }
        else
        {
            rigidbody2D.velocity = new Vector2(speed, 0);
        }

        if (!isCompleted && Time.time > startTime + length && !onlyOnce)
        {
            animator.SetTrigger("spellComplete");
            collider.enabled = false;

            if (boltType != "Ice")
            {
                if (spriteRenderer.flipX)
                {
                    transform.position = new Vector2(transform.position.x - 0.6f, transform.position.y);
                }
                else
                {
                    transform.position = new Vector2(transform.position.x + 0.6f, transform.position.y);
                }
            }

            selfdestruct.isEnabled = true;
            onlyOnce = true;
        }

        if (listOfID.Count > 0 && Time.time >= hitTime + 0.5f)
        {
            listOfID.RemoveAt(0);
        }
    }

    public void BoltHit(EnemyVerification enemy, int id)
    {
        if (boltType == "Water")
        {
            bool add = true;

            foreach (int id2 in listOfID)
            {
                if (id2 == id)
                {
                    add = false;
                }
            }

            if (add)
            {
                listOfID.Add(id);
                enemiesHit++;
                hitTime = Time.time;
            }

            if (enemiesHit == 2)
            {
                End();
            }
        }
        else if (boltType == "Fire")
        {
            if (!enemy.isBurned)
            {
                GameObject burn = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/NegativeStatusEffect"));
                burn.GetComponent<NegativeStatusController>().BurnTarget(null, enemy);
            }

            End();
        }
        else if (boltType == "Electric")
        {
            if (!enemy.isParalyzed)
            {
                GameObject paralyze = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/Paralyze"));
                paralyze.GetComponent<ParalysisController>().paralyzedTarget = enemy.gameObject;
            }

            End();
        }
        else if (boltType == "Ice")
        {
            End();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        EnemyVerification enemy = collision.GetComponent<EnemyVerification>();
        bool enemyDeadCheck = false;

        if (enemy != null)
        {
            enemyDeadCheck = enemy.isDead;
        }

        if (collision.tag == "Wall")
        {
            End();
        }
        else if (collision.tag == "Monster" || collision.tag == "Barrel")
        {
            if (!enemyDeadCheck)
            {
                BoltHit(enemy, collision.GetInstanceID());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyVerification enemy = collision.GetComponent<EnemyVerification>();
        bool enemyDeadCheck = false;

        if (enemy != null)
        {
            enemyDeadCheck = enemy.isDead;         
        }

        if (collision.tag == "Wall")
        {
            End();
        }
        else if (collision.tag == "Monster" || collision.tag == "Barrel")
        {
            if (!enemyDeadCheck)
            {
                BoltHit(enemy, collision.GetInstanceID());
            }
        }
    }

    public void End()
    {
        if (!isCompleted)
        {
            speed = 0;
            animator.SetTrigger("spellComplete");
            selfdestruct.isEnabled = true;
            collider.enabled = false;

            if (boltType == "Water")
            {
                if (spriteRenderer.flipX)
                {
                    transform.position = new Vector2(transform.position.x - 0.6f, transform.position.y);
                }
                else
                {
                    transform.position = new Vector2(transform.position.x + 0.6f, transform.position.y);
                }
            }
            else if (boltType == "Fire")
            {
                if (spriteRenderer.flipX)
                {
                    transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
                }
                else
                {
                    transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y);
                }
            }

            isCompleted = true;
        }
    }
}
