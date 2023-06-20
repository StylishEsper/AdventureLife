using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmAction : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI explanation;
    public Button cancelButton;
    public Button confirmButton;
    public GameObject invisCanvas;

    public Reason reason;

    private Button invisButton;
    private PowerCrystal crystal;
    private Item item;
    private PlayerController player;
    private HeldItem heldItem;
    private ItemToggler toggler;
    private MainMenuController mainMenu;

    private int amount;

    private string path;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Confirm();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Cancel();
        }
    }

    public void SetReasonUpgrade(PowerCrystal crystal, Item item)
    {
        reason = Reason.UpgradeCrystal;
        this.crystal = crystal;
        this.item = item;
        title.text = "Confirm Upgrade";
        explanation.text = "Upgrading cannot be undone. Your crystal shard will be lost forever," +
            " are you sure you want to continue?";
        SetButtonsAndPosition();
    }

    public void SetReasonEquip(PowerCrystal crystal, Item item)
    {
        reason = Reason.EquipCrystal;
        this.crystal = crystal;
        this.item = item;
        title.text = "Confirm Equip";
        explanation.text = "Once a crystal is equipped, it can't be unequiped. Are you sure you" +
            " want to continue?";
        SetButtonsAndPosition();
    }

    public void SetReasonDrop(int amount, PlayerController player, HeldItem heldItem, ItemToggler toggler)
    {
        reason = Reason.DropItems;
        this.amount = amount;
        this.player = player;
        this.heldItem = heldItem;
        this.toggler = toggler;
        title.text = "Confirm Drop";
        explanation.text = "Dropped items are lost forever, are you sure you want to continue?";
        SetButtonsAndPosition();
    }

    public void SetReasonDeleteSave(string path, MainMenuController mainMenu)
    {
        reason = Reason.DeleteSave;
        this.path = path;
        this.mainMenu = mainMenu;
        title.text = "Confirm Delete";
        explanation.text = "Deleted saves cannot be restored. Are you sure you want to continue?";
        SetButtonsAndPosition();
    }

    public void SetButtonsAndPosition()
    {
        cancelButton.onClick.AddListener(Cancel);
        confirmButton.onClick.AddListener(Confirm);

        invisCanvas = Instantiate(invisCanvas);
        transform.SetParent(invisCanvas.transform);
        invisButton = invisCanvas.transform.GetChild(0).GetComponent<Button>();
        invisButton.onClick.AddListener(Cancel);


        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 0);
    }

    public void Confirm()
    {
        if (reason == Reason.UpgradeCrystal)
        {
            crystal.Upgrade();
            item.UseItem();
        }
        else if (reason == Reason.EquipCrystal)
        {
            crystal.EnableCrystal();
            item.UseItem();
        }
        else if (reason == Reason.DropItems)
        {
            player.DropItems(amount, heldItem.item.slotNumber);
            heldItem.DropItem(amount);
            toggler.DropFromAllRelated(heldItem.item.slotNumber, amount);
        }
        else if (reason == Reason.DeleteSave)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                mainMenu.ReloadSaves();
            }
        }

        Cancel();
    }

    public void Cancel()
    {
        Destroy(invisCanvas);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Cancel();
    }

    public enum Reason
    {
        None,
        EquipCrystal,
        UpgradeCrystal,
        DropItems,
        DeleteSave
    }
}
