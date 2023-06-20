using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EntityElement;

public class ImageSetter : MonoBehaviour
{
    public PlayerController player;
    public EnemyVerification enemy;
    public Image image;
    public Sprite waterIcon;
    public Sprite fireIcon;
    public Sprite electricIcon;
    public Sprite iceIcon;
    public Sprite woodIcon;
    public Sprite windIcon;
    public Sprite poisonIcon;

    public bool isEnemy;

    private void Start()
    {

        if (isEnemy)
        {
            if (enemy.element == Element.Elementless)
            {
                image.enabled = false;
            }
            else
            {
                image.enabled = true;
            }

            if (image.enabled)
            {
                if (enemy.element == Element.Water)
                {
                    image.sprite = waterIcon;
                    transform.localScale = new Vector3(0.7f, 1f);
                }
                else if (enemy.element == Element.Fire)
                {
                    image.sprite = fireIcon;
                    transform.localScale = new Vector3(0.2f, 1f);
                }
                else if (enemy.element == Element.Electric)
                {
                    image.sprite = electricIcon;
                    transform.localScale = new Vector3(0.7f, 1f);
                }
                else if (enemy.element == Element.Ice)
                {
                    image.sprite = iceIcon;
                    transform.localScale = new Vector3(1f, 1f);
                }
                else if (enemy.element == Element.Wood)
                {
                    image.sprite = woodIcon;
                    transform.localScale = new Vector3(0.75f, 1f);
                }
                else if (enemy.element == Element.Wind)
                {
                    image.sprite = windIcon;
                    transform.localScale = new Vector3(1f, 1f);
                }
                else if (enemy.element == Element.Poison)
                {
                    image.sprite = poisonIcon;
                    transform.localScale = new Vector3(1f, 1f);
                }
            }
        }
    }

    private void Update()
    {
        if (!isEnemy)
        {
            if (player.hellModeOn)
            {
                image.sprite = fireIcon;
                image.enabled = true;
            }
            else if (player.iceKingOn)
            {
                image.sprite = iceIcon;
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }
    }
}
