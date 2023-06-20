using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
public class Light2DColorOptimizer : MonoBehaviour
{
    public Vector4 color;
    public float multiplier = 1;

    void Update()
    {
        var light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        light.color = new Color(color.x, color.y, color.z, color.w) * multiplier;
    }
}
