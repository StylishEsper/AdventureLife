using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArachnaCollisionDetector : MonoBehaviour
{
    [SerializeField] private Detector detector;
    [SerializeField] private ArachnaController arachna;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            if (detector == Detector.Bottom)
            {
                arachna.grounded = true;
            }
            else if (detector == Detector.Front)
            {
                arachna.somethingInFront = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            if (detector == Detector.Bottom)
            {
                arachna.grounded = true;
            }
            else if (detector == Detector.Front)
            {
                arachna.somethingInFront = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            if (detector == Detector.Bottom)
            {
                arachna.grounded = false;
            }
            else if (detector == Detector.Front)
            {
                arachna.somethingInFront = false;
            }
        }
    }

    public enum Detector
    {
        Bottom,
        Front
    }
}
