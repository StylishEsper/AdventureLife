using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TextParticleSystem : MonoBehaviour
{
    private TextMeshPro textMeshPro;

    private ParticleSystem textParticleSystem;
    private ParticleSystemRenderer rendererSystem;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        textParticleSystem = GetComponent<ParticleSystem>();
        rendererSystem = textParticleSystem.GetComponent<ParticleSystemRenderer>();
        rendererSystem.mesh = textMeshPro.mesh;
    }
}
