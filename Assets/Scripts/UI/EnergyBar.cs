using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public float incrementTime;

    private float startTime;

    private const int minEnergy = 0;
    private const int maxEnergy = 100;

    void Start()
    {
        SetMaxEnergy(maxEnergy);
        SetEnergy(minEnergy);
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time > startTime + incrementTime)
        {
            SetEnergy((int)slider.value + 1);
            startTime = Time.time;
        }
    }

    public void SetMaxEnergy(int energy)
    {
        slider.maxValue = energy;
        slider.value = energy;

        fill.color = gradient.Evaluate(1);
    }

    public void SetEnergy(int energy)
    {
        slider.value = energy;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public int GetEnergy()
    {
        return (int)slider.value;
    }

    public void Cost(int energy)
    {
        SetEnergy((int)slider.value - energy);
    }
}
