using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityElement;

public class NegativeStatusController : MonoBehaviour
{
    public ParticleSystem burnEffect;
    public ParticleSystem poisonEffect;

    public float tick;

    private Transform targetLocation;
    private Health targetHealth;
    private EnemyVerification targetEnemy;
    private PlayerController targetPlayer;
    private Selfdestruct selfdestruct;

    private NegativeStatusType negativeStatus;

    private float damage;
    private float length;
    private float startTime;
    private float existTime;

    private void Start()
    {
        selfdestruct = GetComponent<Selfdestruct>();
        selfdestruct.isEnabled = false;
        startTime = Time.time;
        existTime = Time.time;
    }

    private void Update()
    {
        transform.position = targetLocation.position;

        if (negativeStatus == NegativeStatusType.Burn)
        {
            if (Time.time > startTime + tick)
            {
                if (targetPlayer != null && !targetPlayer.isDead ||
                    targetEnemy != null && !targetEnemy.isDead)
                {
                    if (damage >= 0)
                    {
                        targetHealth.Decrement((int)damage, true, false, Element.Fire);
                    }
                }
                startTime = Time.time;
            }

            if (Time.time > existTime + length || targetPlayer != null && targetPlayer.isDead ||
                targetEnemy != null && targetEnemy.isDead || targetPlayer != null && !targetPlayer.isBurned)
            {
                if (targetPlayer != null)
                {
                    targetPlayer.isBurned = false;
                }
                else if (targetEnemy != null)
                {
                    targetEnemy.isBurned = false;
                }

                burnEffect.Stop();
                selfdestruct.isEnabled = true;
            }
        }
        else if (negativeStatus == NegativeStatusType.Poison)
        {
            if (Time.time > startTime + tick)
            {
                if (targetPlayer != null && !targetPlayer.isDead ||
                    targetEnemy != null && !targetEnemy.isDead)
                {
                    if (damage >= 0)
                    {
                        targetHealth.Decrement((int)damage, true, false, Element.Poison);
                    }
                }

                startTime = Time.time;
            }

            if (Time.time > existTime + length || targetPlayer != null && targetPlayer.isDead ||
                targetEnemy != null && targetEnemy.isDead || targetPlayer != null && !targetPlayer.isPoisoned)
            {
                if (targetPlayer != null)
                {
                    targetPlayer.isPoisoned = false;
                }
                else if (targetEnemy != null)
                {
                    targetEnemy.isPoisoned = false;
                }

                poisonEffect.Stop();
                selfdestruct.isEnabled = true;
            }
        }
    }

    public void BurnTarget(PlayerController player, EnemyVerification enemy)
    {
        negativeStatus = NegativeStatusType.Burn;

        if (player != null)
        {
            targetHealth = player.GetComponent<Health>();
            targetPlayer = player.GetComponent<PlayerController>();
            targetLocation = player.transform;
            player.isBurned = true;

        }
        else if (enemy != null)
        {
            targetHealth = enemy.GetComponent<Health>();
            targetEnemy = enemy.GetComponent<EnemyVerification>();
            targetLocation = enemy.transform;
            enemy.isBurned = true;
        }

        damage = 4f;
        length = 5f;
        burnEffect.Play();
    }

    public void PoisonTarget(PlayerController player, EnemyVerification enemy)
    {
        negativeStatus = NegativeStatusType.Poison;

        if (player != null)
        {
            targetHealth = player.GetComponent<Health>();
            targetPlayer = player.GetComponent<PlayerController>();
            targetLocation = player.transform;
            player.isPoisoned = true;
        }
        else if (enemy != null)
        {
            targetHealth = enemy.GetComponent<Health>();
            targetEnemy = enemy.GetComponent<EnemyVerification>();
            targetLocation = enemy.transform;
            enemy.isBurned = true;
        }

        damage = 2f;
        length = 10f;
        poisonEffect.Play();
    }

    public enum NegativeStatusType
    {
        Burn,
        Poison
    }
}
