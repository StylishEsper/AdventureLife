using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutscenePlayer : MonoBehaviour
{
    private PlayerController player;
    private PlayableDirector playable;
    [SerializeField] private GameObject skipObject;

    [SerializeField] private CutsceneName cutscene;

    private float startTime;

    [SerializeField] private bool playOnCollision;
    [SerializeField] private bool playOnXReached;
    [SerializeField] private bool skippable = true;
    [SerializeField] private bool requiresTimeline = true;
    private bool hasPlayed;
    private bool playing;

    private void Awake()
    {
        if (cutscene == CutsceneName.None)
        {
            gameObject.SetActive(false);
        }

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        playable = GetComponent<PlayableDirector>();
        hasPlayed = false;

        if (playable != null)
        {
            if (playable.playOnAwake)
            {
                playing = true;
                startTime = Time.time;
            }
        }

        PlayerData data = SaveSystem.GetGameData(
            GameObject.Find("SessionData").GetComponent<SessionData>().currentLoadedSave);

        foreach (CutsceneName name in data.playedCutscenes)
        {
            if (name == cutscene)
            {
                hasPlayed = true;

                if (name == CutsceneName.BridgeCollapse)
                {
                    GameObject.Find("Bridge").GetComponent<BridgeController>().BreakBridge();
                }

                gameObject.SetActive(false);
                break;
            }
        }

        if (!hasPlayed && skippable && requiresTimeline)
        {
            skipObject = Instantiate(skipObject);
            skipObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(SkipCutscene);
        }   
    }

    private void Update()
    {
        if (playing && requiresTimeline)
        {
            if (Time.time >= startTime + playable.duration)
            {
                player.playedCutscenes.Add(cutscene);
                Destroy(skipObject);
                playing = false;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                SkipCutscene();
            }
        }

        if (!playing && playOnXReached)
        {
            if (player.transform.position.x >= transform.position.x)
            {
                hasPlayed = true;
                playable.Play();
                playing = true;
                startTime = Time.time;
            }
        }
    }

    public void SkipCutscene()
    {
        Destroy(skipObject);
        player.playedCutscenes.Add(cutscene);
        playing = false;
        playable.initialTime = playable.duration;
        playable.Stop();
        playable.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && requiresTimeline)
        {
            if (collision.tag == "Player")
            {
                if (playOnCollision)
                {
                    hasPlayed = true;
                    playable.Play();
                    playing = true;
                    startTime = Time.time;
                }
            }
        }
    }

    public enum CutsceneName
    {
        None,
        Awake,
        HowIsShe,
        PotionHelp,
        BackToRoom,
        FirstTimeOutside,
        Fos2OldTraveler,
        BridgeCollapse
    }
}
