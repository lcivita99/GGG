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

    public List<CombatStateManager> allPlayers = new List<CombatStateManager>();
    public CombatStateManager playerAttackingYouManager;

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
    //public SpecialState SpecialState = new SpecialState();

    public SpriteRenderer circleSprite;
    public Rigidbody2D rb;
    public Collider2D mainCollider;
    public Transform playerSpriteTargetTransform;
    public PlayerSpriteAnim playerSpriteAnim;

    // attack hitboxes & variables
    public bool attackTimerStuck;

    // light attack
    [SerializeField] public GameObject lightAttackHitbox;
    public float lightAttackStartup;
    public float lightAttackActiveHitboxDuration;
    public float lightAttackEndLag;
    public float lightAttackDuration;
    public float lightAttackDamage;

    // heavy attack
    [SerializeField] public GameObject heavyAttackHitbox;
    public float heavyAttackStartup;
    public float heavyAttackActiveHitboxDuration;
    public float heavyAttackEndLag;
    public float heavyAttackDuration;
    public float heavyAttackDamage;

    // light attack
    [SerializeField] public GameObject grabHitbox;
    public float grabStartup;
    public float grabActiveHitboxDuration;
    public float grabEndLag;
    public float grabDuration;
    public float holdLength;

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
    // UI
    public float health;
    public TextMesh healthText;

    // TEMP FUNCTION
    public void UpdateHealthUI()
    {
        healthText.text = health.ToString();
    }

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
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
        UpdateHealthUI();
        dashStrength = 300;
        dashLength = 0.5f;
        lightAttackDamage = 10f;
        heavyAttackDamage = 20f;
        throwDamage = 15f;
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

        // heavy attack variables
        heavyAttackStartup = 0.35f;
        heavyAttackActiveHitboxDuration = 0.2f;
        heavyAttackEndLag = 0.4f;
        heavyAttackDuration = heavyAttackStartup + heavyAttackActiveHitboxDuration + heavyAttackEndLag;

        // grab variables
        grabStartup = 0.2f;
        grabActiveHitboxDuration = 0.1f;
        grabEndLag = 0.2f;
        grabDuration = grabStartup + grabActiveHitboxDuration + grabEndLag;

        holdLength = 2.69f;

        throwDuration = 0.4f;

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

        // player collider ignore other player's shields
        //for (int i = 0; i < allPlayers.Count; i++)
        //{
        //    if (this != allPlayers[i])
        //    {
        //        Debug.Log("it ran");
        //        Physics2D.IgnoreCollision(mainCollider, allPlayers[i].shield.GetComponentInChildren<Collider2D>(), true);
        //    }
        //}
    }

    void Update()
    {
        currentState.UpdateState(this);
        //if (!(currentState == SplatterState || currentState == ShieldState || currentState == ShieldStunState))
        //{
        
        //}
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
        if (!(currentState == SplatterState || currentState == ShieldState || currentState == ShieldStunState))
        {
            if (takeLightDamageTimer >= attackTriggerTime)
            {
                // ! update player attacking
                if (collision.transform.parent.parent.GetComponent<CombatStateManager>() != null)
                {
                    playerAttackingYouManager = collision.transform.parent.parent.GetComponent<CombatStateManager>();
                }

                if (playerAttackingYouManager.LightAttackState.canHit[playerMovement.playerNumber - 1])
                {
                    takeLightDamageTimer = 0;
                    currentState.ForcedOutOfState(this);
                    playerAttackingYouManager.LightAttackState.canHit[playerMovement.playerNumber - 1] = false;
                    SwitchState(HitstunState, lightAttackDamage);
                }
            }
            if (takeHeavyDamageTimer >= attackTriggerTime)
            {
                // ! update player attacking
                if (collision.transform.parent.parent.GetComponent<CombatStateManager>() != null)
                {
                    playerAttackingYouManager = collision.transform.parent.parent.GetComponent<CombatStateManager>();
                }

                if (playerAttackingYouManager.HeavyAttackState.canHit[playerMovement.playerNumber - 1])
                {
                    takeHeavyDamageTimer = 0;
                    currentState.ForcedOutOfState(this);
                    playerAttackingYouManager.HeavyAttackState.canHit[playerMovement.playerNumber - 1] = false;
                    SwitchState(HitstunState, heavyAttackDamage);
                }
            }
        } else if (currentState == ShieldState)
        {
            // ! update player attacking
            if (collision.transform.parent.parent.GetComponent<CombatStateManager>() != null)
            {
                playerAttackingYouManager = collision.transform.parent.parent.GetComponent<CombatStateManager>();
            }
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
            // ! update player attacking
            if (collision.transform.parent.parent.GetComponent<CombatStateManager>() != null)
            {
                playerAttackingYouManager = collision.transform.parent.parent.GetComponent<CombatStateManager>();
            }

            if (playerAttackingYouManager.GrabState.canHit[playerMovement.playerNumber - 1])
            {
                getGrabbedTimer = 0;
                currentState.ForcedOutOfState(this);
                playerAttackingYouManager.GrabState.canHit[playerMovement.playerNumber - 1] = false;
                SwitchState(GrabbedState);
            }
        }

    //    // get shield light attacked
    //    if (takeLightDamageTimer >= attackTriggerTime && playerAttackingYouManager.LightAttackState.canHit[playerMovement.playerNumber - 1] &&
    //        (currentState == SplatterState || currentState == ShieldState || currentState == ShieldStunState))
    //    {
    //        // ! update player attacking
    //        playerAttackingYouManager = collision.transform.parent.parent.GetComponent<CombatStateManager>();

    //        takeLightDamageTimer = 0;
    //        playerAttackingYouManager.LightAttackState.canHit[playerMovement.playerNumber - 1] = false;
    //        SwitchState(ShieldStunState, lightAttackShieldStunLength);
    //    }
    //    // get shield heavy attacked
    //    if (takeHeavyDamageTimer >= attackTriggerTime && playerAttackingYouManager.HeavyAttackState.canHit[playerMovement.playerNumber - 1] &&
    //        (currentState == SplatterState || currentState == ShieldState || currentState == ShieldStunState))
    //    {
    //        // ! update player attacking
    //        playerAttackingYouManager = collision.transform.parent.parent.GetComponent<CombatStateManager>();

    //        takeHeavyDamageTimer = 0;
    //        playerAttackingYouManager.HeavyAttackState.canHit[playerMovement.playerNumber - 1] = false;
    //        SwitchState(ShieldStunState, heavyAttackShieldStunLength);
    //    }
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
}
