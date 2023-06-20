using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerCrystal : MonoBehaviour
{
    public PlayerController player;
    public Image crystalImage;
    public ImageAnimation imageAnimation;
    public SlotSelectionCheck selectionCheck;
    public GameObject infoPopup;

    public CrystalType crystalType;

    public int currentUpgrades;

    public bool crystalOn;

    private const int maxUpgrades = 6;
    private const int minUpgrades = 0;
    private const int maxAddedAmount = 64;
    private const int minAddedAmount = 1;
    private const float infoPopupDelay = 0.5f;

    private GameObject currInfoPopup;
    private ImageAnimationLists animationLists;

    private float selectedStartTime;

    private bool infoTimeStarted;


    private void Start()
    {
        animationLists = GetComponent<ImageAnimationLists>();
        imageAnimation.sprites = animationLists.crystalSprites;

        if (crystalOn)
        {
            EnableCrystal();
        }
        else
        {
            crystalImage.enabled = false;
            imageAnimation.pause = true;
        }
    }

    private void Update()
    {
        if (selectionCheck.hovered)
        {
            if (!infoTimeStarted)
            {
                selectedStartTime = Time.time;
                infoTimeStarted = true;
            }

            if (Time.time > selectedStartTime + infoPopupDelay)
            {
                LoadInfo();
            }

            if (Input.mouseScrollDelta != new Vector2(0, 0) || Input.GetMouseButtonDown(0))
            {
                DestroyInfo();
            }
        }
        else
        {
            if (infoTimeStarted)
            {
                DestroyInfo();
                infoTimeStarted = false;
            }
        }
    }

    public void LoadInfo()
    {
        if (currInfoPopup == null)
        {
            currInfoPopup = Instantiate(infoPopup);
            ItemInformationSetter itemInformation = currInfoPopup.GetComponent<ItemInformationSetter>();
            itemInformation.SetInfo(Item.ItemName.None, crystalType, true, crystalOn);

            if (crystalOn)
            {
                if (crystalType == CrystalType.Life)
                {
                    itemInformation.AddText("\n\nBonus: +" + Calculate() + " HP, +" +
                        (currentUpgrades + 1) + " HP regen.\nUpgrades: " + currentUpgrades + "/6");
                }
                else if (crystalType == CrystalType.Force)
                {
                    itemInformation.AddText("\n\nBonus: +" + Calculate() + " basic DMG.\nUpgrades: "
                        + currentUpgrades + "/6");
                }
                else if (crystalType == CrystalType.Magic)
                {
                    itemInformation.AddText("\n\nBonus: +" + Calculate() + " spell DMG.\nUpgrades: "
                        + currentUpgrades + "/6");
                }
                else if (crystalType == CrystalType.Luck)
                {
                    itemInformation.AddText("\n\nBonus: +" + Calculate() + "% crit chance.\nUpgrades: "
                        + currentUpgrades + "/6");
                }
            }

            currInfoPopup.transform.SetParent(GameObject.Find("Player").transform.GetChild(0).transform
                .GetChild(2).transform.GetChild(0));
            currInfoPopup.transform.position = new Vector3(transform.position.x + 225f, transform.position.y - 250f);
        }
    }

    public void DestroyInfo()
    {
        if (currInfoPopup != null)
        {
            if (currInfoPopup.activeInHierarchy)
            {
                Destroy(currInfoPopup);
            }
        }
    }

    public void EnableCrystal()
    {
        crystalOn = true;
        crystalImage.enabled = true;
        imageAnimation.pause = false;

        if (crystalType == CrystalType.Life)
        {
            player.bonusHP = Calculate();
            player.regenAmount = currentUpgrades + 1;
            player.health.UpdateMaxHealth();
            player.equippedCrystals[0] = true;
            player.crystalUpgrades[0] = currentUpgrades;
        }
        else if (crystalType == CrystalType.Force)
        {
            player.bonusAtk = Calculate();
            player.equippedCrystals[1] = true;
            player.crystalUpgrades[1] = currentUpgrades;
        }
        else if (crystalType == CrystalType.Magic)
        {
            player.bonusMgc = Calculate();
            player.equippedCrystals[2] = true;
            player.crystalUpgrades[2] = currentUpgrades;
        }
        else if (crystalType == CrystalType.Luck)
        {
            player.bonusCrit = Calculate();
            player.equippedCrystals[3] = true;
            player.crystalUpgrades[3] = currentUpgrades;
        }

    }

    public void Upgrade()
    {
        currentUpgrades = Mathf.Clamp(currentUpgrades + 1, minUpgrades, maxUpgrades);
        imageAnimation.switchSprites = animationLists.crystalOpenSprites;
        imageAnimation.switchOnEnd = true;
        EnableCrystal();
    }

    public bool CheckIfMaxed()
    {
        bool isMaxed = false;

        if (currentUpgrades == maxUpgrades)
        {
            isMaxed = true;
        }

        return isMaxed;
    }

    public int Calculate()
    {
        int addedAmount = 1;

        for (int i = 0; i < currentUpgrades; i++)
        {
            addedAmount *= 2;
        }

        addedAmount = Mathf.Clamp(addedAmount, minAddedAmount, maxAddedAmount);

        return addedAmount;
    }

    public enum CrystalType
    {
        None,
        Life,
        Force,
        Magic,
        Luck
    }
}
