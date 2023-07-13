using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatStateManager : MonoBehaviour
{
    //public GameObject otherPlayer;
    //public CombatStateManager p1Manager;
    //public CombatStateManager p2Manager;
    //public CombatStateManager p3Manager;
    //public CombatStateManager p4Manager;
    public string currentStateString;

    public List<CombatStateManager> allPlayers = new List<CombatStateManager>();
    public CombatStateManager playerAttackingYouManager;
    public CurrencyManager currencyManager;

    public List<CombatStateManager> PlayersYouAreAttacking()
    {
        List<CombatStateManager> list = new List<CombatStateManager>();

        for (int i = 0; i < allPlayers.Count; i++)
        {
            if (allPlayers[i].playerAttackingYouManager == this)
            {
                list.Add(allPlayers[i]);
            }
        }

        return list;
    }

    // use this to disconnect yourself from the player attacking you (AKA, once they already have all the info they need from you)
    public void ResetPlayerAttackingYou()
    {
        playerAttackingYouManager = null;
    }

    public PlayerMovement playerMovement;
    
    public string bufferString = "";
    public Dictionary<string, CombatBaseState> bufferDictionary = new Dictionary<string, CombatBaseState>();
    public float bufferSize;
    //public Dictionary<UnityEngine.InputSystem.Controls.ButtonControl, string> bufferInputToString;

    public CombatBaseState currentState;
    public IdleState IdleState = new IdleState();
    public LightAttackState LightAttackState = new LightAttackState();
    public HeavyAttackState HeavyAttackState = new HeavyAttackState();
    public DashState DashState = new DashState();
    public SplatterState SplatterState = new SplatterState();
    public ShieldState ShieldState = new ShieldState();
    public ShieldStunState ShieldStunState = new ShieldStunState();
    public GrabbedState GrabbedState = new GrabbedState();
    public GrabState GrabState = new GrabState();
    public HoldState HoldState = new HoldState();
    public ThrowState ThrowState = new ThrowState();
    public HitstunState HitstunState = new HitstunState();
    public DyingState DyingState = new DyingState();
    public DeadState DeadState = new DeadState();
    public RespawnState RespawnState = new RespawnState();
    public PipeState PipeState = new PipeState();
    //public SpecialState SpecialState = new SpecialState();

    public SpriteRenderer circleSprite;
    public Rigidbody2D rb;
    public Collider2D mainCollider;
    public GameObject invulnerableCollider;
    public Transform playerSpriteTargetTransform;
    public PlayerSpriteAnim playerSpriteAnim;
    public SpriteRenderer playerSpriteRenderer;
    public GameObject splatterSpriteAnim;

    // attack hitboxes & variables
    public bool attackTimerStuck;

    // light attack
    public GameObject[] lightAttackHitbox;
    public int curLightAttackHitbox;


    public float lightAttackStartup;
    public float lightAttackActiveHitboxDuration;
    public float lightAttackEndLag;
    public float lightAttackDuration;
    public float lightAttackDamage;
    public float lightAttackDamageBonus;

    // heavy attack
    public GameObject[] heavyAttackHitbox;
    public int curHeavyAttackHitbox;


    public float heavyAttackStartup;
    public float heavyAttackActiveHitboxDuration;
    public float heavyAttackEndLag;
    public float heavyAttackDuration;
    public float heavyAttackDamage;
    public float heavyAttackDamageBonus;

    // light attack
    [SerializeField] public GameObject grabHitbox;
    public float grabStartup;
    public float grabActiveHitboxDuration;
    public float grabEndLag;
    public float grabDuration;
    public float holdLength;
    public float throwDamageMultiplier;

    public float throwDuration;

    public float throwDamage;

    // hitstun stuff
    public float heavyAttackInitialHitstunLength;
    public float heavyAttackTotalHitstunLength;
    public float heavyAttackKnockbackStrength;
    public float heavyAttackShieldStunLength;

    public float lightAttackInitialHitstunLength;
    public float lightAttackTotalHitstunLength;
    public float lightAttackKnockbackStrength;
    public float lightAttackShieldStunLength;

    public float throwTotalHitstunLength;
    public float throwKnockbackStrength;

    public float clankHitstunDuration;
    public float clankKnockbackStrength;

    public float takeLightDamageTimer;
    public float takeHeavyDamageTimer;
    public float getGrabbedTimer;

    // splatter invulnerability
    public float splatterInvulnerableTime;
    public int splatterCounter;

    // shield
    public GameObject shield;

    // hitstun timers
    public float attackTriggerTime;

    // movement
    public bool canMove;
    public bool isStuck;

    // dash variables
    public float dashStrength;
    public float dashLength;

    // Pipes
    public Vector2 leftPipeExit;
    public Vector2 rightPipeExit;

    //Interaction Manager
    public PlayerInteractionManager interaction;
    

    // TODO: Temporary gamepad assignment
    public Gamepad gamepad;
    public UnityEngine.InputSystem.Controls.ButtonControl heavyAttackButton;
    public UnityEngine.InputSystem.Controls.ButtonControl lightAttackButton;
    public UnityEngine.InputSystem.Controls.ButtonControl dashButton;
    public UnityEngine.InputSystem.Controls.ButtonControl cross;
    public UnityEngine.InputSystem.Controls.ButtonControl rightTrigger;
    public UnityEngine.InputSystem.Controls.ButtonControl leftTrigger;
    public UnityEngine.InputSystem.Controls.ButtonControl rightBumper;
    public UnityEngine.InputSystem.Controls.ButtonControl leftBumper;
    public UnityEngine.InputSystem.Controls.StickControl leftStick;
    
    
    
    // Stats
    public float health;
    //public TextMesh healthText;
    public HealthBarVisuals healthBarVisuals;


    // prefabs
    public GameObject coinPrefab;

    //public int currency = 0;
    //public float passiveIncomePerMinute = 1;
    //public float passiveIncomeTimer = 0;

    // TEMP FUNCTION
    //public void UpdateHealthUI()
    //{
    //    healthText.text = health.ToString();
    //}

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteractionManager>();
        currencyManager = GetComponent<CurrencyManager>();
        // assign tags
        gameObject.tag = "p" + playerMovement.playerNumber.ToString();

        // TODO setting it to null rn to check when stuff breaks
        playerAttackingYouManager = null;
        
        circleSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<Collider2D>();

        // Setting Numbers
        attackTriggerTime = 0.05f;
        health = 100f;
        // TODO TEMP FUCNTION
        healthBarVisuals.UpdateUI();
        dashStrength = 300;
        dashLength = 0.5f;
        lightAttackDamage = 10f;
        lightAttackDamageBonus = 1;
        heavyAttackDamage = 20f;
        heavyAttackDamageBonus = 1;
        throwDamage = 15f;
        throwDamageMultiplier = 1;
        bufferSize = 0.25f;

        // hitstun stuff
        heavyAttackInitialHitstunLength = 0.2f;
        heavyAttackTotalHitstunLength = 1f;
        heavyAttackKnockbackStrength = 400;
        heavyAttackShieldStunLength = 0.2f;

        lightAttackInitialHitstunLength = 0.13f;
        lightAttackTotalHitstunLength = 0.8f;
        lightAttackKnockbackStrength = 250;
        lightAttackShieldStunLength = 0.1f;

        throwTotalHitstunLength = 1.0f;
        throwKnockbackStrength = 400;

        clankHitstunDuration = 0.2f;
        clankKnockbackStrength = 250;

        // splatter
        splatterInvulnerableTime = 0.6f;

        // movement
        canMove = true;
        isStuck = false;

        // TODO: Temporary gamepad assignment
        gamepad = Gamepad.all[GetComponent<PlayerMovement>().playerNumber - 1];
        heavyAttackButton = gamepad.buttonEast;
        lightAttackButton = gamepad.buttonWest;
        dashButton = gamepad.buttonNorth;
        cross = gamepad.buttonSouth;
        rightTrigger = gamepad.rightTrigger;
        leftTrigger = gamepad.leftTrigger;
        rightBumper = gamepad.rightShoulder;
        leftBumper = gamepad.leftShoulder;
        leftStick = gamepad.leftStick;

        // buffer dictionary
        bufferDictionary.Add("dash", DashState);
        bufferDictionary.Add("lightAttack", LightAttackState);
        bufferDictionary.Add("heavyAttack", HeavyAttackState);
        bufferDictionary.Add("grab", GrabState);
        bufferDictionary.Add("shield", ShieldState);

        // buffer input to string
        //bufferInputToString.Add(dashButton, "dash");
        //bufferInputToString.Add(lightAttackButton, "lightAttack");
        //bufferInputToString.Add(heavyAttackButton, "heavyAttack");
        //bufferInputToString.Add(leftBumper, "grab");
        //bufferInputToString.Add(rightTrigger, "shield");

        // light attack variables
        lightAttackStartup = 0.2f;
        lightAttackActiveHitboxDuration = 0.1f;
        lightAttackEndLag = 0.3f;
        lightAttackDuration = lightAttackStartup + lightAttackActiveHitboxDuration + lightAttackEndLag;
        curLightAttackHitbox = 0;

        // heavy attack variables
        heavyAttackStartup = 0.35f;
        heavyAttackActiveHitboxDuration = 0.2f;
        heavyAttackEndLag = 0.4f;
        heavyAttackDuration = heavyAttackStartup + heavyAttackActiveHitboxDuration + heavyAttackEndLag;

        // grab variables
        grabStartup = 0.2f;
        grabActiveHitboxDuration = 0.1f;
        grabEndLag = 0.3f;
        grabDuration = grabStartup + grabActiveHitboxDuration + grabEndLag;

        holdLength = 2.69f;

        throwDuration = 0.4f;

        // Death variables
        DyingState.dyingLength = 0.3f;
        DeadState.deadLength = 1f;

        RespawnState.respawnLength = 2f;

        // Pipe Positions:
        leftPipeExit = new Vector2(-10f, 8f);
        rightPipeExit = new Vector2(10f, 8f);



        playerSpriteRenderer = playerSpriteAnim.gameObject.GetComponent<SpriteRenderer>();

        currentState = IdleState;
        currentState.EnterState(this);
    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("p1") != null)
        {
            allPlayers.Add(GameObject.FindGameObjectWithTag("p1").GetComponent<CombatStateManager>());
        }
        if (GameObject.FindGameObjectWithTag("p2") != null)
        {
            allPlayers.Add(GameObject.FindGameObjectWithTag("p2").GetComponent<CombatStateManager>());
        }
        if (GameObject.FindGameObjectWithTag("p3") != null)
        {
            allPlayers.Add(GameObject.FindGameObjectWithTag("p3").GetComponent<CombatStateManager>());
        }
        if (GameObject.FindGameObjectWithTag("p4") != null)
        {
            allPlayers.Add(GameObject.FindGameObjectWithTag("p4").GetComponent<CombatStateManager>());
        }

        LightAttackState.canHit = new List<bool>();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            LightAttackState.canHit.Add(true);
        }

        HeavyAttackState.canHit = new List<bool>();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            HeavyAttackState.canHit.Add(true);
        }

        GrabState.canHit = new List<bool>();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            GrabState.canHit.Add(true);
        }
    }

    void Update()
    {
        currentState.UpdateState(this);

        currentStateString = currentState.ToString();
    }

    private void LateUpdate()
    {
        currentState.LateUpdateState(this);
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        currentState.OnTriggerStay(this, collision);
        //Debug.Log("thinks is in trigger");

        UpdateGettingHitTimers(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetGettingHitTimers(collision);
    }


    // TODO Fix that you can splatter away from wall if u start at wall and dash away
    private void OnCollisionStay2D(Collision2D collision)
    {
        
        // SPLATTER
        if ((currentState == HitstunState) && collision.gameObject.layer.Equals(10) && health > 0)
        {
            if (splatterCounter >= 1)
            {
                SplatterState.splatterDirection = collision.contacts[0].normal;
                currentState.ForcedOutOfState(this);
                float collisionStrength = Mathf.Clamp(
                    (HitstunState.hitstunTimer - HitstunState.currentInitialHitstunDuration) / (HitstunState.currentHitstunDuration / 6),
                    0.15f, 1f);
                //Debug.Log(collisionStrength);
                SwitchState(SplatterState, collisionStrength, "hitstun");
            }
            splatterCounter++;
        }
        else if ((currentState == DashState) && collision.gameObject.layer.Equals(10))
        {
            if (splatterCounter >= 1)
            {
                SplatterState.splatterDirection = collision.contacts[0].normal;
                currentState.ForcedOutOfState(this);
                float collisionStrength = Mathf.Clamp(
                     DashState.dashTimer/ (dashLength / 2),
                    0.15f, 1f);
                //Debug.Log(collisionStrength);
                SwitchState(SplatterState, collisionStrength, "dash");
            }
            splatterCounter++;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer.Equals(10))
        {
            splatterCounter = 0;
        }
    }


    public void SwitchState(CombatBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void SwitchState(CombatBaseState state, float number)
    {
        currentState = state;
        currentState.EnterState(this, number);
    }

    public void SwitchState(CombatBaseState state, float number, string str)
    {
        currentState = state;
        currentState.EnterState(this, number, str);
    }

    public void UpdateBufferInput()
    {
        if (lightAttackButton.wasPressedThisFrame)
        {
            bufferString = "lightAttack";
        }
        else if (heavyAttackButton.wasPressedThisFrame)
        {
            bufferString = "heavyAttack";
        }
        else if (dashButton.wasPressedThisFrame)
        {
            bufferString = "dash";
        }
        else if (leftBumper.wasPressedThisFrame)
        {
            bufferString = "grab";
        }
        else if (rightTrigger.isPressed)
        {
            bufferString = "shield";
        }
    }

    private void UpdateGettingHitTimers(Collider2D collision)
    {
        // light attack - take damage
        if (collision.gameObject.layer.Equals(6))
        {
            takeLightDamageTimer += Time.deltaTime;
        }


        // heavy attack - take damage
        if (collision.gameObject.layer.Equals(7))
        {
            takeHeavyDamageTimer += Time.deltaTime;
        }

        // grab - take damage
        if (collision.gameObject.layer.Equals(9))
        {
            getGrabbedTimer += Time.deltaTime;
        }

        GetHit(collision);
    }

    private void GetHit(Collider2D collision)
    {
        if (!(/*currentState == SplatterState ||*/ currentState == ShieldState || currentState == ShieldStunState))
        {
            if (takeLightDamageTimer >= attackTriggerTime)
            {
                UpdatePlayerAttackingYou(collision);

                if (playerAttackingYouManager.LightAttackState.canHit[playerMovement.playerNumber - 1])
                {
                    takeLightDamageTimer = 0;
                    currentState.ForcedOutOfState(this);
                    playerAttackingYouManager.LightAttackState.canHit[playerMovement.playerNumber - 1] = false;
                    SwitchState(HitstunState, lightAttackDamage, collision.tag);
                }
            }
            if (takeHeavyDamageTimer >= attackTriggerTime)
            {
                UpdatePlayerAttackingYou(collision);

                if (playerAttackingYouManager.HeavyAttackState.canHit[playerMovement.playerNumber - 1])
                {
                    takeHeavyDamageTimer = 0;
                    currentState.ForcedOutOfState(this);
                    playerAttackingYouManager.HeavyAttackState.canHit[playerMovement.playerNumber - 1] = false;
                    SwitchState(HitstunState, heavyAttackDamage, collision.tag);
                    
                }
            }
        } else if (currentState == ShieldState)
        {
            UpdatePlayerAttackingYou(collision);

            // get shield light attacked
            if (takeLightDamageTimer >= attackTriggerTime && playerAttackingYouManager.LightAttackState.canHit[playerMovement.playerNumber - 1])
            {

                takeLightDamageTimer = 0;
                playerAttackingYouManager.LightAttackState.canHit[playerMovement.playerNumber - 1] = false;
                SwitchState(ShieldStunState, lightAttackShieldStunLength);
            }

            // get shield heavy attacked
            else if (takeHeavyDamageTimer >= attackTriggerTime && playerAttackingYouManager.HeavyAttackState.canHit[playerMovement.playerNumber - 1])
            {
                //Debug.Log("Heavy Switch");
                takeHeavyDamageTimer = 0;
                playerAttackingYouManager.HeavyAttackState.canHit[playerMovement.playerNumber - 1] = false;
                SwitchState(ShieldStunState, heavyAttackShieldStunLength);
            }
        }
        // get grabbed
        if (getGrabbedTimer >= attackTriggerTime)
        {
            UpdatePlayerAttackingYou(collision);

            if (playerAttackingYouManager.GrabState.canHit[playerMovement.playerNumber - 1])
            {
                getGrabbedTimer = 0;
                currentState.ForcedOutOfState(this);
                playerAttackingYouManager.GrabState.canHit[playerMovement.playerNumber - 1] = false;
                SwitchState(GrabbedState);
            }
        }
    }

    private void UpdatePlayerAttackingYou(Collider2D collision)
    {
        // ! update player attacking
        if (collision.transform.parent.parent)
        {
            if (collision.transform.parent.parent.GetComponent<CombatStateManager>() != null)
            {
                playerAttackingYouManager = collision.transform.parent.parent.GetComponent<CombatStateManager>();
            }
        }
    }

    private void ResetGettingHitTimers(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(6) || collision.gameObject.layer.Equals(7) || collision.gameObject.layer.Equals(9))
        {
            takeLightDamageTimer = 0f;
            takeHeavyDamageTimer = 0f;
            getGrabbedTimer = 0f;
        }
    }

    // for idle state
    public void BecomeInvulnerable(float timeInvulnerable)
    {
        invulnerableCollider.SetActive(true);
        mainCollider.enabled = false;
        playerSpriteRenderer.color = Color.white / 2f;
        StartCoroutine(InvulnerableDelay(timeInvulnerable));
    }

    public void BecomeInvulnerable()
    {
        invulnerableCollider.SetActive(true);
        mainCollider.enabled = false;
        playerSpriteRenderer.color = Color.white / 2f;
    }

    public IEnumerator InvulnerableDelay(float timeInvulnerable)
    {
        yield return new WaitForSeconds(timeInvulnerable);
        BecomeVulnerable();
    }

    public void BecomeVulnerable()
    {
        invulnerableCollider.SetActive(false);
        mainCollider.enabled = true;
        playerSpriteRenderer.color = Color.white;
    }

    public int UpgradeAttack(int curHitbox, GameObject[] hitBoxArray)
    {
        if (curHitbox < hitBoxArray.Length - 1)
        {
            curHitbox += 1;
        }
        return curHitbox;
    }

    public void SpawnDeathCoins()
    {
        // Calculate the positions for the three prefabs
        Vector3 positionA = transform.position + new Vector3(1f, 0f, 0f)/2;
        Vector3 positionB = transform.position + new Vector3(-0.5f, 0.866f, 0f)/2;
        Vector3 positionC = transform.position + new Vector3(-0.5f, -0.866f, 0f)/2;

        // Instantiate the prefabs at the calculated positions
        Instantiate(coinPrefab, positionA, Quaternion.identity);
        Instantiate(coinPrefab, positionB, Quaternion.identity);
        Instantiate(coinPrefab, positionC, Quaternion.identity);
    }

    //public void PassiveIncome()
    //{
    //    passiveIncomeTimer += Time.deltaTime;

    //    float timeToAddCoin = 60 / passiveIncomePerMinute;

    //    if (passiveIncomeTimer >= timeToAddCoin)
    //    {
    //        currency++;
    //        passiveIncomeTimer = 0;
    //    }
    //}
}
