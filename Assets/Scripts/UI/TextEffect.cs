using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    public TxtEffect effect;

    private Button button;
    private TextMeshProUGUI text;

    private bool alphaDown;

    private void Start()
    {
        button = transform.parent.GetComponent<Button>();
        text = GetComponent<TextMeshProUGUI>();
        alphaDown = true;
    }

    private void Update()
    {
        if (effect == TxtEffect.Blink)
        {
            if (alphaDown)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 
                    text.color.a - 0.01f);

                if (text.color.a <= 0)
                {
                    alphaDown = false;
                }
            }
            else
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b,
                    text.color.a + 0.01f);

                if (text.color.a >= 1)
                {
                    alphaDown = true;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (effect == TxtEffect.ColorChange && button.interactable)
        {
            text.color = new Color(0f, 0.75f, 1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (effect == TxtEffect.ColorChange)
        {
            text.color = new Color(1f, 1f, 1f);
        }
    }

    private void OnDisable()
    {
        if (effect == TxtEffect.ColorChange)
        {
            text = GetComponent<TextMeshProUGUI>();
            text.color = new Color(1f, 1f, 1f);
        }
    }

    public enum TxtEffect
    {
        None,
        Blink,
        ColorChange
    }
}
