using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIEffects : MonoBehaviour
{
    public PlayerController player;

    public float right;
    public float left;

    private float originalX;
    private float originalY;
    private float switchTime;

    private bool moveRight;

    private void Start()
    {
        transform.position = new Vector3(329.5f, 953.4f, 0.0f);
        originalX = transform.position.x;
        originalY = transform.position.y;
        moveRight = true;
        switchTime = Time.time;
    }

    private void Update()
    {
        if (player.isKnockedBack && !player.isDead)
        {
            if (moveRight && Time.time > switchTime + 0.05)
            {
                transform.position = new Vector3(transform.position.x + right, transform.position.y);
                moveRight = false;
                switchTime = Time.time;
            }
            else if (!moveRight && Time.time > switchTime + 0.05)
            {
                transform.position = new Vector3(transform.position.x - left, transform.position.y);
                moveRight = true;
                switchTime = Time.time;
            }

        }
        else
        {
            transform.position = new Vector3(originalX, originalY);
        }
    }
}
