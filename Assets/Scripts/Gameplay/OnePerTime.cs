using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePerTime : MonoBehaviour
{
    [SerializeField] private List<ObjectPositionChanger> positionChangers;

    private bool oneFound;

    private void Update()
    {
        oneFound = false;
        foreach (ObjectPositionChanger opc in positionChangers)
        {
            if (opc.detected && !oneFound)
            {
                oneFound = true;
                opc.allowChange = true;
            }

            opc.allowChange = false;
        }
    }
}
