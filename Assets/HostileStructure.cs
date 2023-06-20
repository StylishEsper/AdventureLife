using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileStructure : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Transform attackFireArea;
    private Animator attackAnimator;

    [SerializeField] private float basicCooldown = 5f;
    private float basicAttackTime;
    private float basicDelayTime;

    private bool basicCharged;

    private void Start()
    {
        attackFireArea = transform.GetChild(0).transform;
        attackAnimator = transform.GetChild(0).GetComponent<Animator>();
        basicAttackTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= basicAttackTime + basicCooldown)
        {
            attackAnimator.SetTrigger("charge");
            basicAttackTime = Time.time;
            basicCharged = true;
            basicDelayTime = Time.time;
        }

        if (basicCharged && Time.time >= basicDelayTime + 1.4f)
        {
            basicCharged = false;
            FireAttack();
        }
    }

    public void FireAttack()
    {
        GameObject poisonShot = (GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/PoisonShot"));
        poisonShot.transform.position = new Vector3(attackFireArea.position.x,
            attackFireArea.position.y - 0.3f, 0);
        poisonShot.GetComponent<BulletController>().target = target;
    }
}
