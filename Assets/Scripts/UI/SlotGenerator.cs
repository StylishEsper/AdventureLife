using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using static Item;
using UnityEngine;
using UnityEngine.UI;

public class SlotGenerator : MonoBehaviour
{
    public PlayerController player;
    public GameObject singleSlot;
    public GameObject inventory;
    public HeldItem currentSelection;
    public HeldItem quickCastSlot1;
    public HeldItem quickCastSlot2;
    public HeldItem quickCastSlot3;
    public HeldItem quickCastSlot4;
    public SlotSelectionCheck lifeCrystal;
    public SlotSelectionCheck forceCrystal;
    public SlotSelectionCheck magicCrystal;
    public SlotSelectionCheck luckCrystal;
    public List<GameObject> slots = new List<GameObject>();

    public int numberOfSlots;

    private GameObject slotOptions;
    private Button useButton;
    private Button dropButton;
    private Slider dropSlider;
    private RectTransform rect;
    private HeldItem previousSelection;
    private HeldItem prevQuickSelection;
    private GameObject confirm;


    private int leftIndent;
    private int topIndent;
    private int columnCount;
    private int uniqueItems;

    public void Start()
    {
        slotOptions = GameObject.Find("Player").transform.GetChild(0).
            transform.GetChild(2).transform.GetChild(0).transform.GetChild(4).gameObject;

        useButton = slotOptions.transform.GetChild(0).GetComponent<Button>();
        dropButton = slotOptions.transform.GetChild(1).GetComponent<Button>();
        dropSlider = slotOptions.transform.GetChild(2).GetComponent<Slider>();

        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 100);

