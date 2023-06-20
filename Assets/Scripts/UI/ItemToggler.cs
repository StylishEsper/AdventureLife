using System.Collections;
using System.Collections.Generic;
using static Item;
using UnityEngine;
using Platformer.Mechanics;

public class ItemToggler : MonoBehaviour
{
    public PlayerController player;
    public HeldItem currentItem;
    public HeldItem quickItem1;
    public HeldItem quickItem2;
    public HeldItem quickItem3;
    public HeldItem quickItem4;
    public GameObject inventory;

    [SerializeField] private GameObject infoWindow;
    [SerializeField] private GameObject pauseMenu;

    private const int maxToggle = 4;
    private const int minToggle = 1;

    private int toggle;

    private bool switchItem;

    private void Start()
    {
        quickItem1.Start();
        quickItem2.Start();
        quickItem3.Start();
        quickItem4.Start();
        toggle = 1;
        switchItem = true;
    }

    private void Update()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("UI Canvas");   
        }

        if (Input.GetKeyDown(KeyCode.E) && !infoWindow.activeInHierarchy && !pauseMenu.activeInHierarchy)
        {
            toggle++;

            if (toggle == maxToggle + 1)
            {
                toggle = minToggle;
            }

            switchItem = true;
        }

        if (switchItem || inventory.activeInHierarchy)
        {
            if (toggle == 1)
            {
                currentItem.item = quickItem1.item;
                currentItem.currentStack = quickItem1.currentStack;
            }
            else if (toggle == 2)
            {
                currentItem.item = quickItem2.item;
                currentItem.currentStack = quickItem2.currentStack;
            }
            else if (toggle == 3)
            {
                currentItem.item = quickItem3.item;
                currentItem.currentStack = quickItem3.currentStack;
            }
            else if (toggle == 4)
            {
                currentItem.item = quickItem4.item;
                currentItem.currentStack = quickItem4.currentStack;
            }
         
            currentItem.ReloadSlot();
            switchItem = false;
        }

        if (!inventory.activeInHierarchy && Input.GetKeyDown(KeyCode.F))
        {
            if (currentItem.item.slotNumber != 0)
            {
                currentItem.currentStack = player.slotGenerator.slots[currentItem.item.slotNumber - 1].transform.GetChild(0).
                    GetComponent<HeldItem>().currentStack;
                currentItem.ReloadSlot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentItem.item.itemName != ItemName.None
            && !player.isDead && !infoWindow.activeInHierarchy && !pauseMenu.activeInHierarchy)
        {
            currentItem.UseItem();
            currentItem.ReloadSlot();

            if (toggle == 1 || currentItem.item.slotNumber == quickItem1.item.slotNumber)
            {
                quickItem1.DropItem(1);
            }

            if (toggle == 2 || currentItem.item.slotNumber == quickItem2.item.slotNumber)
            {
                quickItem2.DropItem(1);
            }

            if (toggle == 3 || currentItem.item.slotNumber == quickItem3.item.slotNumber)
            {
                quickItem3.DropItem(1);
            }

            if (toggle == 4 || currentItem.item.slotNumber == quickItem4.item.slotNumber)
            {
                quickItem4.DropItem(1);
            }
        }
    }

    public void DropFromAllRelated(int slotNumber, int amount)
    {
        if (quickItem1.item.slotNumber == slotNumber)
        {
            quickItem1.DropItem(amount);
        }

        if (quickItem2.item.slotNumber == slotNumber)
        {
            quickItem2.DropItem(amount);
        }

        if (quickItem3.item.slotNumber == slotNumber)
        {
            quickItem3.DropItem(amount);
        }

        if (quickItem4.item.slotNumber == slotNumber)
        {
            quickItem4.DropItem(amount);
        }
    }
}
