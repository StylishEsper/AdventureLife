using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconToggler : MonoBehaviour
{
    public PlayerController player;
    public Sprite waterIcon;
    public Sprite fireIcon;
    public Sprite electricIcon;
    public Sprite iceIcon;
    public RectTransform rect;

    private Image displayImage;

    private void Start()
    {
        displayImage = GetComponent<Image>();
        PlayerData data = SaveSystem.LoadGame(player.loadedSave);
        IconTo(data.element);
    }

    private void Update()
    {
        if (!player.isCommanding)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                IconToWater();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                IconToFire();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                IconToElectric();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                IconToIce();
            }
        }
    }

    public void IconToWater()
    {
        displayImage.sprite = waterIcon;
        displayImage.SetNativeSize();
        displayImage.transform.localScale = new Vector3(4f, 4f, 1f);
        rect.sizeDelta = new Vector2(11f, 16f);
        player.currentElement = "Water";
    }

    public void IconToFire()
    {
        displayImage.sprite = fireIcon;
        displayImage.SetNativeSize();
        displayImage.transform.localScale = new Vector3(2f, 2f, 1f);
        rect.sizeDelta = new Vector2(32f, 32f);
        player.currentElement = "Fire";
    }

    public void IconToElectric()
    {
        displayImage.sprite = electricIcon;
        displayImage.SetNativeSize();
        displayImage.transform.localScale = new Vector3(4f, 4f, 1f);
        rect.sizeDelta = new Vector2(12f, 16f);
        player.currentElement = "Electric";
    }

    public void IconToIce()
    {
        displayImage.sprite = iceIcon;
        displayImage.SetNativeSize();
        displayImage.transform.localScale = new Vector3(3f, 3f, 1f);
        rect.sizeDelta = new Vector2(21.5625f, 21.5625f);
        player.currentElement = "Ice";
    }

    public void IconTo(string element)
    {
        if (element == "Water")
        {
            IconToWater();
        }
        else if (element == "Fire")
        {
            IconToFire();   
        }
        else if (element == "Electric")
        {
            IconToElectric();
        }
        else if (element == "Ice")
        {
            IconToIce();
        }
    }
}
