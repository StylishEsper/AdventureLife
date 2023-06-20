using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalysisController : MonoBehaviour
{
    public GameObject paralyzedTarget;
    public Material fontMaterial;
    public Material spritesDefault;

    public float colorTick;
    public float effectTick;

    private EnemyVerification targetEnemy;
    private PlayerController player;
    private Selfdestruct selfdestruct;
    private Animator animator;


    private float colorTime;
    private float effectTime;
    private float existTime;

    private bool changeColor;

    private void Start()
    {
        selfdestruct = GetComponent<Selfdestruct>();
        animator = GetComponent<Animator>();

        if (paralyzedTarget.GetComponent<EnemyVerification>() != null)
        {
            targetEnemy = paralyzedTarget.GetComponent<EnemyVerification>();
            targetEnemy.isParalyzed = true;
            targetEnemy.animator.speed = 0;
        }
        else if (paralyzedTarget.GetComponent<PlayerController>() != null)
        {
            player = paralyzedTarget.GetComponent<PlayerController>();
            player.isParalyzed = true;
            player.controlEnabled = false;
        }

        colorTime = Time.time;
        effectTime = Time.time;
        existTime = Time.time;

        changeColor = true;
    }

    private void Update()
    {
        if (player == null)
        {
            transform.position = paralyzedTarget.transform.position;

            if (Time.time > colorTime + colorTick)
            {
                if (changeColor)
                {
                    targetEnemy.sprite.material = fontMaterial;
                    targetEnemy.sprite.color = new Color(1, 1, 0);
                    changeColor = false;
                }
                else
                {
                    targetEnemy.sprite.material = spritesDefault;
                    targetEnemy.sprite.color = new Color(1, 1, 1);
                    changeColor = true;
                }

                colorTime = Time.time;
            }

            if (Time.time > effectTime + effectTick)
            {
                animator.SetTrigger("playEffect");
                effectTime = Time.time;
            }

            if (Time.time > existTime + selfdestruct.seconds)
            {
                targetEnemy.sprite.material = spritesDefault;
                targetEnemy.animator.speed = 1;
                targetEnemy.sprite.color = new Color(1, 1, 1);
                targetEnemy.isParalyzed = false;
            }

            if (targetEnemy.isDead)
            {
                targetEnemy.sprite.material = spritesDefault;
                targetEnemy.animator.speed = 1;
                targetEnemy.sprite.color = new Color(1, 1, 1);
                targetEnemy.isParalyzed = false;
                selfdestruct.Now();
            }
        }
        else
        {
            transform.position = paralyzedTarget.transform.position;

            if (Time.time > colorTime + colorTick)
            {
                if (changeColor)
                {
                    player.spriteRenderer.material = fontMaterial;
                    player.spriteRenderer.color = new Color(1, 1, 0);
                    changeColor = false;
                }
                else
                {
                    player.spriteRenderer.material = spritesDefault;
                    player.spriteRenderer.color = new Color(1, 1, 1);
                    changeColor = true;
                }

                colorTime = Time.time;
            }

            if (Time.time > effectTime + effectTick)
            {
                animator.SetTrigger("playEffect");
                effectTime = Time.time;
            }

            if (Time.time > existTime + selfdestruct.seconds)
            {
                player.spriteRenderer.material = spritesDefault;
                player.spriteRenderer.color = new Color(1, 1, 1);
                player.isParalyzed = false;
                player.controlEnabled = true;
                player.animator.speed = 1;
            }

            if (!player.isParalyzed && !player.isDead)
            {
                player.spriteRenderer.material = spritesDefault;
                player.spriteRenderer.color = new Color(1, 1, 1);
                player.animator.speed = 1;
                player.controlEnabled = true;
                selfdestruct.Now();
            }

            if (player.isDead)
            {
                player.spriteRenderer.material = spritesDefault;
                player.spriteRenderer.color = new Color(1, 1, 1);
                player.isParalyzed = false;
                player.animator.speed = 1;
                selfdestruct.Now();
            }
        }
    }
}
