using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    private SpriteRenderer sprite;
    private EnemyVerification enemy;
    private PlayerController player;

    [SerializeField] private float invincibleLength = 0.5f;
    private float storeInvincibleLength;
    private float invincibleTime;
    private float blinkTime;

    [SerializeField] private bool isPlayer;
    private bool invincible;
    private bool finishedInvincibleState;
    private bool blinkOff;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (isPlayer)
        {
            player = GetComponent<PlayerController>();
        }
        else
        {
            enemy = GetComponent<EnemyVerification>();
        }

        storeInvincibleLength = invincibleLength;
    }

    private void Update()
    {
        if (invincible && Time.time <= invincibleTime + invincibleLength)
        {
            finishedInvincibleState = false;

            if (Time.time >= blinkTime + 0.05f && !blinkOff)
            {
                if (sprite.enabled)
                {
                    sprite.enabled = false;
                }
                else
                {
                    sprite.enabled = true;
                }

                blinkTime = Time.time;
            }
        }
        else if (!finishedInvincibleState && Time.time >= invincibleTime + invincibleLength)
        {
            blinkOff = false;
            finishedInvincibleState = true;
            sprite.enabled = true;
            invincible = false;

            if (isPlayer)
            {
                player.invincible = false;
            }
            else
            {
                enemy.invincible = false;
            }
        }
    }

    public void Invincible()
    {
        invincible = true;
        invincibleTime = Time.time;

        if (isPlayer)
        {
            player.invincible = true;
            player.speedBoost = 1;
            player.runEffect.Stop();

            invincibleLength = 1f;
        }
        else
        {
            enemy.invincible = true;
            if (enemy.isFrozen)
            {
                invincibleLength = 0.25f;
            }
            else if (enemy.isParalyzed)
            {
                invincibleLength = 0.3f;
            }
            else
            {
                invincibleLength = storeInvincibleLength;
            }
        }
    }

    public void Invincible(float bTime)
    {
        invincible = true;
        invincibleTime = Time.time + bTime;
        player.invincible = true;
        blinkOff = true;
    }
}
