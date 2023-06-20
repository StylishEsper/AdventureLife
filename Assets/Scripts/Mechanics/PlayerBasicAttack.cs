using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    public PlayerController player;

    public string attackName;

    internal new Collider2D collider;

    private float startTime;
    private float colliderDelay;

    private void Start()
    {
        startTime = Time.time;
        collider = GetComponent<Collider2D>();
        collider.enabled = false;

        if (attackName == "Stab")
        {
            player.animator.SetTrigger("attack1");
            SetDelay(0.2f);
            colliderDelay = 0.1f;
            MoveWithPlayer();
        }
        else if (attackName == "Slash")
        {
            player.animator.SetTrigger("attack2");
            SetDelay(0.2f);
            colliderDelay = 0.15f;
            MoveWithPlayer();
        }
        else if (attackName == "Slash2")
        {
            player.animator.SetTrigger("attack3");
            SetDelay(0f);
            colliderDelay = 0.1f;
            MoveWithPlayer();
        }
        else if (attackName == "Slash3")
        {
            player.animator.SetTrigger("attack4");
            SetDelay(0.25f);
            colliderDelay = 0.1f;
            MoveWithPlayer();
        }
        else if (attackName == "JumpSlash")
        {
            player.animator.SetTrigger("jumpAttack");
            SetDelay(0f);
            colliderDelay = 0.05f;
            MoveWithPlayer();
        }
        else if (attackName == "CommandAttack")
        {
            SetDelay(0.2f);
            colliderDelay = 0.1f;
            MoveWithPlayer();
        }

        GetComponent<Absorbable>().energy = player.energy;
        GetComponent<AttackController>().player = player;
    }

    private void Update()
    {
        if (Time.time >= startTime + colliderDelay)
        {
            collider.enabled = true;
        }

        if (attackName == "Stab")
        {
            MoveWithPlayer();

            if (Time.time >= startTime + 0.5f)
            {
                SetAndDestroy();
            }
        }
        else if (attackName == "Slash")
        {
            MoveWithPlayer();

            if (Time.time >= startTime + 0.375f)
            {
                SetAndDestroy();
            }
        }
        else if (attackName == "Slash2")
        {
            MoveWithPlayer();

            if (Time.time >= startTime + 0.3335f)
            {          
                player.doubleSlashAvailable = false;

                if (player.attacksCount < 2)
                {
                    player.attacksCount = 0;
                }

                SetAndDestroy();
            }
        }
        else if (attackName == "Slash3")
        {
            MoveWithPlayer();

            if (Time.time >= startTime + 0.2915f)
            {
                player.tripleSlashAvailable = false;
                player.attacksCount = 0;

                SetAndDestroy();
            }
        }
        else if (attackName == "JumpSlash")
        {
            MoveWithPlayer();

            if (Time.time >= startTime + 0.45f)
            {
                SetAndDestroy();
            }
        }
        else if (attackName == "CommandAttack")
        {
            MoveWithPlayer();

            if (Time.time >= startTime + 0.375f)
            {
                SetAndDestroy();
            }
        }
    }

    public void MoveWithPlayer()
    {
        if (attackName == "Stab")
        {
            if (player.spriteRenderer.flipX)
            {
                transform.position = new Vector3(player.transform.position.x - 0.4f,
                    player.transform.position.y, 2f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x + 0.4f,
                    player.transform.position.y, 2f);
            }
        }
        else if (attackName == "Slash")
        {
            if (player.spriteRenderer.flipX)
            {
                transform.position = new Vector3(player.transform.position.x - 0.25f,
                    player.transform.position.y + 0.15f, 2f);

                transform.localScale = new Vector3(-1f, 0.9f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x + 0.25f,
                    player.transform.position.y + 0.15f, 2f);

                transform.localScale = new Vector3(1f, 0.9f);
            }
        }
        else if (attackName == "Slash2")
        {
            if (player.spriteRenderer.flipX)
            {
                transform.position = new Vector3(player.transform.position.x - 0.2f,
                    player.transform.position.y - 0.1f, 2);

                transform.localScale = new Vector3(-1.2f, 0.5f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x + 0.2f,
                    player.transform.position.y - 0.1f, 2);

                transform.localScale = new Vector3(1.2f, 0.5f);
            }
        }
        else if (attackName == "Slash3")
        {
            if (player.spriteRenderer.flipX)
            {
                transform.position = new Vector3(player.transform.position.x - 0.3f,
                    player.transform.position.y + 0.1f, 2);

                transform.localScale = new Vector3(-1.2f, transform.localScale.y);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x + 0.3f,
                    player.transform.position.y + 0.1f, 2);

                transform.localScale = new Vector3(1.2f, transform.localScale.y);
            }
        }
        else if (attackName == "JumpSlash")
        {
            transform.position = new Vector3(player.transform.position.x,
                player.transform.position.y, 2);
        }
        else if (attackName == "CommandAttack")
        {
            if (player.spriteRenderer.flipX)
            {
                transform.position = new Vector3(player.transform.position.x - 0.35f,
                    player.transform.position.y - 0.12f, 2f);

                transform.localScale = new Vector3(-1f, 1.1f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x + 0.35f,
                    player.transform.position.y - 0.12f, 2f);

                transform.localScale = new Vector3(1f, 1.1f);
            }
        }
    }

    public void SetAndDestroy()
    {
        player.attacking = false;
        player.attackDelayOn = true;
        player.attackTime = Time.time;
        Destroy(this.gameObject);
    }

    public void SetDelay(float delay)
    {
        player.attackDelay = delay;
    }
}
