using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using static Item;
using Platformer.Model;
using Platformer.Core;
using System;
using static EntityElement;

namespace Platformer.Mechanics
{
    [Serializable]
    public class PlayerController : KinematicObject
    {
        /*internal new*/
        //public AudioSource audioSource;

        /*internal new*/
        public Collider2D collider2d;
        public Health health;
        public BuffLength buffLength;
        public ParticleSystem runEffect;
        public ParticleSystem floatEffect;
        public ParticleSystem jumpEffect;
        public ParticleSystem healthPotionEffect;
        public ParticleSystem antidoteEffect;
        public ParticleSystem extinguishEffect;
        public ParticleSystem defrostEffect;
        public ParticleSystem unparalyzeEffect;
        public ParticleSystem herbEffect;
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        public RuntimeAnimatorController hellKidAnimator;
        public RuntimeAnimatorController iceKidAnimator;
        public EnergyBar energy;
        public GameObject stab;
        public GameObject slash;
        public GameObject jumpSlash;
        public List<Item> obtainedItems = new List<Item>();
        public List<Objective> objectives = new List<Objective>();
        public SlotGenerator slotGenerator;
        public ObjectivesControl objectivesControl;
        public Rigidbody2D rb;

        public Vector2 move;

        public List<ObtainableItem.WorldItem> worldItems = new List<ObtainableItem.WorldItem>();
        public List<CutscenePlayer.CutsceneName> playedCutscenes = new List<CutscenePlayer.CutsceneName>();
        public List<GameEvent.MemorableEvent> memorableEvents = new List<GameEvent.MemorableEvent>();
        public JumpState jumpState = JumpState.Grounded;

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;
        public float speedBoost = 1;
        public float runSpeed = 1.35f;
        public Vector2 forceVelocity;
        public float attackTime;
        public float attackDelay;

        public int bonusHP;
        public int regenAmount;
        public int bonusAtk;
        public int bonusMgc;
        public int bonusCrit = 0;
        public int loadedSave;
        public int attacksCount;
        public int[] crystalUpgrades = new int[4];

        public string currentElement;

        public bool disableDataLoad;
        public bool isGhost;
        public bool disableAttack;
        public bool attacking;
        public bool invincible;
        public bool isDead;
        public bool isKnockedBack;
        public bool controlEnabled = true;
        public bool doubleSlashAvailable;
        public bool tripleSlashAvailable;
        public bool attackDelayOn;
        public bool hellModeOn;
        public bool iceKingOn;
        public bool isCommanding;
        public bool isPoisoned;
        public bool isBurned;
        public bool isFrozen;
        public bool isParalyzed;
        public bool disableWaterbolt;
        public bool disableHurricane;
        public bool disableFirebolt;
        public bool disableHellKid;
        public bool disableThunderbolt;
        public bool disableThunderCloud;
        public bool disableIcebolt;
        public bool disableIceKid;
        public bool darkJump;
        public bool[] equippedCrystals = new bool[4];

        private GameObject wallSlideEffect;
        private RuntimeAnimatorController adventureKidAnimator;
        private ParticleSystem hellTransformEffect;
        private ParticleSystem hellDetransformEffect;
        private ParticleSystem kingTransformEffect;
        private ParticleSystem kingDetransformEffect;
        private ParticleSystem walkEffect;
        private ParticleSystem frozenEffect;
        private ParticleSystem breakFreezeEffect;
        private ParticleSystem slowedEffect;
        private Invincibility invincibility;

        private WallDirection wallDirection;

        private float hellModeLength = 20f;
        private float iceKingLength = 15f;
        private float regenCooldown = 5;
        private float jumpBoost = 0.8f;
        private float gValue = 0.8f;
        private float wallJumpedTime;
        private float knockbackTime;
        private float commandTime;
        private float jumpAttackTime;
        private float hellModeTime;
        private float iceKingTime;
        private float regenStartTime;
        private float slowTime;

        private int lastAttackNumber;

        private string knockbackDirection;

