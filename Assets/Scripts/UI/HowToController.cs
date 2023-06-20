using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToController : MonoBehaviour
{
    private InventoryController inventory;
    private InformationWindowController info;
    private GameController game;
    [SerializeField] private Button btnGotIt;

    private void Start()
    {
        inventory = GameObject.Find("Player").transform.GetChild(0).
            transform.GetChild(2).GetComponent<InventoryController>();
        info = GameObject.Find("Player").transform.GetChild(0).
            transform.GetChild(4).GetComponent<InformationWindowController>();
        game = GameObject.Find("GameController").GetComponent<GameController>();
        btnGotIt.onClick.AddListener(Close);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            Time.timeScale = 0;
            inventory.disabled = true;
            info.disabled = true;
            GameObject.Find("Player").GetComponent<PlayerController>().controlEnabled = false;
            game.SetCursorVisibility(true);
        }
    }

    private void Close()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().controlEnabled = true;
        game.SetCursorVisibility(false);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        inventory.disabled = false;
        info.disabled = false;
    }
}
