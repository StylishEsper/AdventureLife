using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using static Objective;
using UnityEngine;
using TMPro;

public class ObjectivesControl : MonoBehaviour
{
    public PlayerController player;
    public GameObject objectiveView;

    private List<GameObject> objectiveViews = new List<GameObject>();
    private RectTransform rect;
    private TextMeshProUGUI status;
    private TextMeshProUGUI title;
    private TextMeshProUGUI description;
    private TextMeshProUGUI count;


    private void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, 500);
        Populate();
    }

    public void ClearAllObjectives()
    {
        foreach (GameObject view in objectiveViews)
        {
            Destroy(view);
        }
    }

    public void Populate()
    {
        ClearAllObjectives();

        int y = -280;

        List<Objective> reversedList = player.objectives;
        reversedList.Reverse();

        foreach (Objective objective in reversedList)
        {
            GameObject o = Instantiate(objectiveView);
            objectiveViews.Add(o);
            o.transform.SetParent(gameObject.transform);
            RectTransform oRect = o.GetComponent<RectTransform>();
            oRect.localPosition = new Vector3(750, y, 0);
            oRect.localScale = new Vector3(11, 11, 1);
            TextMeshProUGUI status = oRect.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI title = oRect.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI description = oRect.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI count = oRect.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

            if (objective.complete)
            {
                status.SetText("COMPLETE");
            }
            else
            {
                status.SetText("INCOMPLETE");
            }

            if (objective.name == Name.FindTheCure)
            {
                title.SetText("Find the Cure");
                description.SetText("Your mom is ill, and the cure is said to only be available" +
                    " halfway across the world, in a dark place known as Hell's Gateway.");
            }
            else if (objective.name == Name.Witchcraft)
            {
                title.SetText("Witchcraft");
                description.SetText("The cure is too far away. But the local witch isn't. Find" +
                    " and ask if she can create it.");
            }

            if (objective.requiresCount)
            {
                count.SetText(objective.currentCount + "/" + objective.maxCount);
            }
            else
            {
                count.SetText("");
            }

            y -= 540;
            rect.sizeDelta = new Vector2(0, rect.sizeDelta.y + 250);
        }
    }
}
