using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPopUp : MonoBehaviour
{
    public TextMeshPro textMesh;
    public Image criticalImage;
    public TextImageSetter imageSetter;

    public float speed;

    private Rigidbody2D rb;

    private float startTime;
    private float speedDecrement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
        speedDecrement = 0.025f;
    }

    private void Update()
    {
        rb.velocity = new Vector2(0, speed);

        if (Time.time > startTime + 0.05f && speed > 0)
        {
            speed -= speedDecrement;
        }
    }

    public void SetColor(float r, float g, float b)
    {
        textMesh.color = new Color(r, g, b);
    }

    public void SetText(string text)
    {
        textMesh.SetText(text);
    }

    public void Critical()
    {
        textMesh.fontSize *= 1.25f;
        criticalImage.enabled = true;
    }

    public void SetElement(string element, bool resistance)
    {
        imageSetter.SetImage(element, resistance);
    }
}
