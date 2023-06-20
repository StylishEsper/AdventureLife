using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    private GameObject fixedBridge;
    private GameObject brokenBridge;

    private void Start()
    {
        fixedBridge = transform.GetChild(0).gameObject;
        brokenBridge = transform.GetChild(1).gameObject;
    }

    public void BreakBridge()
    {
        fixedBridge.SetActive(false);
        brokenBridge.SetActive(true);
    }
}
