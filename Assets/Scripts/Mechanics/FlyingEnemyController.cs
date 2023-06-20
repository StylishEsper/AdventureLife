using Platformer.Gameplay;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Core.Simulation;

public class FlyingEnemyController : MonoBehaviour
{
    public Health health;
    public SpriteRenderer spriteRenderer;
    public PlayerController player;
    public GameObject bullet;
    public ParticleSystem frozenEffect;

    internal new Rigidbody2D rigidbody2D;
    internal new Collider2D collider;
    internal Animator animator;

    public float xSpeed;
    public float ySpeed;
    public float rightTravel;
    public float leftTravel;
    public float upTravel;
    public float downTravel;
    public float dropAgroDistance;
    public float patientLength;
    public float attackDelay;
    public float breakFreezeLength;
    public float frozenTime;

    public string currentXDirection;
    public string currentYDirection;

    public bool invincible;
    public bool standStill;
    public bool canAgro;
    public bool canBePatient;
    public bool isHarmless;
    public bool isStandable;
    public bool isDead;
    public bool disableXTravel;
    public bool disableYTravel;
    public bool agro;

    private EnemyVerification enemy;
    private Invincibility invincibility;
    private Vector2 travelLocation1;
    private Vector2 travelLocation2;

    private float patientTime;
    private float storeXSpeed;
    private float storeYSpeed;
    private float startTime;

    private bool isPatient;
    private bool oneEndReachedX;
    private bool oneEndReachedY;
    private bool killWhenGrounded;
    private bool onWall;
    private bool isAttacking;
    private bool isParalyzed;
    private bool isFrozen;
    private bool goBackX;
    private bool goBackY;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponent<EnemyVerification>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        invincibility = GetComponent<Invincibility>();

        storeXSpeed = xSpeed;
        storeYSpeed = ySpeed;

        startTime = Time.time;

        isAttacking = false;
        goBackX = false;
        goBackY = false;

