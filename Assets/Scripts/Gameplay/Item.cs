using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public PlayerController player;

    public ItemName itemName;

    public int slotNumber;
    public int quickSlotNumber;

    public bool changeRequested;
    public bool usable;
    public bool isPermanent = false;
    public bool visibleInInventory = true;
    public bool inQuickSlot = false;

    public Item(ItemName itemName, int slotNumber)
    {
        this.itemName = itemName;
        this.slotNumber = slotNumber;
        changeRequested = false;

        if (itemName == ItemName.MysteriousCrystal || itemName == ItemName.CrystalShard ||
            itemName == ItemName.Key)
        {
            usable = false;

            if (itemName == ItemName.MysteriousCrystal)
            {
                isPermanent = true;
            }
        }
        else if (itemName == ItemName.WayOfTheSpider || itemName == ItemName.SkillOfASwordsman ||
            itemName == ItemName.FlowOfTheRiver || itemName == ItemName.RageOfTheOcean || 
            itemName == ItemName.BreathOfADragon || itemName == ItemName.HellsMostWanted ||
            itemName == ItemName.RoarOfThunder || itemName == ItemName.GiftFromTheStorm ||
            itemName == ItemName.PinnacleOfWinter || itemName == ItemName.TheIceKing)
        {
            usable = false;
            isPermanent = true;
            visibleInInventory = false;
            this.slotNumber = 0;
        }
        else
        {
            usable = true;
        }
    }

    public void UseItem()
    {
        if (itemName == ItemName.HealthPotion)
        {
            player.health.Increment(50, false, false);
            player.healthPotionEffect.Play();
        }
        else if (itemName == ItemName.Antidote)
        {
            if (player.isPoisoned)
            {
                player.isPoisoned = false;
            }
            player.antidoteEffect.Play();
        }
        else if (itemName == ItemName.Extinguish)
        {
            if (player.isBurned)
            {
                player.isBurned = false;
            }
            player.extinguishEffect.Play();
        }
        else if (itemName == ItemName.Defrost)
        {
            if (player.isFrozen)
            {
                player.Defrost();
            }
            player.defrostEffect.Play();
        }
        else if (itemName == ItemName.Unparalyze)
        {
            if (player.isParalyzed)
            {
                player.isParalyzed = false;
            }
            player.unparalyzeEffect.Play();
        }
        else if (itemName == ItemName.Herb)
        {
            System.Random r = new System.Random();
            int rInt = r.Next(10, 26);
            player.health.Increment(rInt, false, false);
            player.herbEffect.Play();
        }

        player.DropItems(1, slotNumber);
    }

    [SerializeField]
    public enum ItemName
    {
        None,
        HealthPotion,
        Antidote,
        Extinguish,
        Defrost,
        Unparalyze,
        MysteriousCrystal,
        CrystalShard,
        Herb,
        Key,
        WayOfTheSpider,
        SkillOfASwordsman,
        FlowOfTheRiver,
        RageOfTheOcean,
        BreathOfADragon,
        HellsMostWanted,
        RoarOfThunder,
        GiftFromTheStorm,
        PinnacleOfWinter,
        TheIceKing
    }
}
