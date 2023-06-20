using System.Collections;
using System.Collections.Generic;
using static Item;
using static PowerCrystal;
using TMPro;
using UnityEngine;

public class ItemInformationSetter : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    public void SetInfo(ItemName item, CrystalType crystalType, bool isCrystal, bool equipped)
    {
        if (!isCrystal)
        {
            if (item == ItemName.HealthPotion)
            {
                itemName.SetText("Health Potion");
                itemDescription.SetText("Consumable; heals precisely 50 HP. Never more, never less. It's amazing " +
                    "how precise it really is.");
            }
            else if (item == ItemName.Antidote)
            {
                itemName.SetText("Antidote");
                itemDescription.SetText("Consumable; removes the poison status effect. Looks like poison too, but " +
                    "I think I'll take my chances when I'm dying from it.");
            }
            else if (item == ItemName.Extinguish)
            {
                itemName.SetText("Extinguish");
                itemDescription.SetText("Consumable; removes the burn status effect. Wait, do I drink this or " +
                    "pour it on myself when I'm engulfed in flames?");
            }
            else if (item == ItemName.Defrost)
            {
                itemName.SetText("Defrost");
                itemDescription.SetText("Consumable; removes the frozen status effect. How am I supposed to use this" +
                    " while I'm frozen?");
            }
            else if (item == ItemName.Unparalyze)
            {
                itemName.SetText("Unparalyze");
                itemDescription.SetText("Consumable; removes the paralysis status effect. What a shocker.");
            }
            else if (item == ItemName.MysteriousCrystal)
            {
                itemName.SetText("Mysterious Crystal of Unimaginable Power");
                itemDescription.SetText("Can be equipped. It's emanating a powerful aura that attracts all " +
                    "lifeforms. Whether that's a good thing or a bad thing, only one way to find out.");
            }
            else if (item == ItemName.CrystalShard)
            {
                itemName.SetText("Crystal Shard");
                itemDescription.SetText("Used to upgrade equipped crystals. It oddly acts like a magnet towards" +
                    " other crystals.");
            }
            else if (item == ItemName.Herb)
            {
                itemName.SetText("Herb");
                itemDescription.SetText("Consumable; heals anywhere from 10-25 HP.");
            }
            else if (item == ItemName.Key)
            {
                itemName.SetText("Mysterious Key");
                itemDescription.SetText("There's no way to tell what this key is meant for.");
            }
        }
        else
        {
            if (!equipped)
            {
                if (crystalType == CrystalType.Life)
                {
                    itemName.SetText("Life Slot");
                    itemDescription.SetText("Place a Mysterious Crystal here to gain +1 HP and +1 HP regen (regenerates every 5 seconds). ");
                }
                else if (crystalType == CrystalType.Force)
                {
                    itemName.SetText("Force Slot");
                    itemDescription.SetText("Place a Mysterious Crystal here to gain +1 basic attack DMG.");
                }
                else if (crystalType == CrystalType.Magic)
                {
                    itemName.SetText("Magic Slot");
                    itemDescription.SetText("Place a Mysterious Crystal here to gain +1 spell DMG.");
                }
                else if (crystalType == CrystalType.Luck)
                {
                    itemName.SetText("Luck Slot");
                    itemDescription.SetText("Place a Mysterious Crystal here to gain +1% crit chance.");
                }
            }
            else
            {
                if (crystalType == CrystalType.Life)
                {
                    itemName.SetText("Crystal of Life");
                    itemDescription.SetText("Increases HP and HP regen. Upgrading will multiply bonus HP by 2" +
                        " and increases HP regen by 1. ");
                }
                else if (crystalType == CrystalType.Force)
                {
                    itemName.SetText("Crystal of Force");
                    itemDescription.SetText("Increases basic attack DMG. Upgrading will multiply bonus damage by 2. ");
                }
                else if (crystalType == CrystalType.Magic)
                {
                    itemName.SetText("Crystal of Magic");
                    itemDescription.SetText("Increases spell DMG. Upgrades multiply bonus value by 2. ");
                }
                else if (crystalType == CrystalType.Luck)
                {
                    itemName.SetText("Crystal of Luck");
                    itemDescription.SetText("Increases crit chance. Upgrades multiply bonus value by 2. ");
                }
            }
        }
    }

    public void AddText(string text)
    {
        itemDescription.SetText(itemDescription.text + text);
    }
}
