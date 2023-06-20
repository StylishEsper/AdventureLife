using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using static Objective;
using UnityEngine;

public class ObjectiveCreator : MonoBehaviour
{
    public Objective.Name objectiveName;

    public int maxCount;

    public bool requiresCount;

    private PlayerController player;

    private bool created;

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        PlayerData data = SaveSystem.LoadGame(player.loadedSave);

        created = false;

        foreach (Objective objective in data.objectives)
        {
            if (objective.name == objectiveName)
            {
                created = true;
            }
        }
    }

    public void SetObjective()
    {
        Objective objective = new Objective(objectiveName, requiresCount, 0, maxCount);
        player.objectives.Add(objective);
        player.objectivesControl.Populate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!created && collision.tag == "Player")
        {
            SetObjective();
            created = true;
        }
    }
}

