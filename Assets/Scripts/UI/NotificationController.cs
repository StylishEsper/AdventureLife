using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private Image bg;

    private float decrement;
    private float currAlpha;

    private int startCount;
    private int decrementCount;

    [SerializeField] private bool autoSet;
    private bool soft;

    private void Start()
    {
        if (autoSet)
        {
            tmp = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            SetNotification(tmp.text, true);
        }
    }

    private void Update()
    {
        if (tmp != null && bg != null)
        {
            if (!soft)
            {
                if (startCount >= 180)
                {
                    decrementCount++;
                    if (decrementCount >= 6)
                    {
                        currAlpha -= decrement;
                        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, currAlpha);
                        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, currAlpha);
                        decrementCount = 0;
                    }
                }
                else
                {
                    startCount++;
                }

                if (currAlpha <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                decrementCount++;
                if (decrementCount >= 6)
                {
                    currAlpha += decrement;
                    bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, currAlpha);
                    tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, currAlpha);
                    decrementCount = 0;
                }

                if (currAlpha >= 1)
                {
                    soft = false;
                }
            }
        }
    }

    public void SetNotification(string text, bool soft)
    {
        bg = transform.GetChild(0).GetComponent<Image>();
        tmp = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tmp.SetText(text);

        this.soft = soft;

        if (soft)
        {
            currAlpha = 0;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
        }
        else 
        {
            currAlpha = 1;
        }

        decrement = 0.1f;
        startCount = 0;
        decrementCount = 0;
    }
}
