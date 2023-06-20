using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusEffect : MonoBehaviour
{
    public StatusEffect currentStatusEffect;

    private PlayerController player;

    private float frozenTime;
    private float breakFreezeLength = 3;

    private bool animtorStopped;
    private bool wasCommanding;
    
    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (wasCommanding && !player.isCommanding)
        {
            player.animator.SetTrigger("finishedCommand");
            player.animator.speed = 0;
            wasCommanding = false;
        }

        if (player.isFrozen)
        {
            if (Time.time > frozenTime + breakFreezeLength)
            {
                player.Defrost();
            }

            if (player.isDead)
            {
                player.Defrost();
            }
        }

        if (!player.isParalyzed && !player.isBurned && !player.isPoisoned && !player.isFrozen
            && currentStatusEffect != StatusEffect.None)
        {
            currentStatusEffect = StatusEffect.None;
        }
    }

    public void SetAndStartEffect(StatusEffect statusEffect)
    {
        if (currentStatusEffect == StatusEffect.None)
        {
            currentStatusEffect = statusEffect;

            if (statusEffect == StatusEffect.Poisoned)
            {
                GameObject poison = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/NegativeStatusEffect"));
                poison.GetComponent<NegativeStatusController>().PoisonTarget(player, null);
            }
            else if (statusEffect == StatusEffect.Burned)
            {
                GameObject burn = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/NegativeStatusEffect"));
                burn.GetComponent<NegativeStatusController>().BurnTarget(player, null);
            }
            else if (statusEffect == StatusEffect.Frozen)
            {
                player.Freeze();
                frozenTime = Time.time;

                if (!player.isCommanding)
                {
                    player.animator.speed = 0;
                }
                else
                {
                    wasCommanding = true;
                }
            }
            else if (statusEffect == StatusEffect.Paralyzed)
            {
                GameObject paralyze = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/Paralyze"));
                paralyze.GetComponent<ParalysisController>().paralyzedTarget = player.gameObject;

                if (!player.isCommanding)
                {
                    player.animator.speed = 0;
                }
                else
                {
                    wasCommanding = true;
                }
            }
        }
    }

    public enum StatusEffect
    {
        None,
        Poisoned,
        Burned,
        Frozen,
        Paralyzed
    }
}
