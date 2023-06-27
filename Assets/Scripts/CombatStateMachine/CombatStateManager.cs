using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatStateManager : MonoBehaviour
{
    public string bufferString = "";
    public Dictionary<string, CombatBaseState> bufferDictionary = new Dictionary<string, CombatBaseState>();
    public float bufferSize = 0.17f;
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

    [SerializeField] public GameObject heavyAttackHitbox;
    public float heavyAttackStartup;
    public float heavyAttackActiveHitboxDuration;
    public float heavyAttackEndLag;
    public float heavyAttackDuration;

    // dash variables
    public float dashStrength = 200;
    public float dashLength = 0.5f;

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

    public float health = 100f;

    void Start()
    {
        circleSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        

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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        currentState.OnTriggerStay(this, collision);
    }


    public void SwitchState(CombatBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void UpdateBufferInput()
    {
        if (lightAttackButton.isPressed)
        {
            bufferString = "lightAttack";
        }
        else if (heavyAttackButton.isPressed)
        {
            bufferString = "heavyAttack";
        }
        else if (dashButton.isPressed)
        {
            bufferString = "dash";
        }
        else if (leftBumper.isPressed)
        {
            bufferString = "grab";
        }
        else if (rightTrigger.isPressed)
        {
            bufferString = "shield";
        }
    }
}
