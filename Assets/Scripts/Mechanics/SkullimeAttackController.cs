using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullimeAttackController : MonoBehaviour
{
    public GameObject bullet;
    public ParticleSystem chargeEffect;

    public float chargeDelay;
    public float chargeLength;

    private SimpleEnemyController skullime;

    private float startTime;
    private float chargeTime;
    private float fireTime;

    private bool chargeStarted;
    private bool attackFired;

    void Start()
    {
        skullime = GetComponent<SimpleEnemyController>();
        startTime = Time.time;
        chargeStarted = false;
        attackFired = false;
    }


    void Update()
    {
        if (!skullime.isDead && !skullime.isFrozen && !skullime.isParalyzed && skullime.agro)
        {
            if (Time.time >= startTime + chargeDelay)
            {
                skullime.standStill = true;
                skullime.isAttacking = true;
                skullime.control.move.x = 0;

                if (skullime.player.transform.position.x >= transform.position.x)
                {
                    skullime.spriteRenderer.flipX = true;
                    chargeEffect.transform.position = new Vector3(transform.position.x + 0.275f, 
                        transform.position.y - 0.245f);
                }
                else
                {
                    skullime.spriteRenderer.flipX = false;
                    chargeEffect.transform.position = new Vector3(transform.position.x - 0.275f,
                        transform.position.y - 0.245f);
                }

                if (!chargeStarted)
                {
                    chargeEffect.Play();
                    skullime.control.animator.SetTrigger("charging");
                    chargeTime = Time.time;
                    chargeStarted = true;
                }

                if (!attackFired && Time.time >= chargeTime + chargeLength)
                {
                    chargeEffect.Clear();
                    chargeEffect.Stop();
                    skullime.control.animator.SetBool("attacking", true);
                    Attack();
                    attackFired = true;
                    fireTime = Time.time;
                }

                if (attackFired && Time.time >= fireTime + 0.834f)
                {
                    skullime.control.animator.SetBool("attacking", false);
                    chargeStarted = false;
                    attackFired = false;
                    skullime.isAttacking = false;
                    skullime.standStill = false;
                    startTime = Time.time;
                }
            }
        }
    }

    public void Attack()
    {
        GameObject bullet = this.bullet;

        if (skullime.spriteRenderer.flipX)
        {
            bullet.transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y - 0.25f);
        }
        else
        {
            bullet.transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y - 0.25f);
        }

        bullet.GetComponent<BulletController>().target = skullime.player.transform;
        Instantiate(this.bullet);
    }
}
