using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SunLightController : MonoBehaviour
{
    const float maxR = 0;
    const float maxG = 0.75f;
    const float maxB = 1f;
    const float minG = 0;
    const float minB = 0;

    public SpriteRenderer skyColor;

    public float alterAfter;

    public bool gettingDarker;

    private UnityEngine.Rendering.Universal.Light2D sunLight;

    private float startTime;
    private float minIntensity;
    private float maxIntensity;
    private float currentG;
    private float currentB;

    void Start()
    {
        minIntensity = 0.4f;
        maxIntensity = 1f;
        sunLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        startTime = Time.time;
        skyColor.color = new Color(maxR, maxG, maxB);
        currentG = maxG;
        currentB = maxB;
    }

    void Update()
    {
        if (Time.time > startTime + alterAfter)
        {
            if (gettingDarker)
            {
                sunLight.intensity -= 0.01f;
            }
            else
            {
                sunLight.intensity += 0.01f;
            }

            if (sunLight.intensity <= minIntensity || sunLight.intensity >= maxIntensity)
            {
                if (gettingDarker)
                {
                    gettingDarker = false;
                }
                else
                {
                    gettingDarker = true;
                }
            }

            UpdateSkyColor();

            startTime = Time.time;
        }
    }

    public void UpdateSkyColor()
    {
        if (gettingDarker)
        {
            if (currentG > minG && currentG <= maxG)
            {
                currentG -= 0.01f;
            }

            if (currentB > minB && currentB <= maxB)
            {
                currentB -= 0.01f;
            }
        }
        else
        {
            if (currentG >= minG && currentG < maxG)
            {
                currentG += 0.01f;
            }

            if (currentB >= minB && currentB < maxB)
            {
                currentB += 0.01f;
            }
        }

        skyColor.color = new Color(0, currentG, currentB);
    }
}