        private bool isTouchingWall;
        private bool stopJump;
        private bool wallJumped = false;
        private bool oppositeForce = false;
        private bool onWall;
        private bool jump;
        private bool jumpAttackReady;
        private bool disableWallJump;
        private bool disableJumpSlice;
        private bool startCommandAttack;
        private bool isSlowed;
        private bool canForceMove;

        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private void Awake()
        {
            health = GetComponent<Health>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            runEffect = Instantiate(runEffect);
            floatEffect = Instantiate(floatEffect);
            jumpEffect = Instantiate(jumpEffect);
            stab.GetComponent<PlayerBasicAttack>().player = this;
            slash.GetComponent<PlayerBasicAttack>().player = this;
            jumpSlash.GetComponent<PlayerBasicAttack>().player = this;
            rb = GetComponent<Rigidbody2D>();
            invincibility = GetComponent<Invincibility>();

            hellTransformEffect = transform.GetChild(2).transform.GetChild(0).GetComponent<ParticleSystem>();
            hellDetransformEffect = transform.GetChild(2).transform.GetChild(1).GetComponent<ParticleSystem>();
            kingTransformEffect = transform.GetChild(2).transform.GetChild(2).GetComponent<ParticleSystem>();
            kingDetransformEffect = transform.GetChild(2).transform.GetChild(3).GetComponent<ParticleSystem>();
            walkEffect = transform.GetChild(2).transform.GetChild(4).GetComponent<ParticleSystem>();
            frozenEffect = transform.GetChild(2).transform.GetChild(5).GetComponent<ParticleSystem>();
            breakFreezeEffect = transform.GetChild(2).transform.GetChild(6).GetComponent<ParticleSystem>();
            slowedEffect = transform.GetChild(2).transform.GetChild(7).GetComponent<ParticleSystem>();

            adventureKidAnimator = animator.runtimeAnimatorController;
            attacking = false;
            doubleSlashAvailable = false;
            buffLength.Dissapear();
            slotGenerator.Start();
            regenStartTime = Time.time;
            attacksCount = 0;
            //FindAvailableSkills();

            if (!disableJumpSlice)
            {
                jumpAttackReady = true;
            }

            if (forceVelocity.x != 0 || forceVelocity.y != 0)
            {
                canForceMove = true;
            }
        }

        protected override void Start()
        {
            base.Start();

            if (!disableDataLoad)
            {
                var s = GameObject.Find("SessionData");
                if (s != null) loadedSave = s.GetComponent<SessionData>().currentLoadedSave;
                PlayerData data = SaveSystem.LoadGame(loadedSave);

                int i = 0;
                foreach (ItemName itemName in data.itemNames)
                {
                    Item newItem = new Item(itemName, data.slotNumbers[i]);
                    newItem.player = this;
                    newItem.inQuickSlot = data.inQuickSlots[i];

                    if (newItem.inQuickSlot)
                    {
                        newItem.quickSlotNumber = data.quickSlotNumbers[i];

                        if (data.quickSlotNumbers[i] == 1)
                        {
                            slotGenerator.quickCastSlot1.item = newItem;
                            slotGenerator.quickCastSlot1.currentStack++;
                            slotGenerator.quickCastSlot1.ReloadSlot();
                        }
                        else if (data.quickSlotNumbers[i] == 2)
                        {
                            slotGenerator.quickCastSlot2.item = newItem;
                            slotGenerator.quickCastSlot2.currentStack++;
                            slotGenerator.quickCastSlot2.ReloadSlot();
                        }
                        else if (data.quickSlotNumbers[i] == 3)
                        {
                            slotGenerator.quickCastSlot3.item = newItem;
                            slotGenerator.quickCastSlot3.currentStack++;
                            slotGenerator.quickCastSlot3.ReloadSlot();
                        }
                        else if (data.quickSlotNumbers[i] == 4)
                        {
                            slotGenerator.quickCastSlot4.item = newItem;
                            slotGenerator.quickCastSlot4.currentStack++;
                            slotGenerator.quickCastSlot4.ReloadSlot();
                        }
                    }

                    obtainedItems.Add(newItem);
                    i++;
                }

                objectives = data.objectives;
                objectivesControl.Populate();
                worldItems = data.worldItems;
                playedCutscenes = data.playedCutscenes;
                health.SetHealth(data.health);
                energy.SetEnergy(data.energy);
                transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
                currentElement = data.element;
                equippedCrystals = data.equippedCrystals;
                crystalUpgrades = data.crystalUpgrades;

                if (equippedCrystals.Length > 0)
                {
                    PowerCrystal lifeCrystal = slotGenerator.lifeCrystal.transform.GetChild(0).GetComponent<PowerCrystal>();
                    PowerCrystal forceCrystal = slotGenerator.forceCrystal.transform.GetChild(0).GetComponent<PowerCrystal>();
                    PowerCrystal magicCrystal = slotGenerator.magicCrystal.transform.GetChild(0).GetComponent<PowerCrystal>();
                    PowerCrystal luckCrystal = slotGenerator.luckCrystal.transform.GetChild(0).GetComponent<PowerCrystal>();
                    List<PowerCrystal> crystalList = new List<PowerCrystal>();
                    crystalList.Add(lifeCrystal);
                    crystalList.Add(forceCrystal);
                    crystalList.Add(magicCrystal);
                    crystalList.Add(luckCrystal);

                    int c = 0;
                    foreach (PowerCrystal crystal in crystalList)
                    {
                        if (equippedCrystals[c])
                        {
                            crystal.currentUpgrades = crystalUpgrades[c];
                            crystal.EnableCrystal();
                        }

                        c++;
                    }
                }
                else
                {
                    equippedCrystals = new bool[4] { false, false, false, false };
                    crystalUpgrades = new int[4] { 0, 0, 0, 0 };
                }
            }
        }

