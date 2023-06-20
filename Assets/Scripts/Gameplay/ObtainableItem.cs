using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using static Item;
using UnityEngine;

[System.Serializable]
public class ObtainableItem : MonoBehaviour
{
    public Canvas canvas;

    public ItemName itemName;
    public WorldItem worldItemName;

    public float worldLastingLength;
    public float moveDelay;
    public float moveAmount;

    public bool foreverlasting = false;
    public bool stationary = false;
    public bool isWorldItem;

    private PlayerController player;
    private Rigidbody2D rb;

    private Vector3 originalPositiion;
    private Vector3 moveTo;

    private float moveTime;

    private bool goingUp;
    private bool floatEnabled;
    private bool obtainable;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (isWorldItem)
        {
            PlayerData data = SaveSystem.LoadGame(player.loadedSave);

            foreach (WorldItem worldItem in data.worldItems)
            {
                if (worldItem == worldItemName)
                {
                    Destroy(gameObject);
                    break;
                }
            }
        }

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.2f;
        canvas.enabled = false;

        if (!foreverlasting)
        {
            Destroy(gameObject, worldLastingLength);
        }

        goingUp = true;
        floatEnabled = false;
        obtainable = false;
    }

    private void Update()
    {
        if (floatEnabled && !stationary)
        {
            if (Time.time >= moveTime + moveDelay)
            {
                if (goingUp)
                {
                    if (transform.position.y < moveTo.y)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + moveAmount, -1);
                    }
                    else if (transform.position.y >= moveTo.y)
                    {
                        goingUp = false;
                    }
                }
                else
                {
                    if (transform.position.y > originalPositiion.y)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - moveAmount, -1);
                    }
                    else if (transform.position.y <= originalPositiion.y)
                    {
                        goingUp = true;
                    }
                }

                moveTime = Time.time;
            }
        }

        if (obtainable)
        {
            if (Input.GetKeyDown(KeyCode.F) && !player.isDead)
            {
                Item item = null;

                item = new Item(itemName, 0);

                if (!player.slotGenerator.GetIsInventoryFull(item))
                {
                    player.PickUpItem(item, worldItemName);
                    CheckIfTipIsRequired(item.itemName);
                    //Destroy(gameObject);
                }
                else
                {
                    GameObject popupText = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/PopupText"));
                    popupText.transform.position = new Vector3(player.transform.position.x,
                        player.transform.position.y + 0.4f);
                    TextPopUp textPopUp = popupText.GetComponent<TextPopUp>();
                    textPopUp.SetColor(1,0,0);
                    textPopUp.SetText("Inventory Full");
                }
            }
        }
    }

    public void CheckIfTipIsRequired(Item.ItemName item)
    {
        if (item == Item.ItemName.WayOfTheSpider)
        {
            Instantiate(Resources.Load("Prefabs/UI/HowToWallJump"));
        }
        else if (item == Item.ItemName.SkillOfASwordsman)
        {
            
        }
        else if (item == Item.ItemName.FlowOfTheRiver)
        {
         
        }
        else if (item == Item.ItemName.RageOfTheOcean)
        {
          
        }
        else if (item == Item.ItemName.BreathOfADragon)
        {

        }
        else if (item == Item.ItemName.HellsMostWanted)
        {

        }
        else if (item == Item.ItemName.RoarOfThunder)
        {

        }
        else if (item == Item.ItemName.GiftFromTheStorm)
        {

        }
        else if (item == Item.ItemName.PinnacleOfWinter)
        {

        }
        else if (item == Item.ItemName.TheIceKing)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" && rb.gravityScale > 0)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            floatEnabled = true;
            moveTime = Time.time;
            originalPositiion = new Vector3(transform.position.x, transform.position.y, -1f);
            moveTo = new Vector3(transform.position.x, transform.position.y + 0.1f, -1f);
        }

        if (collision.tag == "Player")
        {
            obtainable = true;
            canvas.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            obtainable = false;
            canvas.enabled = false;
        }
    }

    [SerializeField]
    public enum WorldItem
    {
        None,
        ParentsRoomPotion,
        FOSHerb1,
        FOSHerb2,
        FOSHerb3,
        FOSHerb4
    }
}
