using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using static Item;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public ItemSpawner itemSpawner;
    public Canvas canvas;

    private PlayerController player;
    private Animator animator;
    private Rigidbody2D rb;

    private bool openable;
    private bool chestEmpty;


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        canvas.enabled = false;
    }

    private void Update()
    {
        if (openable)
        {
            if (Input.GetKeyDown(KeyCode.F) && !player.isDead && !chestEmpty)
            {
                if (PlayerHasAKey())
                {
                    canvas.enabled = false;
                    animator.SetTrigger("open");
                    itemSpawner.SpawnItem();
                    chestEmpty = true;
                }
                else
                {
                    GameObject popupText = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/PopupText"));
                    popupText.transform.position = new Vector3(player.transform.position.x,
                        player.transform.position.y + 0.4f);
                    TextPopUp textPopUp = popupText.GetComponent<TextPopUp>();
                    textPopUp.SetColor(1, 0, 0);
                    textPopUp.SetText("KEY REQUIRED");
                }
            }
        }
    }

    public bool PlayerHasAKey()
    {
        foreach (Item item in player.obtainedItems)
        {
            if (item.itemName == ItemName.Key)
            {
                item.UseItem();
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
        }

        if (collision.tag == "Player" && !chestEmpty)
        {
            openable = true;
            canvas.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            openable = false;
            canvas.enabled = false;
        }
    }
}