        protected override void Update()
        {
            Regeneration();

            if (controlEnabled)
            {
                if (isSlowed && Time.time >= slowTime + 3f)
                {
                    isSlowed = false;
                }

                if (!isCommanding)
                {
                    move.x = Input.GetAxis("Horizontal");
                }

                if (IsGrounded && wallDirection != WallDirection.Down && 
                    wallDirection != WallDirection.Up && !isTouchingWall)
                {
                    //inCorner = true;
                    if (Input.GetButton("Horizontal"))
                    {
                        isTouchingWall = true;
                    }
                }

                if (isTouchingWall && Input.GetButtonDown("Jump") && !wallJumped)
                {
                    animator.SetTrigger("offWall");
                    wallJumped = true;
                    isTouchingWall = false;
                    oppositeForce = true;
                    wallJumpedTime = Time.time;
                }

                if (oppositeForce && Time.time < wallJumpedTime + 0.1f && !isCommanding)
                {
                    if (wallDirection == WallDirection.Left)
                    {
                        move.x = 0;
                        move.x += 1;
                    }
                    if (wallDirection == WallDirection.Right)
                    {
                        move.x = 0;
                        move.x -= 1;
                    }
                }
                else if (oppositeForce && Time.time > wallJumpedTime + 0.1f)
                {
                    oppositeForce = false;
                }

                if (isTouchingWall && !attacking && !IsGrounded && !isCommanding)
                {
                    animator.SetTrigger("wallHold");
                    gravityModifier = 0.1f;

                    if (velocity.y < 0)
                    {
                        velocity.y = -1;
                    }

                    if (runEffect != null)
                    {
                        speedBoost = 1;
                        runEffect.Stop();
                    }
                }
                else
                {
                    animator.SetTrigger("offWall");
                    gravityModifier = gValue;
                }

                if (isTouchingWall && Input.GetButtonUp("Horizontal"))
                {
                    animator.SetTrigger("offWall");
                    isTouchingWall = false;
                }

                if (IsGrounded)
                {
                    animator.SetTrigger("offWall");
                    isTouchingWall = false;
                }

                Running();

                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump") && !isCommanding)
                {
                    jumpState = JumpState.PrepareToJump;
                }
                else if (Input.GetButtonUp("Jump") && !darkJump)
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }

                if (IsGrounded && move.x != 0 && !walkEffect.isPlaying)
                {
                    walkEffect.Play();
                }               
                else if (!IsGrounded || move.x == 0)
                {
                    if (walkEffect.isPlaying)
                    {
                        walkEffect.Stop();
                    }
                }

                BasicAttacks();

                if (hellModeOn && Time.time >= hellModeTime + hellModeLength)
                {
                    hellModeOn = false;
                    hellDetransformEffect.Play();
                    buffLength.Dissapear();
                    health.entityElement = Element.Elementless;
                    animator.runtimeAnimatorController = adventureKidAnimator;
                    isCommanding = false;
                }

                if (iceKingOn && Time.time >= iceKingTime + iceKingLength)
                {
                    iceKingOn = false;
                    kingDetransformEffect.Play();
                    buffLength.Dissapear();
                    health.entityElement = Element.Elementless;
                    animator.runtimeAnimatorController = adventureKidAnimator;
                    isCommanding = false;
                }

                if (wallSlideEffect != null && !isTouchingWall)
                {
                    Destroy(wallSlideEffect);
                }

                if (onWall && !darkJump)
                {
                    if (IsGrounded && wallDirection == WallDirection.Down && Input.GetButtonDown("Jump"))
                    {
                        jumpEffect.Play();
                    }

                    if (!IsGrounded && wallDirection != WallDirection.Down && Input.GetButtonDown("Jump") && oppositeForce)
                    {
                        jumpEffect.Play();
                    }

                    if (jumpState == JumpState.Landed)
                    {
                        jumpEffect.Play();
                    }
                }
            }
            else
            {
                runEffect.Stop();
                walkEffect.Stop();
                Destroy(wallSlideEffect);
                attackDelay = 0;

                onWall = false;
                gravityModifier = gValue;
                move.x = 0;

                base.Update();
            }

