using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private GameObject listHolder;
    private GameObject currentSpawn;

    [SerializeField] private float setRightTravel;
    [SerializeField] private float setLeftTravel;
    [SerializeField] private SpawnableEnemy enemy;
    [SerializeField] private Condition condition;

    [SerializeField] private int spawnAmount;

    [SerializeField] private bool addToList;
    [SerializeField] private bool useSetTravel;

    private void Start()
    {
        if (target == null || spawnAmount == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (currentSpawn == null)
        {
            if (condition == Condition.TargetXReached)
            {
                if (target.transform.position.x >= transform.position.x)
                {
                    SpawnEnemy();
                }
            }
            else if (condition == Condition.TargetYReached)
            {
                if (target.transform.position.y >= transform.position.y)
                {
                    SpawnEnemy();
                }
            }
        }
        else if (currentSpawn != null)
        {
            if (currentSpawn.GetComponent<EnemyVerification>().isDead)
            {
                SpawnEnemy();
            }
        }
    }

    public void SpawnEnemy()
    {
        GameObject se = null;

        if (enemy == SpawnableEnemy.BlueSlime)
        {
            se = (GameObject)Instantiate(Resources.Load("Prefabs/Monsters/BlueSlime"));
        }
        else if (enemy == SpawnableEnemy.BarrelDude)
        {
            se = (GameObject)Instantiate(Resources.Load("Prefabs/Monsters/BarrelDude"));
        }
        else if (enemy == SpawnableEnemy.BattyBat)
        {
            se = (GameObject)Instantiate(Resources.Load("Prefabs/Monsters/BattyBat"));
        }
        else if (enemy == SpawnableEnemy.Skullime)
        {
            se = (GameObject)Instantiate(Resources.Load("Prefabs/Monsters/Skullime"));
        }
        else if (enemy == SpawnableEnemy.Netcaster)
        {
            se = (GameObject)Instantiate(Resources.Load("Prefabs/Monsters/Netcaster"));

            if (useSetTravel)
            {
                var sec = se.GetComponent<SmartEnemyController>();
                sec.rightTravel = setRightTravel;
                sec.leftTravel = setLeftTravel;
                sec.ResetEnemy();
            }
        }

        se.transform.position = spawnLocation.transform.position;

        if (addToList)
        {
            listHolder.GetComponent<LockedDoor>().enemies.Add(se);
        }

        currentSpawn = se;

        spawnAmount--;
        if (spawnAmount == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (condition == Condition.OnCollision)
        {
            SpawnEnemy();
        }
    }

    public enum Condition
    {
        None,
        TargetXReached,
        TargetYReached,
        OnCollision
    }

    public enum SpawnableEnemy
    {
        None,
        BlueSlime,
        BarrelDude,
        BattyBat,
        Skullime,
        Netcaster
    }
}
