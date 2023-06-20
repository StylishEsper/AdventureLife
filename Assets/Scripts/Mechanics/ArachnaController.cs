using Platformer.Gameplay;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Core.Simulation;

public class ArachnaController : MonoBehaviour
{
    //Pre-loaded and constants
    private Quaternion bottomFacingRight;
    private Quaternion bottomFacingLeft;
    private Quaternion topFacingRight;
    private Quaternion topFacingLeft;
    private Quaternion rightFacingRight;
    private Quaternion rightFacingLeft;
    private Quaternion leftFacingRight;
    private Quaternion leftFacingLeft;
    //

    //Public variables
    public bool invincible;
    public bool grounded;
    public bool somethingInFront;
    //

    //Private variables
    private Rigidbody2D rb;
    private Animator animator;
    private EnemyVerification enemy;
    private Invincibility invincibility;
    private Transform playerTransform;
    [SerializeField] private GameObject meleeAttack;
    [SerializeField] private GameObject stingerAttack;
    private Transform attackFireArea1;
    private Animator attackAnimator1;
    private Transform attackFireArea2;
    private Animator attackAnimator2;
    private Transform webCastArea;
    private Transform stingerArea;
    [SerializeField] private GameObject web;
    private GameObject currWeb;
    private LaserBehaviour laser;
    private GameObject bodyCharge;
    private GameObject crushAttack;
    private Animator crushAnimator;

    private WallDirection wallDirection;
    private WebCast currWebStage;

    [SerializeField] private float speed;
    private float kiteTime;
    private float attackTime;
    private float meleeTime;
    private float switchTime;
    private float webTime;
    private float topTime;
    private float delayMovementTime;
    private float stingTime;

    private bool playerIsRight;
    private bool isDead;
    private bool isAttacking;
    private bool kite;
    private bool startKiteDelay;
    private bool goingRight;
    private bool meleeAttacking;
    private bool playerTooClose;
    private bool meleeLaunched;
    private bool facingRight;
    private bool basic1Ready;
    private bool basic2Ready;
    private bool switchingWall;
    private bool chargingCrush;
    private bool startSmoke;
    private bool stinging;
    private bool stingerLaunched;
    //

    /// <summary>
    /// Sets some values before the first frame.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyVerification>();
        invincibility = GetComponent<Invincibility>();
        playerTransform = GameObject.Find("Player").transform;
        attackFireArea1 = transform.GetChild(5).transform;
        attackAnimator1 = transform.GetChild(5).GetComponent<Animator>();
        attackFireArea2 = transform.GetChild(6).transform;
        attackAnimator2 = transform.GetChild(6).GetComponent<Animator>();
        crushAnimator = transform.GetChild(10).GetComponent<Animator>();
        bodyCharge = transform.GetChild(8).gameObject;
        crushAttack = transform.GetChild(9).gameObject;
        webCastArea = transform.GetChild(7).transform;
        stingerArea = transform.GetChild(11).transform;
        web.GetComponent<LaserBehaviour>().laserFirePoint = webCastArea;
        attackTime = Time.time;
        meleeTime = Time.time;
        webTime = Time.time;
        stingTime = Time.time;

        bottomFacingRight = Quaternion.Euler(0, 0, 0);
        bottomFacingLeft = Quaternion.Euler(0, 180, 0);
        topFacingRight = Quaternion.Euler(0, 180, 180);
        topFacingLeft = Quaternion.Euler(0, 0, 180);
        rightFacingRight = Quaternion.Euler(0, 0, 90);
        rightFacingLeft = Quaternion.Euler(180, 0, 90);
        leftFacingRight = Quaternion.Euler(0, 180, 90);
        leftFacingLeft = Quaternion.Euler(180, 180, 90);

