using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightForDark : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D sun;
    UnityEngine.Rendering.Universal.Light2D selfLight;

    // Start is called before the first frame update
    void Start()
    {
        selfLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        if (sun == null)
        {
            GameObject gameObject = GameObject.Find("Sun");
            sun = gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sun.intensity >= 0.4 && sun.intensity <= 0.49)
        {
            selfLight.intensity = 0.6f;
        }
        else if (sun.intensity >= 0.5 && sun.intensity <= 0.59)
        {
            selfLight.intensity = 0.5f;
        }
        else if (sun.intensity >= 0.6 && sun.intensity <= 0.69)
        {
            selfLight.intensity = 0.4f;
        }
        else if (sun.intensity >= 0.7 && sun.intensity <= 0.79)
        {
            selfLight.intensity = 0.3f;
        }
        else if (sun.intensity >= 0.8 && sun.intensity <= 0.89)
        {
            selfLight.intensity = 0.2f;
        }
        else if (sun.intensity >= 0.9 && sun.intensity <= 0.99)
        {
            selfLight.intensity = 0.1f;
        }
        else if (sun.intensity >= 1)
        {
            selfLight.intensity = 0f;
        }
    }
}
