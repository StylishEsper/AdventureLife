using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public PlayerController player;
    public ParticleSystem castEffect;
    public ParticleSystem hellModeCastEffect;
    public ParticleSystem iceKingCastEffect;
    public ParticleSystem spellAvailableEffect;
    public ParticleSystem barFullEffect;

    public string spellType;

    public bool finishAnim;

    private GameObject castedSpell;
    private NPCController ghostKid;

    private SpellName spellName;

    private int cost;

    private float startTime;
    private float castTime;

    private bool startCasting;
    private bool enoughCost;
    private bool teleportFront;
    private bool unlearnedSpell;

    private void Start()
    {
        ghostKid = GetComponent<NPCController>();
        castEffect = Instantiate(castEffect);
        hellModeCastEffect = Instantiate(hellModeCastEffect);
        iceKingCastEffect = Instantiate(iceKingCastEffect);
    }

    private void Update()
    {
        spellType = player.currentElement;

        if (!player.attacking && !player.isDead)
        {
            if (!ghostKid.isCasting)
            {
                if (player.energy.slider.value == 100)
                {
                    if (!barFullEffect.isPlaying)
                    {
                        barFullEffect.Play();
                    }
                }
                else if (player.energy.slider.value != 100)
                {
                    if (barFullEffect.isPlaying)
                    {
                        barFullEffect.Clear();
                        barFullEffect.Stop();
                    }
                }

                if (player.energy.slider.value >= 25 && player.energy.slider.value != 100)
                { 
                    if (!spellAvailableEffect.isPlaying)
                    {
                        spellAvailableEffect.Play();
                    }
                }
                else if (player.energy.slider.value < 25 || player.energy.slider.value == 100)
                {
                    if (spellAvailableEffect.isPlaying)
                    {
                        spellAvailableEffect.Clear();
                        spellAvailableEffect.Stop();
                    }
                }

                cost = -1;

                if (player.controlEnabled)
                {
                    if (Input.GetKeyDown(KeyCode.O) || Input.GetMouseButtonDown(2))
                    {
                        startTime = Time.time;
                        unlearnedSpell = false;

                        if (spellType == "Water" && !player.disableHurricane)
                        {
                            spellName = SpellName.Hurricane;
                            teleportFront = true;
                        }
                        else if (spellType == "Fire" && !player.disableHellKid)
                        {
                            spellName = SpellName.SummonHellsMostWanted;
                            teleportFront = false;
                        }
                        else if (spellType == "Electric" && !player.disableThunderCloud)
                        {
                            spellName = SpellName.ThunderCloud;
                            teleportFront = true;
                        }
                        else if (spellType == "Ice" && !player.disableIceKid)
                        {
                            spellName = SpellName.SummonTheIceKing;
                            teleportFront = false;
                        }
                        else
                        {
                            spellName = SpellName.None;
                            unlearnedSpell = true;
                        }

                        cost = 75;
                    }                  
                    else if (Input.GetKeyDown(KeyCode.L) || Input.GetMouseButtonDown(1))
                    {
                        startTime = Time.time;
                        teleportFront = true;
                        unlearnedSpell = false;

                        if (spellType == "Water" && !player.disableWaterbolt)
                        {
                            spellName = SpellName.Waterbolt;
                        }
                        else if (spellType == "Fire" && !player.disableFirebolt)
                        {
                            spellName = SpellName.Firebolt;
                        }
                        else if (spellType == "Electric" && !player.disableThunderbolt)
                        {
                            spellName = SpellName.Thunderbolt;
                        }
                        else if (spellType == "Ice" && !player.disableIcebolt)
                        {
                            spellName = SpellName.Icebolt;
                        }
                        else
                        {
                            spellName = SpellName.None;
                            unlearnedSpell = true;
                        }

                        cost = 25;
                    }              
                }

                if (player.energy.slider.value >= cost && cost != -1 && !unlearnedSpell)
                {
                    enoughCost = true;

                    if (finishAnim)
                    {
                        ghostKid.animator.SetBool("casting", false);
                        finishAnim = false;
                    }

                    player.Command();
                    ghostKid.TeleportInfront(teleportFront);
                }

                if (startCasting && Time.time > castTime + 0.2085 && spellName != SpellName.None && enoughCost)
                {
                    ghostKid.animator.SetBool("casting", true);
                    string name = string.Empty;

                    if (spellName != SpellName.SummonHellsMostWanted && spellName != SpellName.SummonTheIceKing)
                    {
                        StartEffect(1);
                        castedSpell = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/" + spellName));
                        castedSpell.GetComponent<Absorbable>().energy = player.energy;

                        if (spellName == SpellName.Hurricane)
                        {
                            if (ghostKid.spriteRenderer.flipX)
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x - 0.7f,
                                    ghostKid.transform.position.y + 1.4f);
                            }
                            else
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x + 0.7f,
                                    ghostKid.transform.position.y + 1.4f);
                            }
                            name = "Hurricane";
                        }
                        else if (spellName == SpellName.Waterbolt)
                        {
                            if (ghostKid.spriteRenderer.flipX)
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x,
                                    ghostKid.transform.position.y);
                                castedSpell.GetComponent<Collider2D>().offset = new Vector2(-0.3f, 0.035f);
                            }
                            else
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x,
                                    ghostKid.transform.position.y);
                            }
                            name = "Waterbolt";
                        }
                        else if (spellName == SpellName.Firebolt)
                        {
                            if (ghostKid.spriteRenderer.flipX)
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x - 0.25f,
                                    ghostKid.transform.position.y + 0.1f);
                                castedSpell.GetComponent<Collider2D>().offset = new Vector2(-0.13f, -0.2f);
                            }
                            else
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x + 0.25f,
                                    ghostKid.transform.position.y + 0.1f);
                            }
                            name = "Firebolt";
                        }
                        else if (spellName == SpellName.Thunderbolt)
                        {
                            if (ghostKid.spriteRenderer.flipX)
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x - 0.25f,
                                    ghostKid.transform.position.y);
                                castedSpell.GetComponent<Collider2D>().offset = new Vector2(-0.02f, 0.03f);
                            }
                            else
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x + 0.25f,
                                    ghostKid.transform.position.y);
                            }
                            name = "Thunderbolt";
                        }
                        if (spellName == SpellName.ThunderCloud)
                        {
                            if (ghostKid.spriteRenderer.flipX)
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x - 0.7f,
                                    ghostKid.transform.position.y + 0.5f);
                            }
                            else
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x + 0.7f,
                                    ghostKid.transform.position.y + 0.5f);
                            }
                            name = "Thunder Cloud";
                        }
                        if (spellName == SpellName.Icebolt)
                        {
                            if (ghostKid.spriteRenderer.flipX)
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x - 0.6f,
                                    ghostKid.transform.position.y + 0.1f);
                            }
                            else
                            {
                                castedSpell.transform.position = new Vector2(ghostKid.transform.position.x + 0.6f,
                                    ghostKid.transform.position.y + 0.1f);
                            }
                            name = "Icebolt";
                        }
                        castedSpell.GetComponent<SpriteRenderer>().flipX = ghostKid.spriteRenderer.flipX;
                    }
                    else if (spellName == SpellName.SummonHellsMostWanted)
                    {
                        StartEffect(2);
                        player.HellMode();
                        name = "Summon: Hell's Most Wanted";
                    }
                    else if (spellName == SpellName.SummonTheIceKing)
                    {
                        StartEffect(3);
                        player.SummonTheIceKing();
                        name = "Summon: The Ice King";
                    }

                    GameObject popupText = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/PopupText"));
                    popupText.transform.position = new Vector3(player.transform.position.x,
                        player.transform.position.y + 0.4f);
                    TextPopUp textPopUp = popupText.GetComponent<TextPopUp>();
                    textPopUp.SetText(name);

                    if (spellType == "Water")
                    {
                        textPopUp.SetColor(0, 0.4f, 1);
                    }
                    else if (spellType == "Fire")
                    {
                        textPopUp.SetColor(1, 0, 0);
                    }
                    else if (spellType == "Electric")
                    {
                        textPopUp.SetColor(1, 1, 0);
                    }

                    textPopUp.SetElement(spellType, false);

                    cost = -1;
                    enoughCost = false;
                    startCasting = false;
                }
            }
        }

        if (player.isDead)
        {
            startCasting = false;
        }

        if (ghostKid.isCasting && Time.time > startTime + 0.5f)
        {
            ghostKid.isCasting = false;
            finishAnim = true;
        }

        if (ghostKid.animator.GetBool("casting"))
        {
            if (player.isKnockedBack && player.isCommanding)
            {
                startCasting = false;
                ghostKid.animator.SetBool("casting", false);
            }

            if (finishAnim && Time.time > startTime + 1f)
            {
                ghostKid.animator.SetBool("casting", false);
                finishAnim = false;
            }
        }
    }

    public void CastSpell()
    {
        startCasting = true;
        castTime = Time.time;
    }

    public void StartEffect(int castNumber)
    {
        if (castNumber == 1)
        {
            if (ghostKid.spriteRenderer.flipX)
            {
                castEffect.transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y);
                castEffect.transform.rotation = new Quaternion(0, -90, 0, 90);
            }
            else
            {
                castEffect.transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y);
                castEffect.transform.rotation = new Quaternion(0, 90, 0, 90);
            }

            castEffect.Play();
        }
        else if (castNumber == 2)
        {
            if (ghostKid.spriteRenderer.flipX)
            {
                hellModeCastEffect.transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y - 0.1f, -1);
                hellModeCastEffect.transform.GetChild(0).rotation = new Quaternion(0, 180, 0, 180);
            }
            else
            {
                hellModeCastEffect.transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y - 0.1f, -1);
                hellModeCastEffect.transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
            }

            hellModeCastEffect.Play();
        }
        else if (castNumber == 3)
        {
            if (ghostKid.spriteRenderer.flipX)
            {
                iceKingCastEffect.transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y - 0.1f, -1);
                iceKingCastEffect.transform.GetChild(0).rotation = new Quaternion(0, 180, 0, 180);
            }
            else
            {
                iceKingCastEffect.transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y - 0.1f, -1);
                iceKingCastEffect.transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
            }

            iceKingCastEffect.Play();
        }
    }

    public enum SpellName
    {
        None,
        Waterbolt,
        Hurricane,
        Firebolt,
        SummonHellsMostWanted,
        Thunderbolt,
        ThunderCloud,
        Icebolt,
        SummonTheIceKing
    }
}
