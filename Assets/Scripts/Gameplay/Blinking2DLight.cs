using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blinking2DLight : MonoBehaviour
{
    public float minIntensity;
    public float maxIntensity;
    public float increment;
    public float decrement;
    public float blinkWaitLength;
    public float perChangeLength;
    public float pauseLength;

    public int blinksBeforePause;

    public Stage stage;

    public bool reverse;
    public bool canPause;

    internal new UnityEngine.Rendering.Universal.Light2D light;

    private float blinkTime;
    private float changeTime;
    private float pauseTime;

    private int cycleCheck;
    private int blinkCount;

    private bool isPaused;

    private void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        light.intensity = minIntensity;

        if (!reverse)
        {
            stage = Stage.Incrementing;
        }
        else
        {
            stage = Stage.Decrementing;
        }

        blinkTime = Time.time;
        cycleCheck = 0;
        blinkCount = 0;
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (Time.time >= blinkTime + blinkWaitLength)
            {
                if (Time.time >= changeTime + perChangeLength && stage != Stage.CycleComplete)
                {
                    if (stage == Stage.Decrementing)
                    {
                        light.intensity = Mathf.Clamp(light.intensity - decrement, minIntensity, maxIntensity);

                        if (light.intensity == minIntensity)
                        {
                            stage = Stage.Incrementing;
                            cycleCheck++;
                        }
                    }
                    else if (stage == Stage.Incrementing)
                    {
                        light.intensity = Mathf.Clamp(light.intensity + increment, minIntensity, maxIntensity);

                        if (light.intensity == maxIntensity)
                        {
                            stage = Stage.Decrementing;
                            cycleCheck++;
                        }
                    }

                    changeTime = Time.time;
                }

                if (cycleCheck == 2)
                {
                    stage = Stage.CycleComplete;
                }

                if (stage == Stage.CycleComplete)
                {
                    cycleCheck = 0;
                    blinkCount++;

                    if (blinkCount == blinksBeforePause && canPause)
                    {
                        isPaused = true;
                        pauseTime = Time.time;
                        blinkCount = 0;
                    }

                    if (light.intensity == minIntensity)
                    {
                        stage = Stage.Incrementing;
                    }
                    else if (light.intensity == maxIntensity)
                    {
                        stage = Stage.Decrementing;
                    }

                    blinkTime = Time.time;
                }
            }
        }
        else
        {
            if (Time.time >= pauseTime + pauseLength)
            {
                isPaused = false;
            }
        }
    }

    public enum Stage
    {
        Decrementing,
        Incrementing,
        CycleComplete
    }
}
