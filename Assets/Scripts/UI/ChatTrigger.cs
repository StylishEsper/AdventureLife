using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject triggerer;
    private new Collider2D collider;

    [SerializeField] private Chat chat;

    private float viewTime;

    private string text;

    [SerializeField] private bool triggerByView;
    [SerializeField] private bool isCharacter;
    private bool requiresCollision;
    private bool inView;

    private void Start()
    {
        //check if player already triggered it, if so, disable GO

        collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            requiresCollision = true;
        }

        if (!requiresCollision && triggerer.transform.position.x >= transform.position.x)
        {
            WontSayItTwice();
        }

        SetText();
    }

    private void Update()
    {
        if (!requiresCollision && !triggerByView)
        {
            if (triggerer.transform.position.x >= transform.position.x)
            {
                WillSayItOnce();
            }
        }

        if (inView)
        {
            if (Time.time >= viewTime + 3.5f)
            {
                WillSayItOnce();
                inView = false;
            }
        }
    }

    private void OnBecameVisible()
    {
        if (triggerByView)
        {
            inView = true;
            viewTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == triggerer.gameObject)
        {
            if (requiresCollision)
            {
                WillSayItOnce();
            }
        }
    }

    public void WillSayItOnce()
    {
        foreach (Transform t in target.transform)
        {
            var d = t.gameObject.GetComponent<DialogController>();
            if (d != null)
            {
                d.SetText(text);
                break;
            }
        }

        WontSayItTwice();
    }

    public void WontSayItTwice()
    {
        if (!isCharacter)
        {
            gameObject.SetActive(false);
        }
        else
        {
            this.enabled = false;
        }
    }

    public void SetText()
    {
        if (chat == Chat.FirstJump)
        {
            text = "Go ahead, jump.";
        }
        else if (chat == Chat.SeeDarkness)
        {
            text = "See that darkness below? Don't fall into it. If you do, you'll be sent back to the " +
                "nearest Lightstone.";
        }
        else if (chat == Chat.TooMany)
        {
            text = "There's too many of them. Let's find another way.";
        }
        else if (chat == Chat.OldTravelerForeshadow)
        {
            text = "Careful now, sonny boy. It's suddenly so windy...";
        }
    }

    public enum Chat
    {
        None,
        FirstJump,
        SeeDarkness,
        TooMany,
        OldTravelerForeshadow
    }
}
