using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInOrder : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects;

    private void Start()
    {
        foreach (GameObject g in gameObjects)
        {
            g.SetActive(true);
        }
    }
}