            if (isKnockedBack && !isDead)
            {
                if (Time.time >= knockbackTime + 0.4f)
                {
                    if (!isFrozen && !isParalyzed)
                    {
                        controlEnabled = true;
                    }

                    isKnockedBack = false;
                    knockbackDirection = "";
                }

                if (knockbackDirection == "left")
                {
                    move.x -= 1;
                }
                else if (knockbackDirection == "right")
                {
                    move.x += 1;
                }

                base.Update();
            }

            if (isCommanding)
            {
                if (startCommandAttack)
                {
                    slash.GetComponent<PlayerBasicAttack>().attackName = "CommandAttack";
                    Instantiate(slash);
                    startCommandAttack = false;
                }

                if (Time.time >= commandTime + 0.5f)
                {
                    animator.SetBool("commanding", false);
                    isCommanding = false;
                }
            }

            if (buffLength.gameObject.activeInHierarchy)
            {
                buffLength.SetLength(buffLength.slider.value - Time.deltaTime);
            }

            UpdateJumpState();
        }

        public void Regeneration()
        {
            if (!isDead && regenAmount > 0 && health.healthBar.slider.value != health.healthBar.slider.maxValue)
            {
                if (Time.time >= regenStartTime + regenCooldown)
                {
                    health.Increment(regenAmount, false, false);
                    regenStartTime = Time.time;
                }
            }
        }

        public void BasicAttacks()
        {
            base.Update(); //allows turning inbetween attacks
            if (!disableAttack)
            {
                if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.S) && attacking)
                {
                    attacksCount = Mathf.Clamp(attacksCount + 1, 0, 2);
                }

                if (!jumpAttackReady && Time.time >= jumpAttackTime + 0.5f)
                {
                    if (!disableJumpSlice)
                    {
                        jumpAttackReady = true;
                    }
                }

