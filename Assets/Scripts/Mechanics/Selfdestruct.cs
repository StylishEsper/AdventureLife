using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestruct : MonoBehaviour
{
    public float seconds;

    public bool isEnabled;

    private float startTime;

    [SerializeField] private bool disableMode;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (!isEnabled)
        {
            startTime = Time.time;
        }

        if (Time.time >= startTime + seconds && isEnabled)
        {
            Now();
        }
    }

    public void Now()
    {
        if (!disableMode)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        startTime = Time.time;
        isEnabled = true;
    }
}
