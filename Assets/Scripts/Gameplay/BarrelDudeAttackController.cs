using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDudeAttackController : MonoBehaviour
{
    public float xRange;
    public float yRange;

    private SimpleEnemyController barrelDude;
    private Animator animator;
    private SpriteRenderer bombSprite;
    private GameObject bombAttack;

    private float attackCooldown;
    private bool attackReady;

    void Start()
    {
        barrelDude = GetComponent<SimpleEnemyController>();
        animator = barrelDude.control.animator;
        attackCooldown = Time.time;
        attackReady = false;
    }

    void Update()
    {
        if (!barrelDude.isDead && !barrelDude.isParalyzed && !barrelDude.isFrozen)
        {
            if (attackReady && barrelDude.control.move.x == 0)
            {
                animator.SetTrigger("attack");

                bombAttack = (GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/BarrelBombThrow"));
                bombSprite = bombAttack.GetComponent<SpriteRenderer>();

                if (barrelDude.spriteRenderer.flipX)
                {
                    bombSprite.flipX = true;
                    bombAttack.transform.position = new Vector3(transform.position.x - xRange,
                        transform.position.y + yRange, 2);
                    BoxCollider2D bc = bombAttack.GetComponent<BoxCollider2D>();
                    bc.offset = new Vector2(-bc.offset.x, bc.offset.y);
                }
                else
                {
                    bombSprite.flipX = false;
                    bombAttack.transform.position = new Vector3(transform.position.x + xRange,
                        transform.position.y + yRange, 2);
                }

                attackReady = false;
                attackCooldown = Time.time;
            }

            if (Time.time > attackCooldown + 5 && !attackReady)
            {
                attackReady = true;
            }
        }
    }
}
