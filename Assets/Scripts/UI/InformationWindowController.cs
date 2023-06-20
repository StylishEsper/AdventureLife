using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationWindowController : MonoBehaviour
{
    public PlayerController player;
    public GameObject content;
    public GameObject objectivesTab;
    public GameObject scrollsTab;
    public GameObject monstersTab;
    public Button btnObjectives;
    public Button btnScrolls;
    public Button btnMonsters;
    public List<GameObject> textContents = new List<GameObject>();
    public GameController game;

    public bool disabled;

    [SerializeField] private TextMeshProUGUI count;
    private Sprite defaultImage;
    private Sprite activeImage;
    private Tabs currTab;

    private void Start()
    {
        content.SetActive(false);
        btnObjectives.onClick.AddListener(SetCurrentTabObjectives);
        btnScrolls.onClick.AddListener(SetCurrentTabScrolls);
        btnMonsters.onClick.AddListener(SetCurrentTabMonsters);
        defaultImage = Resources.Load<Sprite>("Prefabs/UI/UI-Menu-Button-Default");
        activeImage = Resources.Load<Sprite>("Prefabs/UI/UI-Menu-Button-Active");
    }

    private void Update()
    {
        if (!disabled)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (content.activeInHierarchy)
                {
                    player.controlEnabled = true;
                    content.SetActive(false);
                    game.SetCursorVisibility(false);
                    Time.timeScale = 1;
                }
                else
                {
                    SetCurrentTabObjectives();
                    player.controlEnabled = false;
                    game.SetCursorVisibility(true);
                    content.SetActive(true);
                    Time.timeScale = 0;
                }
            }

            if (content.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (currTab == Tabs.Objectives)
                    {
                        SetCurrentTabScrolls();
                    }
                    else if (currTab == Tabs.Scrolls)
                    {
                        SetCurrentTabMonsters();
                    }
                    else if (currTab == Tabs.Monsters)
                    {
                        SetCurrentTabObjectives();
                    }
                }
            }
        }
        else
        {
            if (content.activeInHierarchy)
            {
                content.SetActive(false);
            }
        }
    }

    public void ClearCurrentTab()
    {
        currTab = Tabs.None;
        btnObjectives.GetComponent<Image>().sprite = defaultImage;
        btnScrolls.GetComponent<Image>().sprite = defaultImage;
        btnMonsters.GetComponent<Image>().sprite = defaultImage;
        objectivesTab.SetActive(false);
        scrollsTab.SetActive(false);
        foreach (GameObject gameObject in textContents)
        {
            gameObject.SetActive(false);
        }
        monstersTab.SetActive(false);
    }

    public void SetCurrentTabObjectives()
    {
        ClearCurrentTab();
        currTab = Tabs.Objectives;
        objectivesTab.SetActive(true);
        btnObjectives.GetComponent<Image>().sprite = activeImage;
    }

    public void SetCurrentTabScrolls()
    {
        ClearCurrentTab();
        currTab = Tabs.Scrolls;
        scrollsTab.SetActive(true);
        int c = 0;

        foreach (Item item in player.obtainedItems)
        {
            if (item.itemName == Item.ItemName.WayOfTheSpider)
            {
                textContents[0].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.SkillOfASwordsman)
            {
                textContents[1].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.FlowOfTheRiver)
            {
                textContents[2].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.RageOfTheOcean)
            {
                textContents[3].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.BreathOfADragon)
            {
                textContents[4].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.HellsMostWanted)
            {
                textContents[5].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.RoarOfThunder)
            {
                textContents[6].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.GiftFromTheStorm)
            {
                textContents[7].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.PinnacleOfWinter)
            {
                textContents[8].SetActive(true);
            }
            else if (item.itemName == Item.ItemName.TheIceKing)
            {
                textContents[9].SetActive(true);
            }
        }

        foreach (GameObject text in textContents)
        {
            if (text.activeInHierarchy)
            {
                c++;
            }
        }

        count.SetText(c + "/10");
        btnScrolls.GetComponent<Image>().sprite = activeImage;
    }

    public void SetCurrentTabMonsters()
    {
        ClearCurrentTab();
        currTab = Tabs.Monsters;
        monstersTab.SetActive(true);
        btnMonsters.GetComponent<Image>().sprite = activeImage;
    }

    public enum Tabs
    {
        None,
        Objectives,
        Scrolls,
        Monsters
    }
}
