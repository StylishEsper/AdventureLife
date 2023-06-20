using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventory;
    public PlayerController player;
    public SlotGenerator slotGenerator;
    public GameController game;
    public GameObject infoWindow;

    public bool disabled;

    private void Start()
    {
        inventory.SetActive(false);
    }

    private void Update()
    {
        if (!disabled)
        {
            if (!player.isKnockedBack && !player.isDead && !infoWindow.activeInHierarchy)
            {
                if (inventory.activeInHierarchy)
                {
                    game.SetCursorVisibility(true);
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        slotGenerator.DeselectAll();
                        inventory.SetActive(false);
                        player.controlEnabled = true;
                        game.SetCursorVisibility(false);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        inventory.SetActive(true);
                        player.controlEnabled = false;
                        game.SetCursorVisibility(true);
                    }
                }
            }

            if (player.isKnockedBack && inventory.activeInHierarchy ||
                infoWindow.activeInHierarchy && inventory.activeInHierarchy)
            {
                inventory.SetActive(false);
            }
        }
        else
        {
            if (inventory.activeInHierarchy)
            {
                inventory.SetActive(false);
            }
        }
    }
}
