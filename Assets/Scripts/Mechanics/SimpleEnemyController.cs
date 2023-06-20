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
    public class SimpleEnemyController : MonoBehaviour
    {
        public AudioClip ouch;
        public Health health;
        public SpriteRenderer spriteRenderer;
        public PlayerController player;
        public ParticleSystem frozenEffect;

        internal new Rigidbody2D rigidbody2D;
        internal AnimationController control;
        internal new Collider2D collider;
        internal new AudioSource audio;

        public float dropAgroDistanceX;
        public float dropAgroDistanceY;
        public float leftTravel;
        public float rightTravel;
        public float speed;
        public float patientLength;
        public float breakFreezeLength;
        public float frozenTime;

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
        private Invincibility invincibility;
        private Vector2 travelLocation1;
        private Vector2 travelLocation2;

        private float bounceBackTime;
        private float patientTime;
        private float storeSpeed;

        private bool isPatient;
        private bool bounceBack;
        private bool hasJumped;
        private bool killWhenGrounded;
        private bool oneEndReached;
        private bool onWall;

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
            audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            health = GetComponent<Health>();
            enemy = GetComponent<EnemyVerification>();
            invincibility = GetComponent<Invincibility>();

            travelLocation1 = new Vector2(transform.position.x + rightTravel, transform.position.y);
            travelLocation2 = new Vector2(transform.position.x - leftTravel, transform.position.y);
        }

        private void FixedUpdate()
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

            if (!isParalyzed && !isFrozen && !isAttacking && !isDead)
            {
                if (canAgro)
                {
                    if (autoAgro)
                    {
                        AutoAgroInRange();
                    }

                    if (agro && !bounceBack)
                    {
                        if (player.transform.position.x > transform.position.x)
                        {
                            control.move.x = speed;
                        }
                        else
                        {
                            control.move.x = -speed;
                        }

                        if (player.transform.position.x > transform.position.x + dropAgroDistanceX ||
                            player.transform.position.x < transform.position.x - dropAgroDistanceX ||
                            player.transform.position.y > transform.position.y + dropAgroDistanceY ||
                            player.transform.position.y < transform.position.y - dropAgroDistanceY ||
                            player.isDead)
                        {
                            agro = false;
                            //health.FullHeal();
                        }
                    }
                }

                if (canBounceBack)
                {
                    if (bounceBack && Time.time <= bounceBackTime + 0.5)
                    {
                        if (!hasJumped)
                        {
                            control.jump = true;
                            hasJumped = true;
                        }

                        if (player.transform.position.x > transform.position.x)
                        {
                            control.move.x = -0.2f;
                        }
                        else
                        {
                            control.move.x = 0.2f;
                        }
                    }
                    else if (bounceBack && Time.time >= bounceBackTime + 0.5)
                    {
                        bounceBack = false;
                        control.isKnockedBack = bounceBack;
                        control.move.y = 0;
                        hasJumped = false;
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

            control.animator.SetTrigger("hurt");
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
            
            if (reviveEnabled)
            {
                Schedule<EnemyRespawner>(3).enemy = this;
            }
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
                player.transform.position.y <= transform.position.y + dropAgroDistanceY &&
                player.transform.position.y >= transform.position.y - dropAgroDistanceY)
            {
                agro = true;
            }
        }

        public void RemoveSoul()
        {
            Destroy(gameObject);
        }
    }
}
