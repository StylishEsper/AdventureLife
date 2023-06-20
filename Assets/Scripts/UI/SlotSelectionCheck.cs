using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotSelectionCheck : MonoBehaviour, IPointerEnterHandler
{
    public Button selfButton;

    public bool selected;
    public bool hovered;

    private GameObject inventory;
    private GameObject useButton;
    private GameObject dropButton;
    private GameObject dropSlider;
    private GameObject scrollBar;

    private bool check;

    private void Start()
    {
        inventory = GameObject.Find("Player").transform.GetChild(0).transform.GetChild(2).gameObject;
        useButton = GameObject.Find("Player").transform.GetChild(0).transform.GetChild(2).
            transform.GetChild(0).transform.GetChild(4).transform.GetChild(0).gameObject;
        dropButton = GameObject.Find("Player").transform.GetChild(0).transform.GetChild(2).
            transform.GetChild(0).transform.GetChild(4).transform.GetChild(1).gameObject;
        dropSlider = GameObject.Find("Player").transform.GetChild(0).transform.GetChild(2).
            transform.GetChild(0).transform.GetChild(4).transform.GetChild(2).gameObject;
        scrollBar = GameObject.Find("Player").transform.GetChild(0).transform.GetChild(2).
            transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current != null)
        {
            if (current == gameObject && inventory.activeInHierarchy && Input.GetMouseButtonDown(0))
            {
                if (!selected)
                {
                    selected = true;
                }
                else if (selected)
                {
                    selected = false;
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
            else if (current != gameObject && current != useButton && current != dropButton &&
                    current != dropSlider && current != scrollBar && 
                    current.gameObject.GetComponent<ItemInformationSetter>() == null)
            {
                selected = false;
            }
        }
        else
        {
            selected = false;
        }

        if (selected)
        {
            selfButton.Select();
        }

        if (check)
        {
            hovered = IsPointerOverUIObject();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        check = true;
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
 
        foreach (var s in results)
        {
            if (s.gameObject == gameObject)
            {
                return true;
            }
        }

        check = false;
        return false;
    }

    private void OnDisable()
    {
        selected = false;
    }
}
