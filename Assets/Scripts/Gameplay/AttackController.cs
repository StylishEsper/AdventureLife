using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityElement;

public class AttackController : MonoBehaviour
{
    public new Collider2D collider2D;
    public SpriteRenderer spriteRenderer;
    public PlayerController player;

    private EnergyBar energy;

    public string attackName;

    private int damage;
    private int storeDamage;

    private Element attackElement;

    private bool basicAttack;
    private bool critical;

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        energy = GetComponent<Absorbable>().energy;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        PlayerBasicAttack pBA = GetComponent<PlayerBasicAttack>();


        if (pBA != null)
        {
            player = pBA.player;
            attackName = pBA.attackName;
        }

        if (attackName == "Stab")
        {
            damage = 20;
            basicAttack = true;
            attackElement = Element.Elementless;
        }
        else if (attackName == "Slash")
        {
            damage = 10;
            basicAttack = true;
            attackElement = Element.Elementless;
        }
        else if (attackName == "Slash2")
        {
            damage = 10;
            basicAttack = true;
            attackElement = Element.Elementless;
        }
        else if (attackName == "Slash3")
        {
            damage = 10;
            basicAttack = true;
            attackElement = Element.Elementless;
        }
        else if (attackName == "JumpSlash")
        {
            damage = 15;
            basicAttack = true;
            attackElement = Element.Elementless;
        }
        else if (attackName == "CommandAttack")
        {
            damage = 23;
            basicAttack = true;
            attackElement = Element.Elementless;
        }
        else if (attackName == "Hurricane")
        {
            damage = 25;
            energy.Cost(75);
            attackElement = Element.Water;
        }
        else if (attackName == "Waterbolt")
        {
            damage = 30;
            energy.Cost(25);
            attackElement = Element.Water;
        }
        else if (attackName == "Firebolt")
        {
            damage = 30;
            energy.Cost(25);
            attackElement = Element.Fire;
        }
        else if (attackName == "Thunderbolt")
        {
            damage = 30;
            energy.Cost(25);
            attackElement = Element.Electric;
        }
        else if (attackName == "Thunder Cloud")
        {
            damage = 20;
            energy.Cost(75);
            attackElement = Element.Electric;
        }
        else if (attackName == "Icebolt")
        {
            damage = 30;
            energy.Cost(25);
            attackElement = Element.Ice;
        }

        if (basicAttack)
        {
            damage += player.bonusAtk;

            if (player.hellModeOn)
            {
                attackElement = Element.Fire;
                damage += 8;
            }
            else if (player.iceKingOn)
            {
                attackElement = Element.Ice;
            }
        }
        else
        {
            damage += player.bonusMgc;
        }

        storeDamage = damage;
    }

    public void CriticalHit()
    {
        if (!critical)
        {
            int number = Random.Range(1, 101);

            if (number <= player.bonusCrit)
            {
                critical = true;
            }
        }

        if (critical)
        {
            damage = storeDamage * 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        critical = false;
        damage = storeDamage;

        if (collision.gameObject.GetComponent<EnemyVerification>())
        {
            var entity = collision.gameObject.GetComponent<EnemyVerification>();
            var entityHealth = entity.GetComponent<Health>();

            if (entity != null && !entity.invincible && !entity.isDead)
            {
                GameObject enemyHitEffect = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/EnemyHitEffect"));
                enemyHitEffect.transform.position = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

                if (basicAttack && player.hellModeOn && !entity.isBurned)
                {
                    GameObject burn = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/NegativeStatusEffect"));
                    burn.GetComponent<NegativeStatusController>().BurnTarget(null, entity);
                }

                if (basicAttack && player.iceKingOn && !entity.isFrozen || 
                    attackName == "Icebolt" && !entity.isFrozen)
                {
                    entity.Freeze();
                }
                else if (entity.isFrozen)
                {
                    entity.Defrost();
                    critical = true;
                }

                CriticalHit();

                entityHealth.Decrement(damage, false, critical, attackElement);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!basicAttack)
        {
            critical = false;
            damage = storeDamage;

            if (collision.gameObject.GetComponent<EnemyVerification>())
            {
                var entity = collision.gameObject.GetComponent<EnemyVerification>();
                var entityHealth = entity.GetComponent<Health>();

                if (entity != null && !entity.invincible && !entity.isDead)
                {
                    GameObject enemyHitEffect = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/EnemyHitEffect"));
                    enemyHitEffect.transform.position = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

                    if (entity.isFrozen)
                    {
                        entity.Defrost();
                        critical = true;
                    }

                    CriticalHit();

                    entityHealth.Decrement(damage, false, critical, attackElement);
                }
            }
        }
    }
}
