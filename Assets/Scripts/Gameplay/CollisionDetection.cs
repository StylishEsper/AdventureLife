using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public bool collisionDetected;
    public bool projectileDetection;

    void Start()
    {
        collisionDetected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!projectileDetection)
        {
            if (collision.tag != "Untagged")
            {
                collisionDetected = collision.gameObject != null;
            }
        }
        else
        {
            if (collision.tag == "Projectile")
            {
                collisionDetected = collision.gameObject != null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!projectileDetection)
        {
            if (collision.tag != "Untagged")
            {
                collisionDetected = collision.gameObject != null;
            }
        }
        else
        {
            if (collision.tag == "Projectile")
            {
                collisionDetected = collision.gameObject != null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionDetected = false;
    }
}
