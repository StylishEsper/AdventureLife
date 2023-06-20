using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public GameObject dialogBox;
    public TextMeshProUGUI text;

    public bool talking;

    private float letterTime;
    private float finishedTime;

    private int index;

    private string completeText;
    private string currentText;

    private bool startRemoval;

    private void Start()
    {
        SetVisibility(false);
        talking = false;
        startRemoval = false;
    }

    private void Update()
    {
        if (talking && !startRemoval)
        {
            if (Time.time >= letterTime + 0.05)
            {
                currentText += completeText[index].ToString();
                text.SetText(currentText);
                letterTime = Time.time;
                index++;
            }

            if (index == completeText.Length)
            {
                startRemoval = true;
                finishedTime = Time.time;
            }
        }

        if (startRemoval && Time.time >= finishedTime + 2.5f || Input.GetKeyDown(KeyCode.P))
        {
            talking = false;
            startRemoval = false;
            currentText = string.Empty;
            text.SetText(string.Empty);
            SetVisibility(false);
        }
    }

    public void SetVisibility(bool visible)
    {
        dialogBox.SetActive(visible);
    }

    public void SetText(string text)
    {
        SetVisibility(true);
        completeText = text;
        talking = true;
        letterTime = Time.time;
        currentText = string.Empty;
        index = 0;
    }
}
