using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVerification : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sprite;
    public ParticleSystem breakFreeze;
    public Image agroIcon;

    public Behavior behavior;
    public EntityElement.Element element;

    public float frozenTime;

    public bool isDead;
    public bool invincible;
    public bool isBurned;
    public bool isParalyzed;
    public bool isFrozen;
    public bool isPoisoned;

    private SimpleEnemyController simpleEnemyController;
    private FlyingEnemyController flyingEnemyController;
    private SmartEnemyController smartEnemyController;
    private ArachnaController arachnaController;

    private bool agroGained;
    private bool isBoss;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        if (behavior == Behavior.Simple)
        {
            simpleEnemyController = GetComponent<SimpleEnemyController>();
        }
        else if (behavior == Behavior.Flying)
        {
            flyingEnemyController = GetComponent<FlyingEnemyController>();
        }
        else if (behavior == Behavior.Smart)
        {
            smartEnemyController = GetComponent<SmartEnemyController>();
        }
        else if (behavior == Behavior.Arachna)
        {
            arachnaController = GetComponent<ArachnaController>();
            isBoss = true;
        }
    }

    private void Update()
    {
        if (behavior == Behavior.Simple)
        {
            isDead = simpleEnemyController.isDead;
            simpleEnemyController.invincible = invincible;
            agroGained = simpleEnemyController.agro;
        }
        else if (behavior == Behavior.Flying)
        {
            isDead = flyingEnemyController.isDead;
            flyingEnemyController.invincible = invincible;
            agroGained = flyingEnemyController.agro;
        }
        else if (behavior == Behavior.Smart)
        {
            isDead = smartEnemyController.isDead;
            smartEnemyController.invincible = invincible;
            agroGained = smartEnemyController.agro;
        }
        else if (behavior == Behavior.Arachna)
        {
            arachnaController.invincible = invincible;
        }

        if (!isBoss)
        {
            ShowAgroIcon();
        }

        if (isParalyzed || isFrozen)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }

    public void ShowAgroIcon()
    {
        if (agroGained && !agroIcon.enabled)
        {
            agroIcon.enabled = true;
        }
        else if (!agroGained && agroIcon.enabled)
        {
            agroIcon.enabled = false;
        }
    }

    public void Hurt(bool isOvertime)
    {
        if (behavior == Behavior.Simple)
        {
            simpleEnemyController.Hurt(isOvertime);
        }
        else if (behavior == Behavior.Flying)
        {
            flyingEnemyController.Hurt(isOvertime);
        }
        else if (behavior == Behavior.Smart)
        {
            smartEnemyController.Hurt(isOvertime);
        }
        else if (behavior == Behavior.Arachna)
        {
            arachnaController.Hurt(isOvertime);
        }
    }

    public void Harmless()
    {
        if (behavior == Behavior.Simple)
        {
            simpleEnemyController.Harmless();
        }
        else if (behavior == Behavior.Flying)
        {
            flyingEnemyController.Harmless();
        }
        else if (behavior == Behavior.Smart)
        {
            smartEnemyController.Harmless();
        }
        else if (behavior == Behavior.Arachna)
        {

        }
    }

    public void Freeze()
    {
        isFrozen = true;

        if (behavior == Behavior.Simple)
        {
            simpleEnemyController.frozenTime = Time.time;
            simpleEnemyController.Freeze();
        }
        else if (behavior == Behavior.Flying)
        {
            flyingEnemyController.frozenTime = Time.time;
            flyingEnemyController.Freeze();
        }
        else if (behavior == Behavior.Smart)
        {
            smartEnemyController.Freeze();
        }
        else if (behavior == Behavior.Arachna)
        {

        }
    }

    public void Defrost()
    {
        isFrozen = false;
        breakFreeze.Play();

        if (behavior == Behavior.Simple)
        {
            simpleEnemyController.Defrost();
        }
        else if (behavior == Behavior.Flying)
        {
            flyingEnemyController.Defrost();
        }
        else if (behavior == Behavior.Smart)
        {
            smartEnemyController.Defrost();
        }
        else if (behavior == Behavior.Arachna)
        {

        }
    }

    public void Death()
    {
        if (behavior == Behavior.Simple)
        {
            simpleEnemyController.GetComponent<AnimationController>().animator.SetTrigger("hurt");
            simpleEnemyController.GetComponent<AnimationController>().animator.SetBool("dead", true);
        }
        else if (behavior == Behavior.Flying)
        {
            flyingEnemyController.animator.SetTrigger("death");
        }
        else if (behavior == Behavior.Smart)
        {
            smartEnemyController.GetComponent<AnimationController>().animator.SetTrigger("hurt");
            smartEnemyController.GetComponent<AnimationController>().animator.SetBool("dead", true);
        }
        else if (behavior == Behavior.Arachna)
        {
            
        }
    }

    public enum Behavior
    {
        None,
        Simple,
        Flying,
        Smart,
        Arachna
    }
}
