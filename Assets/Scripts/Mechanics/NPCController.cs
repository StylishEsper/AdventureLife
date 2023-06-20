using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public GameObject player;

    public float distanceAwayX;
    public float distanceAwayY;

    public float maxRight;
    public float maxLeft;
    public float maxUp;
    public float maxDown;
    public float speed;
    public float patientLength;

    public string currentDirection;

    public bool followPlayer;
    public bool canFly;
    public bool standStill;
    public bool canBePatient;
    public bool isCasting;
    public bool isTeleporting;

    private AnimationController control;
    private new Rigidbody2D rigidbody2D;
    private PlayerController playerController;
    private SpellController spellController;
    private DialogController dialog;

    private Vector2 moveTo;

    private float patientTime;
    private float storeSpeed;
    private float distanceCountX;

    private bool maxXReached;
    private bool isPatient;
    private bool outOfRange;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        control = GetComponent<AnimationController>();
        spellController = GetComponent<SpellController>();

        foreach (Transform t in transform)
        {
            var d = t.gameObject.GetComponent<DialogController>();
            if (d != null)
            {
                dialog = d;
                break;
            }
        }

        if (followPlayer)
        {
            playerController = player.GetComponent<PlayerController>();
            transform.position = new Vector2(player.transform.position.x + distanceAwayX,
                player.transform.position.y + distanceAwayY);
        }

        isTeleporting = false;
        storeSpeed = speed;
    }

    private void Update()
    {
        if (followPlayer)
        {
            if (!isCasting && !spellController.finishAnim)
            {
                if (player.transform.position.x > transform.position.x)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }

            if (!outOfRange)
            {
                rigidbody2D.velocity = new Vector2(0, 0);
            }
            else
            {
                if (dialog.talking)
                {
                    speed = storeSpeed * 2;
                }
                else
                {
                    speed = storeSpeed;
                }

                Vector3 newP = Vector3.MoveTowards(transform.localPosition, moveTo,
                    Time.deltaTime * speed);

                rigidbody2D.MovePosition(newP);

                if (moveTo.x - transform.position.x >= 6 && !isTeleporting ||
                    moveTo.x - transform.position.x <= -8 && !isTeleporting ||
                    moveTo.y - transform.position.y >= 5 && !isTeleporting ||
                    moveTo.y - transform.position.y <= -5 && !isTeleporting)
                {
                    isTeleporting = true;
                    animator.SetTrigger("teleport");
                }
            }

            if (!isCasting)
            {
                if (!dialog.talking)
                {
                    CheckIfInRange(2);
                }
                else
                {
                    CheckIfInRange(1);
                }
            }
        }
        else
        {
            if (!standStill)
            {
                if (currentDirection == "right")
                {
                    control.move.x = speed;
                    distanceCountX += speed;

                    if (distanceCountX >= maxRight)
                    {
                        currentDirection = "left";
                        maxXReached = true;
                        distanceCountX = 0;
                    }
                }
                else if (currentDirection == "left")
                {
                    control.move.x = -speed;
                    distanceCountX += speed;

                    if (distanceCountX >= maxLeft)
                    {
                        currentDirection = "right";
                        maxXReached = true;
                        distanceCountX = 0;
                    }
                }
                else if (currentDirection == "up")
                {

                }
                else if (currentDirection == "down")
                {

                }

                if (canBePatient && maxXReached)
                {
                    storeSpeed = speed;
                    speed = 0;
                    isPatient = true;
                    maxXReached = false;
                    patientTime = Time.time;
                }

                if (isPatient && Time.time > patientTime + patientLength)
                {
                    isPatient = false;
                    speed = storeSpeed;
                }
            }
        }
    }

    public bool CheckIfInRange(int distance)
    {
        if (player.transform.position.x <= transform.position.x + distance &&
            player.transform.position.x >= transform.position.x - distance &&
            player.transform.position.y <= transform.position.y + distance &&
            player.transform.position.y >= transform.position.y - distance)
        {
            outOfRange = false;
        }
        else
        {
            outOfRange = true;

            moveTo = new Vector2(player.transform.position.x + distanceAwayX,
                player.transform.position.y + distanceAwayY);
        }

        return outOfRange;
    }

    public void TeleportInfront(bool inFront)
    {
        isTeleporting = true;
        isCasting = true;

        if (inFront)
        {
            if (playerController.spriteRenderer.flipX)
            {
                moveTo = new Vector2(player.transform.position.x - 1,
                    player.transform.position.y + 0.15f);
            }
            else
            {
                moveTo = new Vector2(player.transform.position.x + 1,
                    player.transform.position.y + 0.15f);
            }
        }
        else
        {
            if (playerController.spriteRenderer.flipX)
            {
                moveTo = new Vector2(player.transform.position.x + 1,
                    player.transform.position.y + 0.15f);
            }
            else
            {
                moveTo = new Vector2(player.transform.position.x - 1,
                    player.transform.position.y + 0.15f);
            }
        }

        spriteRenderer.flipX = playerController.spriteRenderer.flipX;

        animator.SetTrigger("teleport");
    }

    public void TeleportComplete()
    {
        transform.position = moveTo;
        isTeleporting = false;

        if (isCasting)
        {
            spellController.CastSpell();
        }
    }
}
