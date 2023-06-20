using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositionChanger : MonoBehaviour
{
    public bool allowChange;
    public bool detected;

    private Vector3 originalPosition;
    private Vector3 alteredPosition;

    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;
    

    private void Start()
    {
        originalPosition = transform.position;
        alteredPosition = new Vector3(transform.position.x + x, transform.position.y + y,
            transform.position.z + z);
        allowChange = false;
    }

    private void Update()
    {
        if (detected && transform.position != alteredPosition)
        {
            transform.position = alteredPosition;
        }
        else if (!detected && transform.position != originalPosition)
        {
            transform.position = originalPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            detected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            detected = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        { 
            detected = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            detected = false;
        }
    }
}
