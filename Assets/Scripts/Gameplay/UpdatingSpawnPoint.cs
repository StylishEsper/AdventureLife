using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatingSpawnPoint : MonoBehaviour
{
    private GameObject player;
    private GameObject spawnPoint;
    private GameObject glow;
    private SpriteRenderer sr;
    [SerializeField] private Sprite onSprite;

    [SerializeField] private UpdateOn updateOn;

    private void Start()
    {
        player = GameObject.Find("Player");
        spawnPoint = GameObject.Find("PlayerSpawnPoint");
        sr = GetComponent<SpriteRenderer>();
        glow = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (updateOn == UpdateOn.XReached)
        {
            if (player.transform.position.x >= transform.position.x)
            {
                UpdateSpawnPoint();
            }
        }
        if (updateOn == UpdateOn.XAndYReached)
        {
            if (player.transform.position.x >= transform.position.x &&
                player.transform.position.y >= transform.position.y - 0.25f)
            {
                UpdateSpawnPoint();
            }
        }
    }

    private void UpdateSpawnPoint()
    {
        spawnPoint.transform.position = transform.position;
        sr.sprite = onSprite;
        glow.SetActive(true);
        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (updateOn == UpdateOn.Collision)
        {
            UpdateSpawnPoint();
        }
    }

    public enum UpdateOn
    {
        None,
        XReached,
        XAndYReached,
        Collision
    }
}