        travelLocation1 = new Vector2(transform.position.x + rightTravel, transform.position.y);
        travelLocation2 = new Vector2(transform.position.x - leftTravel, transform.position.y);
    }

    private void Update()
    {
        if (!enemy.isDead)
        {
            isParalyzed = enemy.isParalyzed;
            isFrozen = enemy.isFrozen;
        }
        else
        {
            isParalyzed = false;
            enemy.isFrozen = false;
        }

        if (isFrozen)
        {
            if (Time.time >= frozenTime + breakFreezeLength)
            {
                enemy.Defrost();
            }

            if (isDead)
            {
                enemy.Defrost();
            }
        }

        if (!isAttacking)
        {
            if (canAgro && !isDead && !isParalyzed && !isFrozen)
            {
                if (agro && !invincible)
                {
                    if (Mathf.Round(player.transform.position.x) > Mathf.Round(transform.position.x))
                    {
                        xSpeed = Mathf.Abs(storeXSpeed);
                    }
                    else
                    {
                        xSpeed = -Mathf.Abs(storeXSpeed);
                    }

                    if (Mathf.Round(player.transform.position.y) > Mathf.Round(transform.position.y - 0.5f))
                    {
                        ySpeed = Mathf.Abs(storeYSpeed);
                    }
                    else
                    {
                        ySpeed = -Mathf.Abs(storeYSpeed);
                    }

                    if (Mathf.Round(player.transform.position.x) == Mathf.Round(transform.position.x))
                    {
                        xSpeed = 0;
                    }

                    if (Mathf.Round(player.transform.position.y) == Mathf.Round(transform.position.y - 0.5f))
                    {
                        ySpeed = 0;
                    }

                    if (player.transform.position.x > transform.position.x + dropAgroDistance ||
                        player.transform.position.x < transform.position.x - dropAgroDistance ||
                        player.transform.position.y > transform.position.y + dropAgroDistance ||
                        player.transform.position.y < transform.position.y - dropAgroDistance ||
                        player.isDead)
                    {
                        agro = false;

                        if (disableXTravel)
                        {
                            goBackX = true;
                            xSpeed = storeXSpeed;
                        }

                        if (disableYTravel)
                        {
                            goBackY = true;
                            ySpeed = storeYSpeed;
                        }

                        //health.FullHeal();
                    }
                }
            }

            if (!standStill && !agro && !isDead && !isParalyzed && !isFrozen)
            {
                if (!disableXTravel || goBackX)
                {
                    if (currentXDirection == "right")
                    {
                        xSpeed = Mathf.Abs(xSpeed);

                        if (transform.position.x >= travelLocation1.x)
                        {
                            if (goBackX)
                            {
                                goBackX = false;
                                xSpeed = 0;
                            }
                            else
                            {
                                currentXDirection = "left";
                                oneEndReachedX = true;
                            }
                        }
                    }
                    else if (currentXDirection == "left")
                    {
                        xSpeed = -Mathf.Abs(xSpeed);

                        if (transform.position.x <= travelLocation2.x)
                        {
                            if (goBackX)
                            {
                                goBackX = false;
                                xSpeed = 0;
                            }
                            else
                            {
                                currentXDirection = "right";
                                oneEndReachedX = true;
                            }
                        }
                    }

                    if (canBePatient && oneEndReachedX && !goBackX)
                    {
                        xSpeed = 0;
                        isPatient = true;
                        oneEndReachedX = false;
                        patientTime = Time.time;
                    }
                }
                else
                {
                    ySpeed = 0;
                }

                if (!disableYTravel || goBackY)
                {
                    if (currentYDirection == "up")
                    {
                        ySpeed = Mathf.Abs(ySpeed);

                        if (transform.position.y >= travelLocation1.y)
                        {
                            if (goBackY)
                            {
                                goBackY = false;
                                ySpeed = 0;
                            }
                            else
                            {
                                currentYDirection = "down";
                                oneEndReachedY = true;
                            }
                        }
                    }
                    else if (currentYDirection == "down")
                    {
                        ySpeed = -Mathf.Abs(ySpeed);

                        if (transform.position.y <= travelLocation2.y)
                        {
                            if (goBackY)
                            {
                                goBackY = false;
                                ySpeed = 0;
                            }
                            else
                            {
                                currentYDirection = "up";
                                oneEndReachedY = true;
                            }
                        }
                    }

                    if (canBePatient && oneEndReachedY)
                    {
                        ySpeed = 0;
                        isPatient = true;
                        oneEndReachedY = false;
                        patientTime = Time.time;
                    }
                }
                else
                {
                    ySpeed = 0;
                }

                if (isPatient && Time.time >= patientTime + patientLength)
                {
                    isPatient = false;
                    xSpeed = storeXSpeed;
                    ySpeed = storeYSpeed;
                }
            }

            if (!isParalyzed && !isFrozen && !invincible)
            {
                if (xSpeed > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else if (xSpeed < 0)
                {
                    spriteRenderer.flipX = true;
                }

                if (player.transform.position.x <= transform.position.x + dropAgroDistance &&
                    player.transform.position.x >= transform.position.x - dropAgroDistance &&
                    player.transform.position.y <= transform.position.y + dropAgroDistance &&
                    player.transform.position.y >= transform.position.y - dropAgroDistance)
                {
                    if (Time.time > startTime + attackDelay && !isDead)
                    {
                        isAttacking = true;
                        Attack();
                    }
                }

                if (!invincible)
                {
                    rigidbody2D.velocity = new Vector2(xSpeed, ySpeed);
                }
            }
            else
            {
                rigidbody2D.velocity = new Vector2(0, 0);
            }

        }
        else 
        {
            rigidbody2D.velocity = new Vector2(0, 0);

            if (player.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            if (Time.time > startTime + 0.52125f)
            {
                isAttacking = false;
            }
        }

        if (killWhenGrounded && rigidbody2D.velocity.y == 0 && onWall && !isAttacking)
        {
            rigidbody2D.simulated = false;
            animator.SetTrigger("hitGround");
            DissolveEffect dissolveEffect = gameObject.AddComponent<DissolveEffect>();
            dissolveEffect.SetDefaultForEnemy(spriteRenderer.material, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();

        if (!isDead && !invincible && !isHarmless)
        {
            if (player != null)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.stuck = false;
                ev.enemy = enemy;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (!isDead && !invincible && !isHarmless)
        {
            if (player != null)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = enemy;
            }
        }

        if (collision.gameObject.tag == "Wall")
        {
            onWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            onWall = false;
        }
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
        startTime = Time.time;
        GameObject bullet = this.bullet;
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y);
        bullet.GetComponent<BulletController>().target = player.transform;
        Instantiate(this.bullet);
    }

    public void Invincible()
    {
        invincibility.Invincible();
    }

    public void Hurt(bool isOvertime)
    {
        if (!isOvertime)
        {
            Invincible();
        }

        if (!isDead)
        {
            animator.SetTrigger("hurt");
        }

        if (canAgro)
        {
            agro = true;
            xSpeed = storeXSpeed;
            ySpeed = storeYSpeed;
        }
    }

    public void Harmless()
    {
        if (canAgro)
        {
            agro = false;
        }

        xSpeed = 0;
        ySpeed = 0;

        isAttacking = false;
        killWhenGrounded = true;

        rigidbody2D.gravityScale = 15;

        isDead = true;
        health.healthBar.Dissapear();
    }

    public void Freeze()
    {
        isFrozen = true;
        frozenEffect.Play();
        animator.speed = 0;
        isHarmless = true;
    }

    public void Defrost()
    {
        isFrozen = false;
        frozenEffect.Clear();
        frozenEffect.Stop();
        animator.speed = 1;
        isHarmless = false;
    }

    public void RemoveSoul()
    {
        Destroy(gameObject);
    }
}
