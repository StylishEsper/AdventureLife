using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffLength : MonoBehaviour
{
    public PlayerController player;
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxLength(float length)
    {
        slider.maxValue = length;
        slider.value = length;

        fill.color = gradient.Evaluate(1);
    }

    public void SetLength(float length)
    {
        slider.value = length;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void Dissapear()
    {
        gameObject.SetActive(false);
    }

    public void Appear()
    {
        gameObject.SetActive(true);
    }
}