                if (attackDelayOn && Time.time >= attackTime + attackDelay || lastAttackNumber == 4 &&
                    Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.S))
                {
                    attackDelayOn = false;
                }

                if (!attacking && !isCommanding && !attackDelayOn)
                {
                    if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.S) &&
                        jumpAttackReady && !IsGrounded)
                    {
                        attacking = true;
                        jumpAttackReady = false;
                        jumpAttackTime = Time.time;
                        var temp = Instantiate(jumpSlash);
                        temp.SetActive(true);
                        lastAttackNumber = 5;
                    }
                    else if (Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.S))
                    {
                        attacking = true;
                        var temp = Instantiate(stab);
                        temp.SetActive(true);
                        lastAttackNumber = 1;
                    }
                    else if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.S))
                    {
                        if (!doubleSlashAvailable && !tripleSlashAvailable)
                        {
                            attacking = true;
                            slash.GetComponent<PlayerBasicAttack>().attackName = "Slash";
                            var temp = Instantiate(slash);
                            temp.SetActive(true);
                            lastAttackNumber = 2;
                        }
                    }

                    if (attacksCount > 0 && !attacking && lastAttackNumber == 2)
                    {
                        attacking = true;
                        slash.GetComponent<PlayerBasicAttack>().attackName = "Slash2";
                        var temp = Instantiate(slash);
                        temp.SetActive(true);
                        lastAttackNumber = 3;
                    }
                    else if (attacksCount > 1 && !attacking && lastAttackNumber == 3)
                    {
                        attacking = true;
                        slash.GetComponent<PlayerBasicAttack>().attackName = "Slash3";
                        var temp = Instantiate(slash);
                        temp.SetActive(true);
                        lastAttackNumber = 4;

                    }
                }
                else if (attacking && !isCommanding)
                {
                    if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.S) && lastAttackNumber == 2)
                    {
                        doubleSlashAvailable = true;
                        attackDelay = 0;
                    }
                    else if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.S) && lastAttackNumber == 3)
                    {
                        tripleSlashAvailable = true;
                        attackDelay = 0;
                    }
                }
            }
        }

        public void Running()
        {
            if (move.x != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (!Input.GetKeyUp(KeyCode.LeftShift) && !Input.GetKeyUp(KeyCode.RightShift))
                    {
                        if (speedBoost == 1f)
                        {
                            speedBoost = runSpeed;
                            runEffect.Play();
                        }
                    }
                }
                else
                {
                    if (speedBoost == runSpeed)
                    {
                        speedBoost = 1f;
                        runEffect.Stop();
                    }
                }
            }
            else if (!Input.GetButton("Horizontal"))
            {
                if (speedBoost == runSpeed)
                {
                    speedBoost = 1f;
                    runEffect.Stop();
                }
            }

            if (runEffect != null)
            {
                if (spriteRenderer.flipX)
                {
                    Quaternion quaternion = new Quaternion(0, 0, 0, 0);
                    runEffect.transform.rotation = quaternion;
                    runEffect.transform.position = new Vector3(transform.position.x,
                        transform.position.y, 2);
                }
                else
                {
                    Quaternion quaternion = new Quaternion(0, 180, 0, 0);
                    runEffect.transform.rotation = quaternion;
                    runEffect.transform.position = new Vector3(transform.position.x,
                        transform.position.y, 2);
                }
            }
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded || wallJumped)
            {
                if (!wallJumped)
                {
                    velocity.y = jumpTakeOffSpeed * (model.jumpModifier * jumpBoost);
                }
                else if (!isTouchingWall)
                {
                    velocity.y = jumpTakeOffSpeed * model.jumpModifier - 1;
                }
                jump = false;
                wallJumped = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }
            velocity.y += forceVelocity.y;

            if (velocity.y < 0 && !IsGrounded)
            {
                animator.SetBool("falling", true);
            }
            else
            {
                animator.SetBool("falling", false);
            }

            if (move.x > 0.01f && !isKnockedBack && !attacking)
            {
                spriteRenderer.flipX = false;
            }
            else if (move.x < -0.01f && !isKnockedBack && !attacking)
            {
                spriteRenderer.flipX = true;
            }

            if (isCommanding)
            {
                velocity.y = 0f;
                jumpState = JumpState.Grounded;
            }

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            
            if (canForceMove && controlEnabled)
            {
                move.x += forceVelocity.x;

                if (move.x == forceVelocity.x && !Input.GetButtonDown("Horizontal"))
                {
                    animator.SetFloat("velocityX", 0);
                }
            }

            targetVelocity = move * (maxSpeed * speedBoost);

            if (isSlowed)
            {
                targetVelocity = targetVelocity / 2;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            RunOnCollision(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            RunOnCollision(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            isTouchingWall = false;
            groundNormal = new Vector2(0, 1);
            animator.SetTrigger("offWall");
        }

        public void RunOnCollision(Collision2D collision)
        {
            var collisionPoint = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

            if (collision.gameObject.tag == "Slope")
            {
                wallDirection = WallDirection.Down;
            }

            if (collision.gameObject.tag == "Monster" && IsGrounded)
            {
                wallDirection = WallDirection.None;
            }

            if (collision.gameObject.tag != "DarkEnergy")
            {
                darkJump = false;
            }

            if (collision.gameObject.tag == "Wall")
            {
                onWall = true;

                wallDirection = WallDirection.None;

                if (collisionPoint.y < transform.position.y && IsGrounded)
                {
                    darkJump = false;
                    animator.SetTrigger("offWall");
                    isTouchingWall = false;
                    wallDirection = WallDirection.Down;
                }
                else if (collisionPoint.y > transform.position.y && !IsGrounded && move.y == 0 && velocity.y > 0)
                {
                    animator.SetTrigger("offWall");
                    isTouchingWall = false;
                    wallDirection = WallDirection.Up;
                }

                if (!disableWallJump)
                {
                    if (collisionPoint.x < transform.position.x && wallDirection != WallDirection.Down
                        && wallDirection != WallDirection.Up)
                    {
                        wallDirection = WallDirection.Left;

                        if (Input.GetButton("Horizontal") && !attacking)
                        {
                            isTouchingWall = true;

                            if (wallSlideEffect == null)
                            {
                                wallSlideEffect = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/WallSlideEffect"));
                            }

                            wallSlideEffect.transform.position = collisionPoint;

                            if (velocity.y > 0)
                            {
                                velocity.y = 0;
                            }
                        }
                    }
                    else if (collisionPoint.x > transform.position.x && wallDirection != WallDirection.Down
                        && wallDirection != WallDirection.Up)
                    {
                        wallDirection = WallDirection.Right;

                        if (Input.GetButton("Horizontal") && !attacking)
                        {
                            isTouchingWall = true;

                            if (wallSlideEffect == null)
                            {
                                wallSlideEffect = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/WallSlideEffect"));
                            }

                            wallSlideEffect.transform.position = collisionPoint;

                            if (velocity.y > 0)
                            {
                                velocity.y = 0;
                            }
                        }
                    }
                }

                if (wallDirection == WallDirection.Down || wallDirection == WallDirection.Up)
                {
                    jumpEffect.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    jumpEffect.transform.rotation = new Quaternion(90, 90, 90, 90);
                }

                if (wallDirection != WallDirection.Up)
                {
                    jumpEffect.transform.position = collisionPoint;
                }
            }
            else if (collision.gameObject.tag != "Wall")
            {
                onWall = false;
            }
        }

        public void Invincible()
        {
            invincibility.Invincible();
        }

        public void Invincible(float bTime)
        {
            invincibility.Invincible(1f);
        }

        public void Knockback(string direction)
        {
            knockbackTime = Time.time;
            controlEnabled = false;

            if (isCommanding)
            {
                isCommanding = false;
                animator.SetBool("commanding", false);
            }

            isKnockedBack = true;
            knockbackDirection = direction;
            attacking = false;
            doubleSlashAvailable = false;
            tripleSlashAvailable = false;
            attacksCount = 0;
        }

        public void HellMode()
        {
            energy.Cost(75);
            hellTransformEffect.Play();

            if (!hellModeOn)
            {
                isCommanding = false;
            }

            if (iceKingOn)
            {
                iceKingOn = false;
                kingDetransformEffect.Play();
            }

            hellModeOn = true;
            hellModeTime = Time.time;
            buffLength.Appear();
            buffLength.SetMaxLength(hellModeLength);
            health.entityElement = Element.Fire;
            animator.runtimeAnimatorController = hellKidAnimator;
        }

        public void SummonTheIceKing()
        {
            energy.Cost(75);
            kingTransformEffect.Play();

            if (!iceKingOn)
            {
                isCommanding = false;
            }

            if (hellModeOn)
            {
                hellModeOn = false;
                hellDetransformEffect.Play();
            }

            iceKingOn = true;
            iceKingTime = Time.time;
            buffLength.Appear();
            buffLength.SetMaxLength(iceKingLength);
            health.entityElement = Element.Ice;
            animator.runtimeAnimatorController = iceKidAnimator;
        }

        public void Command()
        {
            if (!attacking && !isCommanding)
            {
                if (!IsGrounded)
                {
                    floatEffect.transform.position = new Vector3(transform.position.x,
                        transform.position.y - 0.4f);
                    floatEffect.Play();
                }

                startCommandAttack = true;
                Destroy(wallSlideEffect);
                move.x = 0;
                animator.SetBool("commanding", true);
                isCommanding = true;
                commandTime = Time.time;
            }
        }

        public void PickUpItem(Item item, ObtainableItem.WorldItem worldItem)
        {
            if (worldItem != ObtainableItem.WorldItem.None)
            {
                worldItems.Add(worldItem);
            }

            item.player = this;
            obtainedItems.Add(item);
            slotGenerator.Start();

            if (!item.visibleInInventory)
            {
                FindAvailableSkills();
            }
        }

        public void DropItems(int amount, int slot)
        {
            for (int i = 0; i < amount; i++)
            {
                int index = 0;
                foreach (Item item in obtainedItems)
                {
                    if (item.slotNumber == slot)
                    {
                        obtainedItems.RemoveAt(index);
                        break;
                    }

                    index++;
                }
            }

            slotGenerator.Start();
        }

        public void Freeze()
        {
            isFrozen = true;
            frozenEffect.Play();
            controlEnabled = false;
        }

        public void SlowDown()
        {
            isSlowed = true;
            slowTime = Time.time;
            slowedEffect.Play();
        }

        public void Defrost()
        {
            isFrozen = false;
            frozenEffect.Clear();
            frozenEffect.Stop();
            breakFreezeEffect.Play();
            animator.speed = 1;

            if (!isDead)
            {
                controlEnabled = true;
            }

            if (isKnockedBack)
            {
                isKnockedBack = false;
            }
        }

        public void FindAvailableSkills()
        {
            disableWallJump = true;
            disableJumpSlice = true;
            disableWaterbolt = true;
            disableHurricane = true;
            disableFirebolt = true;
            disableHellKid = true;
            disableThunderbolt = true;
            disableThunderCloud = true;
            disableIcebolt = true;
            disableIceKid = true;

            foreach (Item item in obtainedItems)
            {
                if (!item.visibleInInventory)
                {
                    if (item.itemName == ItemName.WayOfTheSpider)
                    {
                        disableWallJump = false;
                    }
                    else if (item.itemName == ItemName.SkillOfASwordsman)
                    {
                        disableJumpSlice = false;
                    }
                    else if (item.itemName == ItemName.FlowOfTheRiver)
                    {
                        disableWaterbolt = false;
                    }
                    else if (item.itemName == ItemName.RageOfTheOcean)
                    {
                        disableHurricane = false;
                    }
                    else if (item.itemName == ItemName.BreathOfADragon)
                    {
                        disableFirebolt = false;
                    }
                    else if (item.itemName == ItemName.HellsMostWanted)
                    {
                        disableHellKid = false;
                    }
                    else if (item.itemName == ItemName.RoarOfThunder)
                    {
                        disableThunderbolt = false;
                    }
                    else if (item.itemName == ItemName.GiftFromTheStorm)
                    {
                        disableThunderCloud = false;
                    }
                    else if (item.itemName == ItemName.PinnacleOfWinter)
                    {
                        disableIcebolt = false;
                    }
                    else if (item.itemName == ItemName.TheIceKing)
                    {
                        disableIceKid = false;
                    }
                }
            }
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        public enum WallDirection
        {
            None,
            Up,
            Down,
            Left,
            Right
        }
    }
}