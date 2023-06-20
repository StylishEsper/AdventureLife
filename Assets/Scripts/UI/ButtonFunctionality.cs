using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using static Item;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctionality : MonoBehaviour
{
    public PlayerController player;
    public Slider slider;
    public SlotGenerator slotGenerator;
    public HeldItem heldItem;
    public Button selfButton;
    public ItemToggler itemToggler;

    public ButtonType buttonType;

    private void Update()
    {
        if (buttonType == ButtonType.QuickSlotItemRemove)
        {
            if (heldItem.item.itemName == ItemName.None)
            {
                selfButton.enabled = false;
            }
            else
            {
                selfButton.enabled = true;
            }
        }
    }

    public void RunFunction()
    {
        if (heldItem == null)
        {
            heldItem = slotGenerator.currentSelection;
        }

        if (buttonType == ButtonType.UseItem)
        {
            heldItem.UseItem();
        }
        else if (buttonType == ButtonType.DropItem)
        {
            int amount = (int)slider.value;
            GameObject confirm = (GameObject)Instantiate(Resources.Load("Prefabs/UI/ConfirmationWindow"));
            confirm.GetComponent<ConfirmAction>().SetReasonDrop(amount, player, heldItem, itemToggler);
        }
        else if (buttonType == ButtonType.QuickSlotItemRemove)
        {
            heldItem.RemoveEntirely();
        }
    }

    public enum ButtonType
    {
        None,
        UseItem,
        DropItem,
        QuickSlotItemRemove
    }
}
