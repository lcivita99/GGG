using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatStateManager : MonoBehaviour
{
    public GameObject otherPlayer;
    
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
    public GrabState GrabState = new GrabState();
    public HoldState HoldState = new HoldState();
    public ThrowState ThrowState = new ThrowState();
    public HitstunState HitstunState = new HitstunState();
    //public SpecialState SpecialState = new SpecialState();

    public SpriteRenderer circleSprite;
    public Rigidbody2D rb;
    public Transform playerSpriteTargetTransform;

    // attack hitboxes & variables
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

    // hitstun stuff
    public float heavyAttackInitialHitstunLength;
    public float heavyAttackTotalHitstunLength;
    public float heavyAttackKnockbackStrength;

    public float lightAttackInitialHitstunLength;
    public float lightAttackTotalHitstunLength;
    public float lightAttackKnockbackStrength;

    public float clankHitstunDuration;
    public float clankKnockbackStrength;

    public float takeLightDamageTimer;
    public float takeHeavyDamageTimer;

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

    public float health;

    void Start()
    {
        circleSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Setting Numbers
        attackTriggerTime = 0.05f;
        health = 100f;
        dashStrength = 200;
        dashLength = 0.5f;
        lightAttackDamage = 10f;
        heavyAttackDamage = 20f;
        bufferSize = 0.25f;

        // hitstun stuff
        heavyAttackInitialHitstunLength = 0.2f;
        heavyAttackTotalHitstunLength = 1f;
        heavyAttackKnockbackStrength = 400;

        lightAttackInitialHitstunLength = 0.1f;
        lightAttackTotalHitstunLength = 0.7f;
        lightAttackKnockbackStrength = 250;

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
        lightAttackEndLag = 0.2f;
        lightAttackDuration = lightAttackStartup + lightAttackActiveHitboxDuration + lightAttackEndLag;

        // heavy attack variables
        heavyAttackStartup = 0.35f;
        heavyAttackActiveHitboxDuration = 0.2f;
        heavyAttackEndLag = 0.35f;
        heavyAttackDuration = heavyAttackStartup + heavyAttackActiveHitboxDuration + heavyAttackEndLag;

        

        currentState = IdleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
        if (!(currentState == SplatterState || currentState == ShieldState || currentState == ShieldStunState))
        {
            //Debug.Log(currentState.ToString());
            GetHit();
        } // else GetHitOnShield();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        currentState.OnTriggerStay(this, collision);

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
    }

    private void GetHit()
    {
        if (takeLightDamageTimer >= attackTriggerTime)
        {
            takeLightDamageTimer = 0;
            currentState.HitOutOfState(this);
            SwitchState(HitstunState, lightAttackDamage);
        }
        if (takeHeavyDamageTimer >= attackTriggerTime)
        {
            takeHeavyDamageTimer = 0;
            currentState.HitOutOfState(this);
            SwitchState(HitstunState, heavyAttackDamage);
        }
    }

    private void ResetGettingHitTimers(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(6) || collision.gameObject.layer.Equals(7))
        {
            takeLightDamageTimer = 0f;
            takeHeavyDamageTimer = 0f;
        }
    }
}
