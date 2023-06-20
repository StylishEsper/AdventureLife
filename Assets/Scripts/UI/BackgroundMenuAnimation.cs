using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMenuAnimation : MonoBehaviour
{
    [SerializeField] private Animator sadKid;
    [SerializeField] private Animator dad;

    private float startTime;
    private float drinkTime;

    private bool headLowered;

    private void Start()
    {
        startTime = Time.time;
        drinkTime = Time.time;
        headLowered = false;
        dad.SetTrigger("holdingCoffee");
    }

    private void Update()
    {
        if (!headLowered && Time.time >= startTime + 5f)
        {
            sadKid.SetTrigger("lowerHead");
            headLowered = true;
        }

        if (Time.time >= drinkTime + 5f)
        {
            dad.SetTrigger("drink");
            drinkTime = Time.time;
        }
    }
}
