using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> spawnableItems = new List<GameObject>();

    public bool isObject = false;

    private EnemyVerification enemy;

    private bool canSpawn;

    private void Start()
    {
        if (!isObject)
        {
            enemy = GetComponent<EnemyVerification>();     
        }

        canSpawn = true;
    }

    private void Update()
    {
        if (!isObject)
        {
            if (enemy.isDead && canSpawn)
            {
                SpawnItem();
            }
            else if (!enemy.isDead)
            {
                canSpawn = true;
            }
        }
    }

    public void SpawnItem()
    {
        if (!isObject)
        {
            canSpawn = false;

            int number = Random.Range(0, 101);

            if (number <= spawnableItems.Count - 1)
            {
                GameObject spawnedItem = Instantiate(spawnableItems[number]);
                spawnedItem.transform.position = transform.position;
            }
        }
        else
        {
            canSpawn = false;

            int number = Random.Range(0, spawnableItems.Count);

            GameObject spawnedItem = Instantiate(spawnableItems[number]);
            spawnedItem.transform.position = transform.position;
        }
    }
}
