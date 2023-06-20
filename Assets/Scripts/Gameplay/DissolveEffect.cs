using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    public Material defaultMaterial;

    public bool isOn = false;
    public bool destroyObjectOnDissolve;
    public bool destroySelfOnRevive;

    [SerializeField] private Material material;
    private SpriteRenderer sprite;
    private ParticleSystem dissolveParticle;
    private ParticleSystem reviveParticle;
    private PlayerController player;

    private float dissolveAmount;
    private float dissolveSpeed = 0.5f;
    private float waitTime = 2f;
    private float startTime;

    private bool isDissolving;
    private bool canRevive;
    private bool waitOn;


    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        material = (Material)Resources.Load("Material/Dissolvable");
        sprite.material = material;

        var p1 = (GameObject)Resources.Load("Prefabs/Effects/DissolveParticle");
        var p2 = (GameObject)Resources.Load("Prefabs/Effects/ReviveParticle");
        dissolveParticle = Instantiate(p1.GetComponent<ParticleSystem>());
        reviveParticle = Instantiate(p2.GetComponent<ParticleSystem>());
        dissolveParticle.transform.parent = gameObject.transform;
        reviveParticle.transform.parent = gameObject.transform;
        dissolveParticle.transform.localPosition = new Vector3(0f, -0.2f, -1f);
        reviveParticle.transform.localPosition = new Vector3(0.75f, 0.5f, -1f);
        material.SetFloat("_DissolveAmount", 0);
    }

    private void Update()
    {
        if (waitOn && Time.time >= startTime + waitTime)
        {
            isOn = true;
        }

        if (sprite.material != material)
        {
            sprite.material = material;
        }

        if (isOn)
        {
            if (isDissolving)
            {
                if (!dissolveParticle.isPlaying)
                {
                    dissolveParticle.Play();
                }

                dissolveAmount = Mathf.Clamp01(dissolveAmount + dissolveSpeed * Time.deltaTime);
                material.SetFloat("_DissolveAmount", dissolveAmount);

                if (dissolveAmount == 1)
                {
                    dissolveParticle.Stop();
                    isDissolving = false;

                    if (destroyObjectOnDissolve)
                    {
                        Destroy(dissolveParticle.gameObject, 1);
                        Destroy(reviveParticle.gameObject, 1);
                        Destroy(gameObject, 1);
                    }
                }
            }
            else if (canRevive)
            {
                if (!reviveParticle.isPlaying)
                {
                    dissolveParticle.Clear();
                    reviveParticle.Play();
                }

                dissolveAmount = Mathf.Clamp01(dissolveAmount - (dissolveSpeed + 0.45f) * Time.deltaTime);
                material.SetFloat("_DissolveAmount", dissolveAmount);

                if (dissolveAmount == 0)
                {
                    reviveParticle.Stop();
                    isDissolving = true;

                    if (destroySelfOnRevive)
                    {
                        End();
                    }
                }
            }
        }
    }

    public void End()
    {
        Destroy(dissolveParticle.gameObject, 1);
        Destroy(reviveParticle.gameObject, 1);
        sprite.material = defaultMaterial;
        Destroy(this);
    }

    public void SetDefaultForPlayer(Material defaultMaterial)
    {
        player = GetComponent<PlayerController>();
        this.defaultMaterial = defaultMaterial;
        dissolveAmount = 0;
        isDissolving = true;
        destroySelfOnRevive = true;
        canRevive = true;
        isOn = true;
    }

    public void SetDefaultForEnemy(Material defaultMaterial,bool canRevive)
    {
        this.defaultMaterial = defaultMaterial;
        dissolveAmount = 0;
        isDissolving = true;

        if (canRevive)
        {
            destroySelfOnRevive = true;
        }
        else
        {
            destroyObjectOnDissolve = true;
        }

        this.canRevive = canRevive;
        waitOn = true;
        startTime = Time.time;
    }

    public void StartDissolve(float dissolveSpeed)
    {
        isDissolving = true;
        this.dissolveSpeed = dissolveSpeed;
    }

    public void StopDissolve(float dissolveSpeed)
    {
        isDissolving = false;
        this.dissolveSpeed = dissolveSpeed;
    }
}
