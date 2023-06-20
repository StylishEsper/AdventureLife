using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class MainMenuController : MonoBehaviour
{
    public bool disableInput = true;

    public float movementSpeed = 0.125f;

    [SerializeField] private Button playButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button saveButton1;
    [SerializeField] private Button saveButton2;
    [SerializeField] private Button saveButton3;
    [SerializeField] private GameObject buttonGroup1;
    [SerializeField] private GameObject buttonGroup2;
    [SerializeField] private GameObject anyKeyTip;
    [SerializeField] private GameObject fog;
    [SerializeField] private GameObject weather;
    [SerializeField] private RuntimeAnimatorController save2Unselected;
    [SerializeField] private RuntimeAnimatorController save2Selected;
    [SerializeField] private ParticleSystem sleepEffect;
    [SerializeField] private GameObject[] saves;
    [SerializeField] private GameObject[] newGames;
    [SerializeField] private Button[] startButtons;
    private Animator save1Animator;
    private Animator save2Animator;
    private Animator save3Animator;
    private GameObject confirm;
    internal new Camera camera;

    private Vector3 playCameraPosition;
    private Vector3 mainCameraPosition;

    private MenuScreen currentScreen;
    private MenuScreen switchScreen;

    private int currentSelectedSave;

    private bool moving;

    private void Start()
    {
        if (!disableInput)
        {
            camera = Camera.main;
            playCameraPosition = new Vector3(12, camera.transform.position.y,
                camera.transform.position.z);
            mainCameraPosition = new Vector3(0, camera.transform.position.y,
                camera.transform.position.z);
            currentScreen = MenuScreen.Main;
            switchScreen = MenuScreen.Main;

            playButton.gameObject.SetActive(true);
            buttonGroup1.SetActive(true);

            moving = false;
        }
        else
        {
            playButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            buttonGroup1.SetActive(false);
            buttonGroup2.SetActive(false);
            playButton.onClick.AddListener(SetScreenPlay);
            backButton.onClick.AddListener(SetScreenMain);
            exitButton.onClick.AddListener(ExitApp);
            confirmButton.onClick.AddListener(LoadGame);
            toggleButton.onClick.AddListener(Toggle);
            deleteButton.onClick.AddListener(DeleteSave);
            currentSelectedSave = 0;
            saveButton1.onClick.AddListener(() => { SelectSaveState(1); });
            saveButton2.onClick.AddListener(() => { SelectSaveState(2); });
            saveButton3.onClick.AddListener(() => { SelectSaveState(3); });
            startButtons[0].onClick.AddListener(() => { LoadGame(1); });
            startButtons[1].onClick.AddListener(() => { LoadGame(2); });
            startButtons[2].onClick.AddListener(() => { LoadGame(3); });
            save1Animator = saveButton1.transform.GetChild(0).transform.GetChild(3).
                transform.GetChild(0).GetComponent<Animator>();
            save2Animator = saveButton2.transform.GetChild(0).transform.GetChild(3).
                transform.GetChild(1).GetComponent<Animator>();
            save3Animator = saveButton3.transform.GetChild(0).transform.GetChild(3).
                transform.GetChild(0).GetComponent<Animator>();
            ReloadSaves();
            DeselectAllSaves();

            int i = 0;
            foreach (GameObject save in saves)
            {
                i++;
                PlayerData data = SaveSystem.GetGameData(i);

                if (data != null)
                {
                    var s = save.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                    var t = save.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                    string editedText = Regex.Replace(data.scene, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
                    s.text = editedText;
                    t.text = TimeSpan.FromSeconds(data.timePlayed).ToString("hh':'mm':'ss");
                }
            }
        }
    }

    private void Update()
    {
        if (!disableInput && confirm == null)
        {
            if (currentScreen == MenuScreen.Main)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {

                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ExitApp();
                }
            }
            else if (currentScreen == MenuScreen.Play)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Toggle();
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    LoadGame();
                }
                else if (Input.GetKeyDown(KeyCode.Delete))
                {
                    DeleteSave();
                }

                if (EventSystem.current.currentSelectedGameObject != saveButton1.gameObject &&
                    EventSystem.current.currentSelectedGameObject != saveButton2.gameObject &&
                    EventSystem.current.currentSelectedGameObject != saveButton3.gameObject &&
                    EventSystem.current.currentSelectedGameObject != toggleButton.gameObject &&
                    EventSystem.current.currentSelectedGameObject != deleteButton.gameObject &&
                    EventSystem.current.currentSelectedGameObject != confirmButton.gameObject &&
                    currentSelectedSave != 0 && Input.GetMouseButtonDown(0))
                {
                    currentSelectedSave = 0;
                    DeselectAllSaves();
                }
            }

            if (playButton.gameObject.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    SetScreenPlay();
                }
            }
            else if (backButton.gameObject.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    SetScreenMain();
                }
            }

            if (currentScreen != switchScreen)
            {
                if (switchScreen == MenuScreen.Main)
                {
                    if (camera.transform.position.x > mainCameraPosition.x)
                    {
                        camera.transform.position = new Vector3(camera.transform.position.x - movementSpeed,
                            camera.transform.position.y, camera.transform.position.z);

                        moving = true;
                    }
                    else
                    {
                        camera.transform.position = mainCameraPosition;
                        currentScreen = MenuScreen.Main;
                        fog.SetActive(true);
                        weather.SetActive(true);
                        moving = false;
                    }
                }
                else if (switchScreen == MenuScreen.Play)
                {
                    if (camera.transform.position.x < playCameraPosition.x)
                    {
                        camera.transform.position = new Vector3(camera.transform.position.x + movementSpeed,
                            camera.transform.position.y, camera.transform.position.z);

                        moving = true;
                    }
                    else
                    {
                        camera.transform.position = playCameraPosition;
                        currentScreen = MenuScreen.Play;
                        moving = false;
                    }
                }
                else if (switchScreen == MenuScreen.Settings)
                {

                }
                else if (switchScreen == MenuScreen.Credits)
                {

                }
            }
        }
        else
        {
            if (Input.anyKey && confirm == null)
            {
                disableInput = false;
                anyKeyTip.SetActive(false);
                Start();
            }
        }
    }

    public void SetScreenMain()
    {
        DeselectAllSaves();

        if (moving)
        {
            currentScreen = switchScreen;
        }

        switchScreen = MenuScreen.Main;
        backButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        buttonGroup1.SetActive(true);
        buttonGroup2.SetActive(false);
    }

    public void SetScreenPlay()
    {
        if (moving)
        {
            currentScreen = switchScreen;
        }

        switchScreen = MenuScreen.Play;
        backButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        buttonGroup1.SetActive(false);
        buttonGroup2.SetActive(true);
        fog.SetActive(false);
        weather.SetActive(false);
    }

    public void SetScreenSettings()
    {
        switchScreen = MenuScreen.Settings;
    }

    public void SetScreenCredits()
    {
        switchScreen = MenuScreen.Credits;
    }

    public void DeselectAllSaves()
    {
        Image img1, img2, img3;

        if (saves[0].activeInHierarchy)
        {
            img1 = saveButton1.transform.GetChild(0).GetComponent<Image>();
        }
        else
        {
            img1 = newGames[0].GetComponent<Image>();
        }

        if (saves[1].activeInHierarchy)
        {
            img2 = saveButton2.transform.GetChild(0).GetComponent<Image>();
        }
        else
        {
            img2 = newGames[1].GetComponent<Image>();
        }

        if (saves[2].activeInHierarchy)
        {
            img3 = saveButton3.transform.GetChild(0).GetComponent<Image>();
        }
        else
        {
            img3 = newGames[2].GetComponent<Image>();
        }

        Color defaultColor = new Color(0.15f, 0.15f, 0.15f);

        img1.color = defaultColor;
        img2.color = defaultColor;
        img3.color = defaultColor;

        save1Animator.SetBool("asleep", true);
        sleepEffect.Play();
        save2Animator.runtimeAnimatorController = save2Unselected;
        save2Animator.gameObject.transform.rotation = new Quaternion(0, 0, 0.1f, 1);
        save3Animator.SetBool("throw", true);
        save3Animator.transform.position = new Vector3(save3Animator.transform.position.x,
            2.2672f);
    }

    public void SelectSaveState(int i)
    {
        if (!moving)
        {
            DeselectAllSaves();

            Image img1, img2, img3;

            if (saves[0].activeInHierarchy)
            {
                img1 = saveButton1.transform.GetChild(0).GetComponent<Image>();
            }
            else
            {
                img1 = newGames[0].GetComponent<Image>();
            }

            if (saves[1].activeInHierarchy)
            {
                img2 = saveButton2.transform.GetChild(0).GetComponent<Image>();
            }
            else
            {
                img2 = newGames[1].GetComponent<Image>();
            }

            if (saves[2].activeInHierarchy)
            {
                img3 = saveButton3.transform.GetChild(0).GetComponent<Image>();
            }
            else
            {
                img3 = newGames[2].GetComponent<Image>();
            }

            Color selectedColor = new Color(0, 0.7f, 1f);

            if (i == 1)
            {
                if (saves[0].activeInHierarchy)
                {
                    img1.color = selectedColor;
                    save1Animator.SetBool("asleep", false);
                    sleepEffect.Stop();

                    if (currentSelectedSave == i)
                    {
                        LoadGame();
                    }
                }
                else
                {
                    img1.color = selectedColor;
                }
            }
            else if (i == 2)
            {
                if (saves[1].activeInHierarchy)
                {
                    img2.color = selectedColor;
                    save2Animator.runtimeAnimatorController = save2Selected;
                    save2Animator.gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);

                    if (currentSelectedSave == i)
                    {
                        LoadGame();
                    }
                }
                else
                {
                    img2.color = selectedColor;
                }
            }
            else if (i == 3)
            {
                if (saves[2].activeInHierarchy)
                {
                    img3.color = selectedColor;
                    save3Animator.SetBool("throw", false);
                    save3Animator.transform.position = new Vector3(save3Animator.transform.position.x,
                        save3Animator.transform.position.y - 0.834f);

                    if (currentSelectedSave == i)
                    {
                        LoadGame();
                    }
                }
                else
                {
                    img3.color = selectedColor;
                }
            }

            currentSelectedSave = i;
        }
    }

    public void Toggle()
    {
        if (currentSelectedSave == 1)
        {
            SelectSaveState(2);
        }
        else if (currentSelectedSave == 2)
        {
            SelectSaveState(3);
        }
        else if (currentSelectedSave == 3)
        {
            SelectSaveState(1);
        }
        else
        {
            SelectSaveState(1);
        }
    }

    public void LoadGame()
    {
        if (currentSelectedSave > 0)
        {
            PlayerData data = SaveSystem.LoadGame(currentSelectedSave);
            var session = GameObject.Find("SessionData").GetComponent<SessionData>();
            
            session.pauseSession = true;
            session.timePlayed = data.timePlayed;
            session.currentLoadedSave = currentSelectedSave;
            SceneManager.LoadScene(data.scene);
        }
    }

    public void LoadGame(int i)
    {
        PlayerData data = SaveSystem.LoadGame(i);
        var session = GameObject.Find("SessionData").GetComponent<SessionData>();

        session.pauseSession = true;
        session.timePlayed = data.timePlayed;
        session.currentLoadedSave = i;
        SceneManager.LoadScene(data.scene);
    }

    public void DeleteSave()
    {
        if (confirm == null)
        {
            int i = -1;

            if (currentSelectedSave == 1)
            {
                i = 1;
            }
            else if (currentSelectedSave == 2)
            {
                i = 2;
            }
            else if (currentSelectedSave == 3)
            {
                i = 3;
            }

            string path = Application.persistentDataPath + "/TwoSpiritSavedData" + i + ".txt";

            if (File.Exists(path))
            {
                confirm = (GameObject)Instantiate(Resources.Load("Prefabs/UI/ConfirmationWindow"));
                confirm.GetComponent<ConfirmAction>().SetReasonDeleteSave(path, this);
            }
        }
    }

    public void ReloadSaves()
    {
        bool[] existentSaves = SaveSystem.ExistentSaves();
        int i = 0;

        foreach (GameObject save in saves)
        {
            save.SetActive(existentSaves[i]);

            if (!save.activeInHierarchy)
            {
                newGames[i].SetActive(true);
            }

            i++;
        }
    }

    public void ExitApp()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public enum MenuScreen
    {
        None,
        Main,
        Play,
        Settings,
        Credits
    }
}
