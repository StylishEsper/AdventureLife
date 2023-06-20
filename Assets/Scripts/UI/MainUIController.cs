using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformer.UI
{
    /// <summary>
    /// A simple controller for switching between UI panels.
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        public GameObject[] panels;

        [SerializeField] private InventoryController inventory;
        [SerializeField] private InformationWindowController info;
        [SerializeField] private Button btnMenu;
        [SerializeField] private Button btnSettings;
        [SerializeField] private Button btnSave;
        private Sprite defaultImage;
        private Sprite activeImage;
        private PauseButtons currSelection;

        private bool btnIsSelected;

        private void Start()
        {
            btnMenu.onClick.AddListener(LoadMainMenu);
            btnSettings.onClick.AddListener(ShowSettings);
            btnSave.onClick.AddListener(SaveGame);
            defaultImage = Resources.Load<Sprite>("Prefabs/UI/UI-Menu-Button-Default");
            activeImage = Resources.Load<Sprite>("Prefabs/UI/UI-Menu-Button-Active");
        }

        private void Update()
        {
            if (btnIsSelected)
            {
                if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
                {
                    ClearSelection();
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currSelection == PauseButtons.None)
                {
                    currSelection = PauseButtons.MainMenu;
                    btnMenu.GetComponent<Image>().sprite = activeImage;
                }
                else if (currSelection == PauseButtons.MainMenu)
                {
                    ClearSelection();
                    currSelection = PauseButtons.Settings;
                    btnSettings.GetComponent<Image>().sprite = activeImage;
                }
                else if (currSelection == PauseButtons.Settings)
                {
                    ClearSelection();
                    currSelection = PauseButtons.Save;
                    btnSave.GetComponent<Image>().sprite = activeImage;
                }
                else if (currSelection == PauseButtons.Save)
                {
                    ClearSelection();
                    currSelection = PauseButtons.MainMenu;
                    btnMenu.GetComponent<Image>().sprite = activeImage;
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currSelection == PauseButtons.MainMenu)
                {
                    LoadMainMenu();
                }
                else if (currSelection == PauseButtons.Settings)
                {
                    ShowSettings();
                }
                else if (currSelection == PauseButtons.Save)
                {
                    SaveGame();
                }
            }

            if (gameObject.activeInHierarchy && inventory.gameObject.activeInHierarchy ||
                gameObject.activeInHierarchy && info.gameObject.activeInHierarchy)
            {
                inventory.disabled = true;
                info.disabled = true;
            }
        }

        public void ClearSelection()
        {
            currSelection = PauseButtons.None;
            btnIsSelected = false;
            btnMenu.GetComponent<Image>().sprite = defaultImage;
            btnSettings.GetComponent<Image>().sprite = defaultImage;
            btnSave.GetComponent<Image>().sprite = defaultImage;
        }

        public void LoadMainMenu()
        {
            ClearSelection();
            currSelection = PauseButtons.MainMenu;
            btnMenu.GetComponent<Image>().sprite = activeImage;
            btnIsSelected = true;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowSettings()
        {
            ClearSelection();
            currSelection = PauseButtons.Settings;
            btnSettings.GetComponent<Image>().sprite = activeImage;
            btnIsSelected = true;
        }

        public void SaveGame()
        {
            ClearSelection();
            currSelection = PauseButtons.Save;
            btnSave.GetComponent<Image>().sprite = activeImage;
            btnIsSelected = true;

            var player = GameObject.Find("Player").GetComponent<PlayerController>();

            List<Item.ItemName> itemNames = new List<Item.ItemName>();
            List<int> slotNumbers = new List<int>();
            int[] quickSlotNumbers = new int[player.obtainedItems.Count];
            bool[] inQuickSlots = new bool[player.obtainedItems.Count];
            int i = 0;

            foreach (Item item in player.obtainedItems)
            {
                itemNames.Add(item.itemName);
                slotNumbers.Add(item.slotNumber);

                if (item.inQuickSlot)
                {
                    quickSlotNumbers[i] = item.quickSlotNumber;
                    inQuickSlots[i] = item.inQuickSlot;
                }

                i++;
            }
            var sessionData = GameObject.Find("SessionData").GetComponent<SessionData>();
            string scene = sessionData.scene;
            float timePlayed = sessionData.timePlayed;

            SaveSystem.SaveGame(itemNames, slotNumbers, player.objectives, player.worldItems, player.playedCutscenes, player.memorableEvents,
                player.health.GetHealth(), player.energy.GetEnergy(), player.loadedSave, quickSlotNumbers, player.crystalUpgrades,
                timePlayed, player.transform.position, scene, player.currentElement, inQuickSlots, player.equippedCrystals);

            GameObject notif = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notification"));
            notif.GetComponent<NotificationController>().SetNotification(
                "The game has been sucessfully saved.", false);
        }

        public void SetActivePanel(int index)
        {
            for (var i = 0; i < panels.Length; i++)
            {
                var active = i == index;
                var g = panels[i];
                if (g.activeSelf != active) g.SetActive(active);
            }
        }

        private void OnDisable()
        {
            inventory.disabled = false;
            info.disabled = false;
        }

        private void OnEnable()
        {
            SetActivePanel(0);
        }

        public enum PauseButtons
        {
            None,
            MainMenu,
            Settings,
            Save
        }
    }
}