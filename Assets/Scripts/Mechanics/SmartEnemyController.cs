using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class SmartEnemyController : MonoBehaviour
    {
        public Health health;
        public SpriteRenderer spriteRenderer;
        public Invincibility invincibility;
        public PlayerController player;
        public ParticleSystem frozenEffect;
        public GameObject bullet;

        internal new Rigidbody2D rigidbody2D;
        internal AnimationController control;
        internal new Collider2D collider;

        public float dropAgroDistanceX;
        public float dropAgroDistanceUp;
        public float dropAgroDistanceDown;
        public float leftTravel;
        public float rightTravel;
        public float speed;
        public float patientLength;
        public float breakFreezeLength;
        public float frozenTime;
        public float attackRange;

        public string currentDirection;

        public bool invincible;
        public bool standStill;
        public bool canAgro;
        public bool canBounceBack;
        public bool canBePatient;
        public bool isHarmless;
        public bool isStandable;
        public bool isDead;
        public bool reviveEnabled;
        public bool isParalyzed;
        public bool isFrozen;
        public bool isAttacking;
        public bool autoAgro;
        public bool agro;

        private EnemyVerification enemy;
        [SerializeField] private Transform maxDistanceRight;
        [SerializeField] private Transform maxDistanceLeft;
        private Vector2 travelLocation1;
        private Vector2 travelLocation2;
        private Vector2 maxRight;
        private Vector2 maxLeft;

        private float bounceBackTime;
        private float patientTime;
        private float storeSpeed;
        private float moveDelayTime;
        private float bounceLength;
        private float dodgeTime;
        private float attackStartTime;
        private float attackCompleteTime;

        private bool isPatient;
        private bool bounceBack;
        private bool hasJumped;
        private bool killWhenGrounded;
        private bool oneEndReached;
        private bool onWall;
        private bool inAttackRange;
        private bool startMoveDelay;
        private bool isDodge;
        private bool dodgeAvailable;
        private bool forceMove;
        private bool useDefinedMax;

        public Bounds Bounds => collider.bounds;

        private void Awake()
        {
            if (player == null)
            {
                player = GameObject.Find("Player").GetComponent<PlayerController>();
            }

            rigidbody2D = GetComponent<Rigidbody2D>();
            control = GetComponent<AnimationController>();
            collider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            invincibility = GetComponent<Invincibility>();
            health = GetComponent<Health>();
            enemy = GetComponent<EnemyVerification>();
        }

        private void Start()
        {
            travelLocation1 = new Vector2(transform.position.x + rightTravel, transform.position.y);
            travelLocation2 = new Vector2(transform.position.x - leftTravel, transform.position.y);
            dodgeAvailable = true;

            if (maxDistanceRight != null && maxDistanceLeft != null)
            {
                useDefinedMax = true;
                maxRight = maxDistanceRight.position;
                maxLeft = maxDistanceLeft.position;
            }
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

            if (!isParalyzed && !isFrozen && !isDead)
            {
                if (canAgro)
                {
                    if (autoAgro)
                    {
                        AutoAgroInRange();
                    }

                    if (agro && !bounceBack && !inAttackRange || forceMove && !isAttacking)
                    {
                        if (player.transform.position.x > transform.position.x)
                        {
                            control.move.x = speed;
                        }
                        else if (player.transform.position.x < transform.position.x)
                        {
                            control.move.x = -speed;
                        }
                    }

                    if (agro && !bounceBack && !forceMove)
                    {
                        if (player.transform.position.x > transform.position.x &&
                            transform.position.x >= player.transform.position.x - attackRange)
                        {
                            spriteRenderer.flipX = false;
                            inAttackRange = true;
                            control.move.x = 0;
                            startMoveDelay = false;

                            if (!isAttacking && Time.time >= attackCompleteTime + 3f)
                            {
                                control.animator.SetTrigger("attack");
                                isAttacking = true;
                                attackStartTime = Time.time;
                            }
                        }
                        else if (player.transform.position.x < transform.position.x &&
                                transform.position.x <= player.transform.position.x + attackRange)
                        {
                            spriteRenderer.flipX = true;
                            inAttackRange = true;
                            control.move.x = 0;
                            startMoveDelay = false;

                            if (!isAttacking && Time.time >= attackCompleteTime + 3f)
                            {
                                control.animator.SetTrigger("attack");
                                isAttacking = true;
                                attackStartTime = Time.time;
                            }
                        }
                        else if (!startMoveDelay)
                        {
                            moveDelayTime = Time.time;
                            startMoveDelay = true;
                        }

                        if (startMoveDelay && Time.time >= moveDelayTime + 0.5f)
                        {
                            inAttackRange = false;
                        }

                        if (player.transform.position.x > transform.position.x + dropAgroDistanceX ||
                            player.transform.position.x < transform.position.x - dropAgroDistanceX ||
                            player.transform.position.y > transform.position.y + dropAgroDistanceUp ||
                            player.transform.position.y < transform.position.y - dropAgroDistanceDown ||
                            player.isDead)
                        {
                            agro = false;
                            //health.FullHeal();
                        }
                    }
                }

                if (isAttacking && Time.time >= attackStartTime + 0.8f)
                {
                    Attack();
                    attackCompleteTime = Time.time;
                    isAttacking = false;
                }

                if (!dodgeAvailable && Time.time >= dodgeTime + 4f && !bounceBack)
                {
                    dodgeAvailable = true;
                }

                forceMove = false;

                if (CheckIfTooClose() && dodgeAvailable && agro)
                {
                    if (!isAttacking)
                    {
                        control.animator.SetTrigger("dodge");
                    }

                    bounceBack = true;
                    bounceBackTime = Time.time;
                    bounceLength = 0.25f;
                    control.isKnockedBack = bounceBack;
                    isDodge = true;
                    dodgeTime = Time.time;
                    dodgeAvailable = false;
                }
                else if (CheckIfTooClose() && !dodgeAvailable)
                {
                    forceMove = true;
                }

                if (canBounceBack)
                {
                    if (bounceBack && Time.time < bounceBackTime + bounceLength)
                    {
                        if (!hasJumped)
                        {
                            control.jump = true;
                            hasJumped = true;
                        }

                        if (player.transform.position.x > transform.position.x)
                        {
                            if (isDodge)
                            {
                                control.move.x = -1f;
                            }
                            else
                            {
                                control.move.x = -0.2f;
                            }
                        }
                        else
                        {
                            if (isDodge)
                            {
                                control.move.x = 1f;
                            }
                            else
                            {
                                control.move.x = 0.2f;
                            }
                        }
                    }
                    else if (bounceBack && Time.time >= bounceBackTime + bounceLength)
                    {
                        bounceBack = false;
                        control.isKnockedBack = bounceBack;
                        control.move.y = 0;
                        control.move.x = 0;
                        control.jump = false;
                        hasJumped = false;
                        isDodge = false;
                    }
                }

                if (!standStill && !agro && !isDead)
                {
                    if (currentDirection == "right")
                    {
                        control.move.x = speed;

                        if (transform.position.x >= travelLocation1.x)
                        {
                            currentDirection = "left";
                            oneEndReached = true;
                        }
                    }
                    else if (currentDirection == "left")
                    {
                        control.move.x = -speed;

                        if (transform.position.x <= travelLocation2.x)
                        {
                            currentDirection = "right";
                            oneEndReached = true;
                        }
                    }

                    if (canBePatient && oneEndReached)
                    {
                        storeSpeed = speed;
                        speed = 0;
                        isPatient = true;
                        oneEndReached = false;
                        patientTime = Time.time;
                    }
                }
            }
            else
            {
                control.move.x = 0;
            }

            if (isPatient && Time.time >= patientTime + patientLength)
            {
                isPatient = false;
                speed = storeSpeed;
            }

            if (useDefinedMax)
            {
                if (control.move.x > 0 && transform.position.x >= maxRight.x ||
                    control.move.x < 0 && transform.position.x <= maxLeft.x)
                {
                    control.move.x = 0;
                }
            }

            if (killWhenGrounded && control.velocity.y == 0 && onWall)
            {
                rigidbody2D.simulated = false;
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

        public bool CheckIfTooClose()
        {
            bool close = false;

            if (player.transform.position.x > transform.position.x &&
                transform.position.x >= player.transform.position.x - attackRange)
            {
                if (player.transform.position.x - transform.position.x <= 1.2f)
                {
                    close = true;
                }
                else
                {
                    close = false;
                }
            }
            else if (player.transform.position.x < transform.position.x &&
                    transform.position.x <= player.transform.position.x + attackRange)
            {
                if (transform.position.x - player.transform.position.x <= 1.2f)
                {
                    close = true;
                }
                else
                {
                    close = false;
                }
            }

            return close;
        }

        public void Attack()
        {
            GameObject bullet = this.bullet;

            if (spriteRenderer.flipX)
            {
                bullet.transform.position = new Vector3(transform.position.x + 0.18f, transform.position.y + 0.2f);
            }
            else
            {
                bullet.transform.position = new Vector3(transform.position.x - 0.18f, transform.position.y + 0.2f);
            }

            float yForce = 100f;

            if (attackRange <= 3)
            {
                yForce = 125f;
            }
            else if (attackRange > 3)
            {
                yForce = 150f;
            }

            var b = bullet.GetComponent<BulletController>();
            b.yForce = yForce;
            b.target = player.transform;
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

            if (!isAttacking)
            {
                control.animator.SetTrigger("hurt");
            }

            control.move.x = 0;

            if (canAgro)
            {
                agro = true;
            }

            if (canBounceBack && !isOvertime)
            {
                bounceBack = true;
                control.isKnockedBack = bounceBack;
                bounceBackTime = Time.time;
                bounceLength = 0.5f;
            }
        }

        public void Harmless()
        {
            DissolveEffect dissolveEffect = gameObject.AddComponent<DissolveEffect>();
            dissolveEffect.SetDefaultForEnemy(spriteRenderer.material, reviveEnabled);

            if (canAgro)
            {
                agro = false;
            }

            control.move.x = 0;

            if (!reviveEnabled)
            {
                killWhenGrounded = true;
            }

            isDead = true;
            control.isDead = true;
            health.healthBar.Dissapear();
        }

        public void Harmful()
        {
            isDead = false;
            control.isDead = false;
            health.healthBar.Appear();

            if (!reviveEnabled)
            {
                rigidbody2D.simulated = true;
            }

            health.FullHeal();
        }

        public void Freeze()
        {
            frozenTime = Time.time;
            isFrozen = true;
            frozenEffect.Play();
            control.animator.speed = 0;
            isHarmless = true;
        }

        public void Defrost()
        {
            isFrozen = false;
            frozenEffect.Clear();
            frozenEffect.Stop();
            control.animator.speed = 1;

            if (health.entity != Health.Entity.BarrelDude)
            {
                isHarmless = false;
            }
        }

        public void AutoAgroInRange()
        {
            if (player.transform.position.x <= transform.position.x + dropAgroDistanceX &&
                player.transform.position.x >= transform.position.x - dropAgroDistanceX &&
                player.transform.position.y <= transform.position.y + dropAgroDistanceUp &&
                player.transform.position.y >= transform.position.y - dropAgroDistanceDown)
            {
                agro = true;
            }
        }

        public void RemoveSoul()
        {
            Destroy(gameObject);
        }

        public void ResetEnemy()
        {
            Awake();
        }
    }
}
