using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextImageSetter : MonoBehaviour
{
    public Image image;
    public Sprite waterIcon;
    public Sprite fireIcon;
    public Sprite electricIcon;
    public Sprite iceIcon;
    public Sprite resistanceIcon;
    public ImageAnimation imageAnimation;

    public void SetImage(string element, bool resistance)
    {
        if (element != null && element != "None" && !resistance)
        {
            image.enabled = true;

            if (element == "Water")
            {
                image.sprite = waterIcon;
                transform.localScale = new Vector3(1f, 1.4f);
            }
            else if (element == "Fire")
            {
                image.sprite = fireIcon;
                transform.localScale = new Vector3(1.4f, 1.4f);
            }
            else if (element == "Electric")
            {
                image.sprite = electricIcon;
                transform.localScale = new Vector3(1f, 1.4f);
            }
            else if (element == "Ice")
            {
                image.sprite = iceIcon;
                transform.localScale = new Vector3(1.4f, 1.4f);
            }
        }
        else if (resistance)
        {
            image.enabled = true;
            image.sprite = resistanceIcon;
            imageAnimation.pause = false;
            transform.localScale = new Vector3(1.5f, 1.5f);
        }
    }
}
