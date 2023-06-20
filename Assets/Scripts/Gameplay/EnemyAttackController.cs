using Platformer.Gameplay;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Core.Simulation;
using static EntityElement;

public class EnemyAttackController : MonoBehaviour
{
    public AttackName attackName;

    private int damage;

    private Element attackElement;

    private void Start()
    {
        if (attackName == AttackName.BarrelBomb)
        {
            damage = 20;
            attackElement = Element.Fire;
        }
        else if (attackName == AttackName.BattyBatBullet)
        {
            damage = 21;
            attackElement = Element.Poison;
        }
        else if (attackName == AttackName.BulletSword)
        {
            damage = 16;
            attackElement = Element.Elementless;
        }
        else if (attackName == AttackName.StickyBlob)
        {
            damage = 13;
            attackElement = Element.Elementless;
        }
        else if (attackName == AttackName.ChaoticWind)
        {
            damage = 46;
            attackElement = Element.Wind;
        }
        else if (attackName == AttackName.PoisonShot)
        {
            damage = 21;
            attackElement = Element.Poison;
        }
        else if (attackName == AttackName.ArachnaFFLeg)
        {
            damage = 33;
            attackElement = Element.Poison;
        }
        else if (attackName == AttackName.ArachnaFFCrush)
        {
            damage = 48;
            attackElement = Element.Poison;
        }
        else if (attackName == AttackName.ArachnaStinger)
        {
            damage = 13;
            attackElement = Element.Elementless;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionPoint = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);
        var player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null && !player.invincible && !player.isDead)
        {
            if (attackElement == Element.Poison && !player.isPoisoned)
            {
                player.GetComponent<PlayerStatusEffect>().SetAndStartEffect(PlayerStatusEffect.StatusEffect.Poisoned);
            }
            else if (attackElement == Element.Fire && !player.isBurned)
            {
                player.GetComponent<PlayerStatusEffect>().SetAndStartEffect(PlayerStatusEffect.StatusEffect.Burned);
            }
            else if (attackElement == Element.Ice && !player.isFrozen)
            {
                player.GetComponent<PlayerStatusEffect>().SetAndStartEffect(PlayerStatusEffect.StatusEffect.Frozen);
            }
            else if (attackElement == Element.Electric && !player.isParalyzed)
            {
                player.GetComponent<PlayerStatusEffect>().SetAndStartEffect(PlayerStatusEffect.StatusEffect.Paralyzed);
            }
            else
            {
                player.animator.SetTrigger("hurt");
            }

            if (attackName == AttackName.StickyBlob)
            {
                player.SlowDown();
            }

            player.Bounce(3);


            if (collisionPoint.x > transform.position.x)
            {
                player.Knockback("right");
            }
            else
            {
                player.Knockback("left");
            }

            player.Invincible();

            player.health.Decrement(damage, false, false, attackElement);
        }  
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var collisionPoint = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);
        var player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null && !player.invincible && !player.isDead)
        {
            if (attackElement == Element.Poison && !player.isPoisoned)
            {
                player.GetComponent<PlayerStatusEffect>().SetAndStartEffect(PlayerStatusEffect.StatusEffect.Poisoned);
            }
            else if (attackElement == Element.Fire && !player.isBurned)
            {
                player.GetComponent<PlayerStatusEffect>().SetAndStartEffect(PlayerStatusEffect.StatusEffect.Burned);
            }
            else if (attackElement == Element.Ice && !player.isFrozen)
            {
                player.GetComponent<PlayerStatusEffect>().SetAndStartEffect(PlayerStatusEffect.StatusEffect.Frozen);
            }
            else if (attackElement == Element.Electric && !player.isParalyzed)
            {
                player.GetComponent<PlayerStatusEffect>().SetAndStartEffect(PlayerStatusEffect.StatusEffect.Paralyzed);
            }
            else
            {
                player.animator.SetTrigger("hurt");
            }

            player.Bounce(3);

            if (collisionPoint.x > transform.position.x)
            {
                player.Knockback("right");
            }
            else
            {
                player.Knockback("left");
            }

            player.Invincible();
            player.health.Decrement(damage, false, false, attackElement);
        }
    }

    public enum AttackName
    {
        BarrelBomb,
        BattyBatBullet,
        BulletSword,
        StickyBlob,
        ChaoticWind,
        PoisonShot,
        ArachnaFFLeg,
        ArachnaFFCrush,
        ArachnaStinger
    }
}
