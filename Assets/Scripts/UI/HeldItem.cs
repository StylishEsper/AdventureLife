using System.Collections;
using System.Collections.Generic;
using static Item;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HeldItem : MonoBehaviour
{
    public Sprite healthPotionSprite;
    public Sprite antidoteSprite;
    public Sprite extinguishSprite;
    public Sprite defrostSprite;
    public Sprite unparalyzeSprite;
    public Sprite crystalSprite;
    public Sprite shardSprite;
    public Sprite herbSprite;
    public Sprite keySprite;
    public TextMeshProUGUI text;
    public SlotSelectionCheck selectionCheck;
    public ImageAnimation imageAnimation;
    public ImageAnimationLists animationLists;
    public GameObject infoPopup;
    public GameObject currInfoPopup;

    public Item item;

    public int currentStack = 0;
    public int slotNumber;

    public bool isSelected;
    public bool isHovered;
    public bool canPause;
    public bool canShowInfo = false;

    private const int maxStack = 16;
    private const float infoPopupDelay = 0.5f;

    private Image image;

    private float hoveredStartTime;

    private bool stackable;
    private bool infoTimeStarted;

    public void Start()
    {
        image = GetComponent<Image>();

        if (imageAnimation != null && canPause)
        {
            imageAnimation.pause = true;
        }

        if (item == null)
        {
            item = new Item(ItemName.None, slotNumber);
        }

        stackable = true;
        text.enabled = false;
        infoTimeStarted = false;
        ReloadSlot();
    }

    private void Update()
    {
        if (selectionCheck != null)
        {
            isSelected = selectionCheck.selected;
            isHovered = selectionCheck.hovered;
        }

        if (canPause)
        {
            if (isSelected && item.itemName != ItemName.None)
            {
                imageAnimation.pause = false;
            }
            else if (!isSelected && item.itemName != ItemName.None)
            {
                imageAnimation.pause = true;
            }
        }

        if (item.itemName != ItemName.None && canShowInfo)
        {
            if (isHovered)
            {
                if (!infoTimeStarted)
                {
                    hoveredStartTime = Time.time;
                    infoTimeStarted = true;
                }

                if (Time.time > hoveredStartTime + infoPopupDelay)
                {
                    LoadInfo();
                }

                if (Input.mouseScrollDelta != new Vector2(0, 0))
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
    }

    public void LoadInfo()
    {
        if (currInfoPopup == null)
        {
            currInfoPopup = Instantiate(infoPopup);
            currInfoPopup.GetComponent<ItemInformationSetter>().SetInfo(item.itemName, PowerCrystal.CrystalType.None, false, false);
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
    
    public bool IsSlotFull()
    {
        if (item.itemName == ItemName.None)
        {
            return false;
        }
        else
        {
            if (currentStack < maxStack && stackable)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public bool WillBeOverMax(int amount)
    {
        if (currentStack + amount > maxStack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddItem(Item item)
    {
        stackable = true;

        if (this.item.itemName == ItemName.None)
        {
            this.item = item;
            this.item.slotNumber = slotNumber;
        }
        else
        {
            item.slotNumber = slotNumber;
        }

        if (this.item.itemName == ItemName.MysteriousCrystal || 
            this.item.itemName == ItemName.CrystalShard ||
            this.item.itemName == ItemName.Key)
        {
            stackable = false;
            currentStack++;
        }

        if (stackable)
        {
            currentStack++;
        }

        ReloadSlot();
    }

    public void ReloadSlot()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (item.itemName == ItemName.HealthPotion)
        {
            image.sprite = healthPotionSprite;
            image.color = new Color(1, 1, 1, 1);
            image.transform.localScale = new Vector3(1, 1, 1);
            stackable = true;
            text.enabled = true;
            text.text = currentStack.ToString();

            if (imageAnimation != null)
            {
                imageAnimation.sprites = animationLists.healthPotionSprites;
                imageAnimation.spritePerFrame = 10;
            }
        }
        else if (item.itemName == ItemName.Antidote)
        {
            image.sprite = antidoteSprite;
            image.color = new Color(1, 1, 1, 1);
            image.transform.localScale = new Vector3(1, 1, 1);
            stackable = true;
            text.enabled = true;
            text.text = currentStack.ToString();

            if (imageAnimation != null)
            {
                imageAnimation.sprites = animationLists.antidoteSprites;
                imageAnimation.spritePerFrame = 10;
            }
        }
        else if (item.itemName == ItemName.Extinguish)
        {
            image.sprite = extinguishSprite;
            image.color = new Color(1, 1, 1, 1);
            image.transform.localScale = new Vector3(1, 1, 1);
            stackable = true;
            text.enabled = true;
            text.text = currentStack.ToString();

            if (imageAnimation != null)
            {
                imageAnimation.sprites = animationLists.extinguishSprites;
                imageAnimation.spritePerFrame = 10;
            }
        }
        else if (item.itemName == ItemName.Defrost)
        {
            image.sprite = defrostSprite;
            image.color = new Color(1, 1, 1, 1);
            image.transform.localScale = new Vector3(1, 1, 1);
            stackable = true;
            text.enabled = true;
            text.text = currentStack.ToString();

            if (imageAnimation != null)
            {
                imageAnimation.sprites = animationLists.defrostSprites;
                imageAnimation.spritePerFrame = 10;
            }
        }
        else if (item.itemName == ItemName.Unparalyze)
        {
            image.sprite = unparalyzeSprite;
            image.color = new Color(1, 1, 1, 1);
            image.transform.localScale = new Vector3(1, 1, 1);
            stackable = true;
            text.enabled = true;
            text.text = currentStack.ToString();

            if (imageAnimation != null)
            {
                imageAnimation.sprites = animationLists.unparalyzeSprites;
                imageAnimation.spritePerFrame = 10;
            }
        }
        else if (item.itemName == ItemName.MysteriousCrystal)
        {
            image.sprite = crystalSprite;
            image.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            image.color = new Color(1, 1, 1, 1);
            stackable = false;

            if (imageAnimation != null)
            {
                imageAnimation.sprites = animationLists.crystalSprites;
                imageAnimation.spritePerFrame = 15;
            }
        }
        else if (item.itemName == ItemName.CrystalShard)
        {
            image.sprite = shardSprite;
            image.color = new Color(1, 1, 1, 1);
            stackable = false;
        }
        else if (item.itemName == ItemName.Herb)
        {
            image.sprite = herbSprite;
            image.color = new Color(1, 1, 1, 1);
            image.transform.localScale = new Vector3(1, 1, 1);
            stackable = true;
            text.enabled = true;
            text.text = currentStack.ToString();

            if (imageAnimation != null)
            {
                imageAnimation.sprites = null;
            }
        }
        else if (item.itemName == ItemName.Key)
        {
            image.sprite = keySprite;
            image.color = new Color(1, 1, 1, 1);
            stackable = false;
        }
        else
        {
            image.color = new Color(1, 1, 1, 0);

            stackable = false;
            text.enabled = false;

            if (imageAnimation != null)
            {
                imageAnimation.sprites = null;
            }
        }

        if (item.itemName == ItemName.Key)
        {
            image.SetNativeSize();
            image.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
        else if (item.itemName != ItemName.MysteriousCrystal)
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(60, 60);
            image.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public bool IsHoldingItem()
    {
        if (item.itemName == ItemName.None)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void UseItem()
    {
        item.UseItem();
        DropItem(1);
    }

    public void DropItem(int amount)
    {
        currentStack -= amount;

        if (currentStack <= 0)
        {
            item.itemName = ItemName.None;
        }

        ReloadSlot();
    }

    public void RemoveEntirely()
    {
        item = new Item(ItemName.None, 0);
        item.inQuickSlot = false;
        item.quickSlotNumber = 0;
        ReloadSlot();
    }

    public void Copy(HeldItem cItem)
    {
        item = new Item(cItem.item.itemName, cItem.slotNumber);
        item.player = cItem.item.player;
        item.inQuickSlot = cItem.item.inQuickSlot;
        item.quickSlotNumber = cItem.item.quickSlotNumber;
        currentStack = cItem.currentStack;
        ReloadSlot();
    }

    private void OnDisable()
    {
        DestroyInfo();
    }
}
