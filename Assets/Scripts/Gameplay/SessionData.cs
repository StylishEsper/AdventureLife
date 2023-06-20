using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionData : MonoBehaviour
{
    public string scene;

    public float timePlayed;

    public int currentLoadedSave;

    public bool pauseSession;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        timePlayed = 0;
    }

    private void Update()
    {
        if (!pauseSession)
        {
            if (scene != "MainMenu" && scene != string.Empty)
            {
                timePlayed += Time.deltaTime;
                string time = TimeSpan.FromSeconds(timePlayed).ToString("hh':'mm':'ss");
                Debug.Log(time);
            }

            scene = SceneManager.GetActiveScene().name;

            if (scene == "MainMenu" && currentLoadedSave != 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (scene != SceneManager.GetActiveScene().name)
            {
                pauseSession = false;
            }
        }
    }
}
