using Platformer.Mechanics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enterable : MonoBehaviour
{
    public Canvas canvas;

    public Vector3 position;

    public string scene;

    public bool autoEnter;

    private PlayerController player;

    private bool canEnter;

    private void Start()
    {
        if (!autoEnter)
        {
            canvas.enabled = false;
        }
    }

    private void Update()
    {
        if (canEnter)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Save();
                SceneManager.LoadScene(scene);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (autoEnter)
            {
                Save();
                SceneManager.LoadScene(scene);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !autoEnter)
        {
            canvas.enabled = true;
            canEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !autoEnter)
        {
            canvas.enabled = false;
            canEnter = false;
        }
    }

    public void Save()
    {
        var s = GameObject.Find("SessionData");
        if (s == null) return;
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        List<Item.ItemName> itemNames = new List<Item.ItemName>();
        List<int> slotNumbers = new List<int>();
        int[] quickSlotNumbers = new int[player.obtainedItems.Count];
        bool[] inQuickSlots = new bool[player.obtainedItems.Count];
        int i = 0;

        foreach (Item item in player.obtainedItems)
        {
            itemNames.Add(item.itemName);
            slotNumbers.Add(item.slotNumber);

            if (item.inQuickSlot)
            {
                quickSlotNumbers[i] = item.quickSlotNumber;
                inQuickSlots[i] = item.inQuickSlot;
            }

            i++;
        }

        float timePlayed = s.GetComponent<SessionData>().timePlayed;

        SaveSystem.SaveGame(itemNames, slotNumbers, player.objectives, player.worldItems, player.playedCutscenes, player.memorableEvents,
            player.health.GetHealth(), player.energy.GetEnergy(), player.loadedSave, quickSlotNumbers, player.crystalUpgrades,
            timePlayed, position, scene, player.currentElement, inQuickSlots, player.equippedCrystals);
    }
}