        leftIndent = 58;
        topIndent = 58;
        columnCount = 0;

        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }

        slots.Clear();

        uniqueItems = FindSlotsRequired();

        if (numberOfSlots == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < numberOfSlots; i++)
            {
                slots.Add(Instantiate(singleSlot));
                slots[i].transform.SetParent(gameObject.transform);

                var heldItem = slots[i].transform.GetChild(0).GetComponent<HeldItem>();
                heldItem.slotNumber = i + 1;
                heldItem.Start();

                columnCount++;

                if (columnCount == 7)
                {
                    columnCount = 1;
                    leftIndent = 58;
                    topIndent += 118;
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + 125);
                }

                slots[i].transform.position = new Vector3(transform.position.x + leftIndent, transform.position.y - topIndent);
                leftIndent += 118;
            }
        }

        FillSlots();
    }

    public void Update()
    {
        List<HeldItem> heldItems = new List<HeldItem>();
        heldItems.Add(quickCastSlot1);
        heldItems.Add(quickCastSlot2);
        heldItems.Add(quickCastSlot3);
        heldItems.Add(quickCastSlot4);

        //Keep updating quick slots when inventory is active
        if (inventory.activeInHierarchy)
        {
            foreach (HeldItem h in heldItems)
            {
                if (h.item.slotNumber != 0)
                {
                    var temp = slots[h.item.slotNumber - 1].transform.GetChild(0).GetComponent<HeldItem>();

                    if (h != temp)
                    {
                        h.Copy(temp);
                    }

                    if (h.item.itemName == ItemName.None)
                    {
                        h.RemoveEntirely();
                    }
                }
            }
        }

        //Placing from inventory slot to quick slot
        if (currentSelection != null && currentSelection.item.usable)
        {
            int inc = 1;
            foreach (HeldItem h in heldItems)
            {
                if (h.isSelected)
                {
                    h.item = currentSelection.item;
                    h.currentStack = currentSelection.currentStack;
                    h.item.inQuickSlot = true;
                    h.item.quickSlotNumber = inc;
                    h.ReloadSlot();
                    break;
                }
                inc++;
            }
        }
        //For crystals
        else if (currentSelection != null && currentSelection.item.itemName == ItemName.MysteriousCrystal && 
            confirm == null || currentSelection != null && currentSelection.item.itemName == ItemName.CrystalShard &&
            confirm == null)
        {
            SlotSelectionCheck temp = null;

            if (lifeCrystal.selected || forceCrystal.selected || magicCrystal.selected || luckCrystal.selected)
            {
                if (lifeCrystal.selected)
                {
                    temp = lifeCrystal;
                }
                else if (forceCrystal.selected)
                {
                    temp = forceCrystal;
                }
                else if (magicCrystal.selected)
                {
                    temp = magicCrystal;
                }
                else if (luckCrystal.selected)
                {
                    temp = luckCrystal;
                }

                PowerCrystal crystal = temp.transform.GetChild(0).GetComponent<PowerCrystal>();

                if (currentSelection.item.itemName == ItemName.MysteriousCrystal)
                {
                    if (!crystal.crystalOn)
                    {
                        confirm = (GameObject)Instantiate(Resources.Load("Prefabs/UI/ConfirmationWindow"));
                        confirm.GetComponent<ConfirmAction>().SetReasonEquip(crystal, currentSelection.item);
                    }
                }
                else
                {
                    if (crystal.crystalOn)
                    {
                        if (!crystal.CheckIfMaxed())
                        {
                            confirm = (GameObject)Instantiate(Resources.Load("Prefabs/UI/ConfirmationWindow"));
                            confirm.GetComponent<ConfirmAction>().SetReasonUpgrade(crystal, currentSelection.item);
                        }
                    }
                }
            }
        }

        //Switching between quick slots
        if (quickCastSlot1.isSelected || quickCastSlot2.isSelected || quickCastSlot3.isSelected ||
            quickCastSlot4.isSelected)
        {
            HeldItem currQuickSelection = null;

            if (quickCastSlot1.isSelected)
            {
                currQuickSelection = quickCastSlot1;
            }
            else if (quickCastSlot2.isSelected)
            {
                currQuickSelection = quickCastSlot2;
            }
            else if (quickCastSlot3.isSelected)
            {
                currQuickSelection = quickCastSlot3;
            }
            else if (quickCastSlot4.isSelected)
            {
                currQuickSelection = quickCastSlot4;
            }

            var tempItem = currQuickSelection.item;
            var tempStack = currQuickSelection.currentStack;

            if (currQuickSelection != prevQuickSelection && prevQuickSelection != null && 
                prevQuickSelection.item.itemName != ItemName.None)
            {
                currQuickSelection.item = prevQuickSelection.item;
                currQuickSelection.currentStack = prevQuickSelection.currentStack;
                currQuickSelection.ReloadSlot();
                prevQuickSelection.item = tempItem;
                prevQuickSelection.currentStack = tempStack;
                prevQuickSelection.ReloadSlot();
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }

            prevQuickSelection = currQuickSelection;
        }
        else
        {
            prevQuickSelection = null;
        }

        int i = 1;

        //Activating or deactivating slot options, also handles switching inventory slots
        foreach (GameObject slot in slots)
        {
            HeldItem s = slot.transform.GetChild(0).GetComponent<HeldItem>();

            if (inventory.activeInHierarchy)
            {
                if (s.isSelected)
                {
                    currentSelection = s;
                    slotOptions.SetActive(true);

                    if (!s.IsHoldingItem() || s.item.isPermanent)
                    {
                        useButton.interactable = false;
                        dropButton.interactable = false;
                        dropSlider.interactable = false;
                    }
                    else if (!s.item.usable)
                    {
                        useButton.interactable = false;

                        Slider slider = dropSlider.GetComponent<Slider>();
                        slider.maxValue = s.currentStack;
                    }
                    else
                    {
                        useButton.interactable = true;
                        dropButton.interactable = true;
                        dropSlider.interactable = true;

                        Slider slider = dropSlider.GetComponent<Slider>();
                        slider.maxValue = s.currentStack;
                    }

                    if (currentSelection != previousSelection && currentSelection != null &&
                        previousSelection != null && previousSelection.item.itemName != ItemName.None)
                    {
                        int prev = previousSelection.item.slotNumber;
                        int prevQuick = previousSelection.item.quickSlotNumber;
                        bool prevIn = previousSelection.item.inQuickSlot;
                        int curr = currentSelection.item.slotNumber;

                        if (curr == 0)
                        {
                            curr = currentSelection.slotNumber;
                        }

                        int currQuick = currentSelection.item.quickSlotNumber;
                        bool currIn = currentSelection.item.inQuickSlot;

                        foreach (Item item in player.obtainedItems)
                        {
                            if (item.slotNumber == prev || item.slotNumber == curr)
                            {
                                item.changeRequested = true;

                                if (item.slotNumber == prev)
                                {
                                    item.quickSlotNumber = 0;
                                }
                            }
                        }

                        int changedAmount = 1;

                        foreach (Item item in player.obtainedItems)
                        {
                            if (previousSelection.item.itemName != currentSelection.item.itemName)
                            {
                                if (item.slotNumber == prev && item.changeRequested)
                                {
                                    item.slotNumber = curr;
                                    item.quickSlotNumber = currQuick;
                                    item.inQuickSlot = currIn;                             
                                    item.changeRequested = false;
                                }
                                else if (item.slotNumber == curr && item.changeRequested)
                                {
                                    item.slotNumber = prev;
                                    item.quickSlotNumber = prevQuick;
                                    item.inQuickSlot = prevIn;
                                    item.changeRequested = false;
                                }               
                            }
                            else
                            {
                                if (!currentSelection.IsSlotFull() && !currentSelection.WillBeOverMax(changedAmount))
                                {
                                    if (item.slotNumber == prev && item.changeRequested)
                                    {
                                        changedAmount++;
                                        item.slotNumber = curr;
                                        item.slotNumber = currQuick;
                                        item.inQuickSlot = currIn;
                                        item.changeRequested = false;
                                    }
                                }
                                else
                                {       
                                    if (item.slotNumber == prev && item.changeRequested)
                                    {
                                        item.changeRequested = false;
                                    }
                                    else if (item.slotNumber == curr && item.changeRequested)
                                    {
                                        item.changeRequested = false;
                                    }
                                }
                            }
                        }

                        Start();
                    }

                    previousSelection = s;

                    break;
                }

                if (i == slots.Count)
                {
                    slotOptions.SetActive(false);
                    currentSelection = null;
                    previousSelection = null;
                }

                i++;
            }
        }
    }

    public int FindSlotsRequired()
    {
        const int maxStack = 16;
        int numberOfHealthPots = 0;
        int numberOfAntidotes = 0;
        int numberOfExtinguish = 0;
        int numberOfDefrost = 0;
        int numberOfUnparalyze = 0;
        int numberOfHerb = 0;
        int numberOfUnique = 0;
        bool uniqueHealthPotion = true;
        bool uniqueAntidote = true;
        bool uniqueExtinguish = true;
        bool uniqueDefrost = true;
        bool uniqueUnparalyze = true;
        bool uniqueHerb = true;

        foreach (Item item in player.obtainedItems)
        {
            if (item.itemName == ItemName.HealthPotion)
            {
                numberOfHealthPots++;

                if (uniqueHealthPotion)
                {
                    numberOfUnique++;
                    uniqueHealthPotion = false;
                }

                if (numberOfHealthPots == maxStack)
                {
                    numberOfHealthPots = 0;
                    uniqueHealthPotion = true;
                }
            }
            else if (item.itemName == ItemName.Antidote)
            {
                numberOfAntidotes++;

                if (uniqueAntidote)
                {
                    numberOfUnique++;
                    uniqueAntidote = false;
                }

                if (numberOfAntidotes == maxStack)
                {
                    numberOfAntidotes = 0;
                    uniqueAntidote = true;
                }
            }
            else if (item.itemName == ItemName.Extinguish)
            {
                numberOfExtinguish++;

                if (uniqueExtinguish)
                {
                    numberOfUnique++;
                    uniqueExtinguish = false;
                }

                if (numberOfExtinguish == maxStack)
                {
                    numberOfExtinguish = 0;
                    uniqueExtinguish = true;
                }
            }
            else if (item.itemName == ItemName.Defrost)
            {
                numberOfDefrost++;

                if (uniqueDefrost)
                {
                    numberOfUnique++;
                    uniqueDefrost = false;
                }

                if (numberOfDefrost == maxStack)
                {
                    numberOfDefrost = 0;
                    uniqueDefrost = true;
                }
            }
            else if (item.itemName == ItemName.Unparalyze)
            {
                numberOfUnparalyze++;

                if (uniqueUnparalyze)
                {
                    numberOfUnique++;
                    uniqueUnparalyze = false;
                }

                if (numberOfUnparalyze == maxStack)
                {
                    numberOfUnparalyze = 0;
                    uniqueUnparalyze = true;
                }
            }
            else if (item.itemName == ItemName.MysteriousCrystal ||
                item.itemName == ItemName.CrystalShard || 
                item.itemName == ItemName.Key)
            {
                numberOfUnique++;
            }
            else if (item.itemName == ItemName.Herb)
            {
                numberOfHerb++;

                if (uniqueHerb)
                {
                    numberOfUnique++;
                    uniqueHerb = false;
                }

                if (numberOfHerb == maxStack)
                {
                    numberOfHerb = 0;
                    uniqueHerb = true;
                }
            }
        }

        return numberOfUnique;
    }

    public bool GetIsInventoryFull(Item pickupAttempt)
    {
        bool isFull = false;
        const int maxStack = 16;
        int numberOfHealthPots = 0;
        int numberOfAntidotes = 0;
        int numberOfExtinguish = 0;
        int numberOfDefrost = 0;
        int numberOfUnparalyze = 0;
        int numberOfUnique = 0;
        int numberOfHerb = 0;
        bool uniqueHealthPotion = true;
        bool uniqueAntidote = true;
        bool uniqueExtinguish = true;
        bool uniqueDefrost = true;
        bool uniqueUnparalyze = true;
        bool uniqueHerb = true;

        if (pickupAttempt.itemName == ItemName.HealthPotion)
        {
            numberOfHealthPots++;
            numberOfUnique++;
            uniqueHealthPotion = false;
        }
        else if (pickupAttempt.itemName == ItemName.Antidote)
        {
            numberOfAntidotes++;
            numberOfUnique++;
            uniqueAntidote = false;
        }
        else if (pickupAttempt.itemName == ItemName.Extinguish)
        {
            numberOfExtinguish++;
            numberOfUnique++;
            uniqueExtinguish = false;
        }
        else if (pickupAttempt.itemName == ItemName.Defrost)
        {
            numberOfDefrost++;
            numberOfUnique++;
            uniqueDefrost = false;
        }
        else if (pickupAttempt.itemName == ItemName.Unparalyze)
        {
            numberOfUnparalyze++;
            numberOfUnique++;
            uniqueUnparalyze = false;
        }
        else if (pickupAttempt.itemName == ItemName.MysteriousCrystal ||
            pickupAttempt.itemName == ItemName.CrystalShard ||
            pickupAttempt.itemName == ItemName.Key)
        {
            numberOfUnique++;
        }
        else if (pickupAttempt.itemName == ItemName.Herb)
        {
            numberOfHerb++;
            numberOfUnique++;
            uniqueHerb = false;
        }

        foreach (Item item in player.obtainedItems)
        {
            if (item.itemName == ItemName.HealthPotion)
            {
                numberOfHealthPots++;

                if (uniqueHealthPotion)
                {
                    numberOfUnique++;
                    uniqueHealthPotion = false;
                }

                if (numberOfHealthPots == maxStack)
                {
                    numberOfHealthPots = 0;
                    uniqueHealthPotion = true;
                }
            }
            else if (item.itemName == ItemName.Antidote)
            {
                numberOfAntidotes++;

                if (uniqueAntidote)
                {
                    numberOfUnique++;
                    uniqueAntidote = false;
                }

                if (numberOfAntidotes == maxStack)
                {
                    numberOfAntidotes = 0;
                    uniqueAntidote = true;
                }
            }
            else if (item.itemName == ItemName.Extinguish)
            {
                numberOfExtinguish++;

                if (uniqueExtinguish)
                {
                    numberOfUnique++;
                    uniqueExtinguish = false;
                }

                if (numberOfExtinguish == maxStack)
                {
                    numberOfExtinguish = 0;
                    uniqueExtinguish = true;
                }
            }
            else if (item.itemName == ItemName.Defrost)
            {
                numberOfDefrost++;

                if (uniqueDefrost)
                {
                    numberOfUnique++;
                    uniqueDefrost = false;
                }

                if (numberOfDefrost == maxStack)
                {
                    numberOfDefrost = 0;
                    uniqueDefrost = true;
                }
            }
            else if (item.itemName == ItemName.Unparalyze)
            {
                numberOfUnparalyze++;

                if (uniqueUnparalyze)
                {
                    numberOfUnique++;
                    uniqueUnparalyze = false;
                }

                if (numberOfUnparalyze == maxStack)
                {
                    numberOfUnparalyze = 0;
                    uniqueUnparalyze = true;
                }
            }
            else if (item.itemName == ItemName.MysteriousCrystal ||
                item.itemName == ItemName.CrystalShard)
            {
                numberOfUnique++;
            }
            else if (item.itemName == ItemName.Herb)
            {
                numberOfHerb++;

                if (uniqueHerb)
                {
                    numberOfUnique++;
                    uniqueHerb = false;
                }

                if (numberOfHerb == maxStack)
                {
                    numberOfHerb = 0;
                    uniqueHerb = true;
                }
            }
        }

        if (numberOfUnique >= numberOfSlots + 1)
        {
            isFull = true;
        }

        return isFull;
    }

    public void FillSlots()
    {
        foreach (Item item in player.obtainedItems)
        {
            if (item.visibleInInventory)
            {
                HeldItem heldItem = null;

                foreach (GameObject slot in slots)
                {
                    heldItem = slot.transform.GetChild(0).GetComponent<HeldItem>();

                    if (heldItem.item.itemName == item.itemName && item.slotNumber == 0)
                    {
                        if (!heldItem.IsSlotFull())
                        {
                            break;
                        }
                    }
                    else if (heldItem.item.itemName == item.itemName && item.slotNumber == heldItem.slotNumber)
                    {
                        if (!heldItem.IsSlotFull())
                        {
                            break;
                        }
                    }

                    heldItem = null;
                }

                if (heldItem == null)
                {
                    foreach (GameObject slot in slots)
                    {
                        heldItem = slot.transform.GetChild(0).GetComponent<HeldItem>();

                        if (heldItem.item.itemName == ItemName.None && item.slotNumber == 0)
                        {
                            break;
                        }
                        else if (heldItem.item.itemName == ItemName.None && item.slotNumber == heldItem.slotNumber)
                        {
                            break;
                        }
                    }
                }

                heldItem.AddItem(item);
            }
        }
    }

    public void InventoryExpansion()
    {
        if (uniqueItems > numberOfSlots)
        {
            numberOfSlots = uniqueItems;
        }
    }

    public void DeselectAll()
    {
        foreach (GameObject slot in slots)
        {
            HeldItem s = slot.transform.GetChild(0).GetComponent<HeldItem>();
            s.isSelected = false;
            slotOptions.SetActive(false);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
