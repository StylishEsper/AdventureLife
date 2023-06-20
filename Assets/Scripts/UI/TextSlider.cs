using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI text;

    public float minValue;
    public float maxValue;

    private void Start()
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = minValue;
    }

    private void Update()
    {
        text.text = "Drop x" + slider.value;
    }
}
