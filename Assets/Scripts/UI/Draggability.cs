using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggability : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rect;

    private Vector2 offset;

    private bool dragging;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - offset;

            /*if (Input.mousePosition.x < Screen.width - 100 && Input.mousePosition.y < Screen.height - 50)
            {

            }*/
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}
