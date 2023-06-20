using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private List<GameObject> additionalDestroy = new List<GameObject>(); //code it

    [SerializeField] private MemorableEvent memorableEvent;
    [SerializeField] private AddMethod addMethod;
    [SerializeField] private DestroyMethod destroyMethodOnStart;
    [SerializeField] private DestroyMethod destroyMethodOnUpdate;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        GameObject go = GameObject.Find("SessionData");

        if (go != null)
        {
            SessionData session = go.GetComponent<SessionData>();
            PlayerData data = SaveSystem.GetGameData(session.currentLoadedSave);

            foreach (MemorableEvent me in data.memorableEvents)
            {
                if (me == memorableEvent)
                {
                    if (me == MemorableEvent.Fos2BridgeCollapse)
                    {
                        GameObject.Find("Bridge").GetComponent<BridgeController>().BreakBridge();
                    }

                    UseStartDestroyMethod();
                }
            }
        }

        if (addMethod == AddMethod.Immediate)
        {
            player.memorableEvents.Add(memorableEvent);
        }
    }

    private void Update()
    {
        if (addMethod == AddMethod.OnXReached)
        {
            if (player.transform.position.x >= transform.position.x)
            {
                player.memorableEvents.Add(memorableEvent);

                if (memorableEvent == MemorableEvent.Fos2OldTravler)
                {
                    GetComponent<Selfdestruct>().isEnabled = true;
                    enabled = false;
                }
                else
                {
                    UseUpdateDestroyMethod();
                }
            }
        }
        else if (addMethod == AddMethod.OnYReached)
        {
            if (player.transform.position.y >= transform.position.y)
            {
                player.memorableEvents.Add(memorableEvent);
                UseUpdateDestroyMethod();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (addMethod == AddMethod.OnCollision)
            {
                player.memorableEvents.Add(memorableEvent);
            }
        }
    }

    public void UseStartDestroyMethod()
    {
        if (destroyMethodOnStart == DestroyMethod.OnlyScript)
        {
            this.enabled = false;
        }
        else if (destroyMethodOnStart == DestroyMethod.GameObject)
        {
            gameObject.SetActive(false);
        }


        int c = additionalDestroy.Count;

        for (int i = 0; i > c; i++)
        {
            additionalDestroy.RemoveAt(0);
        }
    }
    public void UseUpdateDestroyMethod()
    {
        if (destroyMethodOnUpdate == DestroyMethod.OnlyScript)
        {
            this.enabled = false;
        }
        else if (destroyMethodOnUpdate == DestroyMethod.GameObject)
        {
            gameObject.SetActive(false);
        }
    }

    public enum MemorableEvent
    {
        None,
        Fos2OldTravler,
        Fos2BridgeCollapse,
        BtfDoorOpened,
        BtfDoorOpened2
    }

    public enum AddMethod
    {
        None,
        Immediate,
        OnCollision,
        OnXReached,
        OnYReached
    }

    public enum DestroyMethod
    {
        None,
        OnlyScript,
        GameObject,
    }
}
