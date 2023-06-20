using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    public Transform laserFirePoint;

    public FireDirection fireDirection;

    public bool endReached;

    private LineRenderer lineRenderer;

    [SerializeField] private float defDistanceRay = 100;
    [SerializeField] private float incrementValue;
    private float currValue;

    [SerializeField] private bool incrementToEnd;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        currValue = 0;
    }

    private void Update()
    {
        transform.position = laserFirePoint.position;
        Beam();
    }

    private void Beam()
    {
        RaycastHit2D h = Physics2D.Raycast(transform.position, transform.up);

        if (fireDirection == FireDirection.Down)
        {
            h = Physics2D.Raycast(transform.position, -transform.up);
        }

        if (h.collider.tag == "Wall")
        {
            float d = h.point.y - transform.position.y;
            Draw2DRay(d);
        }
        else
        {
            Draw2DRay();
        }
        
    }

    private void Draw2DRay()
    {
        lineRenderer.SetPosition(0, new Vector3(0, 0));

        if (fireDirection == FireDirection.Up)
        {
            if (incrementToEnd && currValue < defDistanceRay)
            {
                currValue += incrementValue;
                lineRenderer.SetPosition(1, new Vector3(0, currValue));
                endReached = false;
            }
            else
            {
                lineRenderer.SetPosition(1, new Vector3(0, defDistanceRay));
                endReached = true;
            }
        }
        else if (fireDirection == FireDirection.Down)
        {
            if (incrementToEnd && currValue > (defDistanceRay * -1))
            {
                currValue = -incrementValue;
                lineRenderer.SetPosition(1, new Vector3(0, currValue));
                endReached = false;
            }
            else
            {
                lineRenderer.SetPosition(1, new Vector3(0, defDistanceRay * -1));
                endReached = true;
            }
        }
    }

    private void Draw2DRay(float end)
    {
        lineRenderer.SetPosition(0, new Vector3(0, 0));

        if (fireDirection == FireDirection.Up)
        {
            if (incrementToEnd && currValue < end)
            {
                currValue += incrementValue;
                lineRenderer.SetPosition(1, new Vector3(0, currValue));
                endReached = false;
            }
            else
            {
                lineRenderer.SetPosition(1, new Vector3(0, end));
                endReached = true;
            }
        }
        else if (fireDirection == FireDirection.Down)
        {
            if (incrementToEnd && currValue > end)
            {
                currValue -= incrementValue;
                lineRenderer.SetPosition(1, new Vector3(0, currValue));
                endReached = false;
            }
            else
            {
                lineRenderer.SetPosition(1, new Vector3(0, end));
                endReached = true;
            }
        }
    }

    public enum FireDirection
    {
        Free,
        Up,
        Down,
        Left,
        Right
    }
}