        stingerAttack.GetComponent<BulletController>().target = playerTransform;
    }

    /// <summary>
    /// Runs every frame. Does everything required for Arachna to function.
    /// </summary>
    private void Update()
    {
        if (!meleeAttacking && !stinging && !switchingWall)
        {
            if (startKiteDelay && Time.time >= kiteTime + 1.25f &&
                wallDirection == WallDirection.Bottom)
            {
                startKiteDelay = false;
            }

            if (startSmoke && grounded)
            {
                startSmoke = false;
                crushAnimator.SetTrigger("start");
            }

            if (!isAttacking && Time.time >= attackTime + 5)
            {
                ReadyBasicAttack(1);
                basic1Ready = false;
            }
            else if (isAttacking)
            {
                if (Time.time >= attackTime + 6.5f && !basic1Ready)
                {
                    basic1Ready = true;
                    LaunchBasicAttack(1);
                }

                if (Time.time >= attackTime + 5.75f && !basic2Ready)
                {
                    basic2Ready = true;
                    ReadyBasicAttack(2);
                }
                else if (Time.time >= attackTime + 7.25f && basic2Ready)
                {
                    basic2Ready = false;
                    LaunchBasicAttack(2);
                }
            }

            if (wallDirection == WallDirection.Bottom)
            {
                rb.gravityScale = 1;
            }
            else if (wallDirection == WallDirection.Top)
            {
                rb.gravityScale = -1;
            }

            if (grounded && !switchingWall)
            {
                PerformMovement();
            }

            if (!isAttacking && Time.time >= delayMovementTime + 1)
            {
                if (!stinging && Time.time >= stingTime + 4)
                {
                    ReadyStinger();
                }

                CheckPlayerTooClose();
                if (wallDirection == WallDirection.Bottom)
                {
                    if (playerTooClose && Time.time >= meleeTime + 5)
                    {
                        ReadyMeleeAttack();
                        meleeLaunched = false;
                        isAttacking = false;
                        attackTime = Time.time;
                    }
                }
            }
        }
        else if (stinging)
        {
            if (!stingerLaunched)
            {
                if (Time.time >= stingTime + 0.7f)
                {
                    LaunchStinger();
                }
            }
            else
            {
                if (Time.time >= stingTime + 1.3f)
                {
                    stingTime = Time.time;
                    stinging = false;
                }
            }
        }
        else if (meleeAttacking)
        {
            if (!meleeLaunched)
            {
                if (Time.time >= meleeTime + 0.7f)
                {
                    LaunchMeleeAttack();
                    meleeLaunched = true;
                }
            }
            else
            {
                if (Time.time >= meleeTime + 1.3f)
                {
                    meleeAttacking = false;
                }
            }
        }
        else if (switchingWall)
        {
            if (Time.time >= switchTime + 0.5 && currWebStage == WebCast.WebLoadRequired)
            {
                currWeb = Instantiate(web);
                laser = currWeb.GetComponent<LaserBehaviour>();

                if (wallDirection == WallDirection.Bottom)
                {
                    laser.fireDirection = LaserBehaviour.FireDirection.Up;
                }
                else if (wallDirection == WallDirection.Top)
                {
                    laser.fireDirection = LaserBehaviour.FireDirection.Down;
                }

                currWebStage = WebCast.WaitingForEndReached;
            }
            else if (currWebStage == WebCast.WaitingForEndReached)
            {
                if (laser.endReached)
                {
                    currWebStage = WebCast.EndReached;

                    if (wallDirection == WallDirection.Bottom)
                    {
                        rb.gravityScale = -1;
                    }
                    else if (wallDirection == WallDirection.Top)
                    {
                        rb.gravityScale = 10;
                    }

                    switchTime = Time.time;
                }
            }
            else if (currWebStage == WebCast.EndReached)
            {
                animator.SetTrigger("drag");
                currWebStage = WebCast.WaitingForPullCompletion;
            }
            else if (Time.time >= switchTime + 0.75 && currWebStage == WebCast.WaitingForPullCompletion)
            {
                currWebStage = WebCast.None;
                Destroy(currWeb);
                animator.SetTrigger("castComplete");

                if (wallDirection == WallDirection.Bottom)
                {
                    wallDirection = WallDirection.Top;
                    transform.position = new Vector3(transform.position.x,
                        transform.position.y - 2, transform.position.z);
                }
                else if (wallDirection == WallDirection.Top)
                {
                    wallDirection = WallDirection.Bottom;
                    transform.position = new Vector3(transform.position.x,
                        transform.position.y + 2, transform.position.z);
                }

                ChangeDirection();

                switchingWall = false;
                webTime = Time.time;
                topTime = Time.time;
                enemy.invincible = false;
            }

            if (chargingCrush && Time.time >= switchTime + 2)
            {
                animator.SetTrigger("crush");
                crushAttack.SetActive(true);
                bodyCharge.SetActive(false);
                chargingCrush = false;
                switchingWall = false;
                wallDirection = WallDirection.Bottom;
                ChangeDirection();
                delayMovementTime = Time.time;
                startSmoke = true;
            }
        }
    }

    /// <summary>
    /// Changes the rotation of Arachna based on its walking direction and wall direction.
    /// </summary>
    public void ChangeDirection()
    {
        if (wallDirection == WallDirection.Bottom)
        {
            if (rb.velocity.x < 0)
            {
                goingRight = false;
                transform.rotation = bottomFacingLeft;
            }
            else if (rb.velocity.x > 0)
            {
                goingRight = true;
                transform.rotation = bottomFacingRight;
            }
            else
            {
                FacePlayer();
            }
        }
        else if (wallDirection == WallDirection.Right)
        {
            if (rb.velocity.y < 0)
            {
                transform.rotation = rightFacingLeft;
            }
            else
            {
                transform.rotation = rightFacingRight;
            }
        }
        else if (wallDirection == WallDirection.Top)
        {
            if (rb.velocity.x < 0)
            {
                transform.rotation = topFacingLeft;
            }
            else
            {
                transform.rotation = topFacingRight;
            }
        }
        else if (wallDirection == WallDirection.Left)
        {
            if (rb.velocity.y < 0)
            {
                transform.rotation = leftFacingLeft;
            }
            else
            {
                transform.rotation = leftFacingRight;
            }
        }
    }

    /// <summary>
    /// Simply plays a charging animation for each basic attack depending on the
    /// given integer. Sets is attacking to true.
    /// </summary>
    /// <param name="i"></param>
    public void ReadyBasicAttack(int i)
    {
        if (i == 1)
        {
            isAttacking = true;
            animator.SetBool("attacking", true);
            attackAnimator1.SetTrigger("charge");
        }
        else
        {
            attackAnimator2.SetTrigger("charge");
        }
    }

    /// <summary>
    /// Launches a projectile in a specific location depending on given int.
    /// </summary>
    /// <param name="i"></param>
    public void LaunchBasicAttack(int i)
    {
        if (i == 1)
        {
            GameObject poisonShot = (GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/PoisonShot"));
            poisonShot.transform.position = new Vector3(attackFireArea1.position.x,
                attackFireArea1.position.y - 0.15f);
            poisonShot.GetComponent<BulletController>().target = playerTransform;
        }
        else
        {
            isAttacking = false;
            animator.SetBool("attacking", false);
            GameObject poisonShot = (GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/PoisonShot"));
            poisonShot.transform.position = new Vector3(attackFireArea2.position.x,
                attackFireArea2.position.y - 0.15f);
            poisonShot.GetComponent<BulletController>().target = playerTransform;
            attackTime = Time.time;
        }
    }

    /// <summary>
    /// Sets up Arachna for a melee attack.
    /// </summary>
    public void ReadyMeleeAttack()
    {
        rb.velocity = new Vector2(0, 0);
        animator.SetFloat("velocityX", 0);
        meleeAttacking = true;
        animator.SetBool("meleeAttack", meleeAttacking);
        meleeTime = Time.time;
        
        FacePlayer();
    }

    /// <summary>
    /// Sets up Arachna to shoot a ranged attack called Stinger.
    /// </summary>
    public void ReadyStinger()
    {
        rb.velocity = new Vector2(0, 0);
        animator.SetFloat("velocityX", 0);
        animator.SetTrigger("stinger");
        stinging = true;
        stingerLaunched = false;
        isAttacking = false;
        attackTime = Time.time;
        stingTime = Time.time;

        FacePlayer();
    }

    /// <summary>
    /// Forces Arachna to face the player. Only works if Arachna is walking on
    /// ground or ceiling.
    /// </summary>
    public void FacePlayer()
    {
        if (wallDirection == WallDirection.Bottom && !isAttacking)
        {
            if (playerTransform.position.x > transform.position.x)
            {
                transform.rotation = bottomFacingRight;
                facingRight = true;
            }
            else if ((playerTransform.position.x < transform.position.x))
            {
                transform.rotation = bottomFacingLeft;
                facingRight = false;
            }
        }
        else if (wallDirection == WallDirection.Top)
        {
            if (playerTransform.position.x > transform.position.x)
            {
                transform.rotation = topFacingRight;
                facingRight = true;
            }
            else if (playerTransform.position.x < transform.position.x)
            {
                transform.rotation = topFacingLeft;
                facingRight = false;
            }
        }
    }

    /// <summary>
    /// Launches a melee attack that selfdestructs after a short duration of existence.
    /// </summary>
    public void LaunchMeleeAttack()
    {
        var ma = Instantiate(meleeAttack);

        if (facingRight)
        {
            ma.transform.position = new Vector3(transform.position.x + 1.25f, transform.position.y - 1.3f);
        }
        else
        {
            ma.transform.position = new Vector3(transform.position.x - 1.25f, transform.position.y - 1.3f);
        }

        ma.transform.rotation = transform.rotation;
        Destroy(ma, 0.5f);
        meleeTime = Time.time;
    }

    public void LaunchStinger()
    {
        var s = Instantiate(stingerAttack);
        s.transform.position = new Vector3(stingerArea.position.x, stingerArea.position.y);
        stingerLaunched = true;
    }

    /// <summary>
    /// Checks if player is too close. Only occurs when Arachna is on the bottom wall.
    /// </summary>
    public void CheckPlayerTooClose()
    {
        if (wallDirection == WallDirection.Bottom)
        {
            playerTooClose = false;
            if (playerTransform.position.x > transform.position.x)
            {
                if (playerTransform.position.x < transform.position.x + 2)
                {
                    playerTooClose = true;
                }
            }
            else if (playerTransform.position.x < transform.position.x)
            {
                if (playerTransform.position.x > transform.position.x - 2)
                {
                    playerTooClose = true;
                }
            }
        }
    }

    /// <summary>
    /// Determine if Arachna should kite the player. Depends on distance from the player.
    /// </summary>
    public void ShouldKite()
    {
        if (!startKiteDelay)
        {
            kite = true;

            if (wallDirection == WallDirection.Bottom)
            {
                if (playerTransform.position.x > transform.position.x)
                {
                    playerIsRight = true;
                    if (playerTransform.position.x > transform.position.x + 4)
                    {
                        kite = false;
                    }
                }
                else if (playerTransform.position.x < transform.position.x)
                {
                    playerIsRight = false;
                    if (playerTransform.position.x < transform.position.x - 4)
                    {
                        kite = false;
                    }
                }
            }

            startKiteDelay = true;
            kiteTime = Time.time;
        }
    }

    /// <summary>
    /// Alters Arachna's movement to mimic kiting-like behaviour based on the players
    /// position.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="movement"></param>
    /// <returns></returns>
    public float MoveLikeMarksman(float n, float movement)
    {
        if (kite)
        {
            if (playerIsRight)
            {
                n = -movement;
            }
        }
        else
        {
            if (!playerIsRight)
            {
                n = -movement;
            }
        }

        return n;
    }

    /// <summary>
    /// Performs basic movement depending on the wall direction. May determine when to
    /// switch walls.
    /// </summary>
    public void PerformMovement()
    {
        ShouldKite();

        float movement = speed;
        float x = 0;
        float y = 0;

        if (wallDirection == WallDirection.Bottom)
        {
            x = movement;
            y = rb.velocity.y;

            x = MoveLikeMarksman(x, movement);

            if (grounded && somethingInFront && Time.time >= webTime + 5 && !isAttacking
                    && !meleeAttacking)
            {
                Web(WallDirection.Top);
            }
        }
        else if (wallDirection == WallDirection.Right)
        {
            rb.gravityScale = 0;
            x = 1;
        }
        else if (wallDirection == WallDirection.Top)
        {
            if (goingRight)
            {
                x = -movement;
            }
            else
            {
                x = movement;
            }
    
            y = rb.velocity.y;

            if (!isAttacking && Time.time >= topTime + 5)
            {
                Crush();
                return;
            }
        }
        else if (wallDirection == WallDirection.Left)
        {
            rb.gravityScale = 0;
            x = -1;
        }

        if (!switchingWall)
        {
            ChangeDirection();
        }

        if (Time.time >= delayMovementTime + 1)
        {
            rb.velocity = new Vector2(x, y);
        }
        else
        {
            x = 0;
        }

        animator.SetFloat("velocityX", Mathf.Abs(x));
    }

    /// <summary>
    /// Prepares Arachna to perform a crush attack.
    /// </summary>
    private void Crush()
    {
        switchingWall = true;
        isAttacking = false;
        attackTime = Time.time;
        switchTime = Time.time;
        bodyCharge.SetActive(true);
        chargingCrush = true;
        animator.SetFloat("velocityX", 0);
    }

    /// <summary>
    /// Prepares Arachna to shoot a web that will change the current wall direction.
    /// </summary>
    /// <param name="wall"></param>
    private void Web(WallDirection wall)
    {
        switchingWall = true;
        animator.SetTrigger("castingWeb");
        currWebStage = WebCast.WebLoadRequired;
        isAttacking = false;
        FacePlayer();
        attackTime = Time.time;
        switchTime = Time.time;
        webTime = Time.time;
        enemy.invincible = true;
    }

    /// <summary>
    /// Randomly decides a new wall direction.
    /// </summary>
    public void SwitchWalls()
    {
        var newWall = (WallDirection)Random.Range(0, 3);

        if (wallDirection == newWall)
        {
            if (wallDirection == WallDirection.Bottom)
            {
                newWall++;
            }
            else
            {
                newWall--;
            }
        }

        wallDirection = newWall;
    }

    /// <summary>
    /// Starts invincibility.
    /// </summary>
    /// <param name="isOvertime"></param>
    public void Hurt(bool isOvertime)
    {
        if (!isOvertime)
        {
            invincible = true;
            invincibility.Invincible();
        }
    }

    /// <summary>
    /// Hurts player when player touches Arachna.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();

        if (!isDead && !invincible)
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

    /// <summary>
    /// Hurts player when player is touching Arachna. Considers the player "stuck"
    /// and throws the player back a further distance.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (!isDead && !invincible)
        {
            if (player != null)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = enemy;
            }
        }
    }

    /// <summary>
    /// Provides stages for web casting.
    /// </summary>
    private enum WebCast
    {
        None,
        WebLoadRequired,
        WaitingForEndReached,
        EndReached,
        WaitingForPullCompletion
    }

    /// <summary>
    /// Names the walls in the room. Used to determine which wall Arachna is currently
    /// standing on.
    /// </summary>
    private enum WallDirection
    {
        Bottom,
        Top,
        Right,
        Left
    }
}
