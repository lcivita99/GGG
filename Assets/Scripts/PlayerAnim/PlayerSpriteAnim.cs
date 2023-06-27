using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpriteAnim : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private CombatStateManager combatStateManager;

    // player for position & movement reference
    [SerializeField] private GameObject player;
    // equal to player transform, but includes rotation based on player input
    [SerializeField] private GameObject targetTransform;

    private Rigidbody2D playerRb;
    private Vector3 moveDir;

    // ! state checker -- Pass in class type
    // ! example: InState<IdleState>()
    private bool InState<State>()
    {
        return combatStateManager.currentState.GetType() == typeof(State);
    }


    // limb bases
    [SerializeField] private Transform rightLegBase;
    [SerializeField] private Transform leftLegBase;
    [SerializeField] private Transform rightArmBase;
    [SerializeField] private Transform leftArmBase;
    
    // limb targets -- updated every frame
    [SerializeField] private Transform rightLegTarget;
    [SerializeField] private Transform leftLegTarget;
    [SerializeField] private Transform rightArmTarget;
    [SerializeField] private Transform leftArmTarget;
    
    // IK targets -- usually update based on current position vs. target position
    [SerializeField] private Transform rightLegTargetIK;
    [SerializeField] private Transform leftLegTargetIK;
    [SerializeField] private Transform rightArmTargetIK;
    [SerializeField] private Transform leftArmTargetIK;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        combatStateManager = player.GetComponent<CombatStateManager>();

        // low framerate effect
        // TODO: Is this the best way?
        InvokeRepeating("SetTransform", 0.2f, 0.07f);
    }

    // Update is called once per frame
    void Update()
    {
        MatchPlayerMovement();
            
        if (InState<IdleState>())
        {
            WalkUpdateTarget();

            WalkSwitchIKTarget(leftArmTargetIK, leftArmTarget);
            WalkSwitchIKTarget(leftLegTargetIK, leftLegTarget);
            WalkSwitchIKTarget(rightLegTargetIK, rightLegTarget);
            WalkSwitchIKTarget(rightArmTargetIK, rightArmTarget);
        }
        else if (InState<LightAttackState>())
        {

        }
        else if (InState<HeavyAttackState>())
        {

        }
        else if (InState<DashState>())
        {
            DashUpdateTarget();
        }
        else
        {
            return;
        }
    }

    private void SetTransform()
    {
        transform.position = targetTransform.transform.position;
        transform.rotation = targetTransform.transform.rotation;
    }

    void MatchPlayerMovement()
    {
        moveDir = playerMovement.gamepad.leftStick.ReadValue();
        if (moveDir.magnitude > 1)
        {
            moveDir = moveDir.normalized;
        }

        // TODO: Lerp with proper Time.deltaTime usage
        targetTransform.transform.up = Vector3.Lerp(targetTransform.transform.up, moveDir, Time.deltaTime * 25);
        targetTransform.transform.position = player.transform.position;
    }

    void WalkSwitchIKTarget(Transform IKTransform, Transform IKConstant)
    {
        if (Vector3.Distance(IKTransform.position, IKConstant.position) > 0.6f)
        {
            IKTransform.position = IKConstant.position;
        }
    }

    void WalkUpdateTarget()
    {
        // TODO: make these into Vector3 functions or variables perhaps?
        rightLegTarget.position = rightLegBase.position + (-transform.up + transform.right) / 4 + moveDir/2;
        rightArmTarget.position = rightArmBase.position + (transform.up + transform.right) / 4 + moveDir/2;

        leftLegTarget.position = leftLegBase.position + (-transform.up + -transform.right) / 4 + moveDir/2;
        leftArmTarget.position = leftArmBase.position + (transform.up + -transform.right) / 4 + moveDir/2;
    }

    void DashUpdateTarget()
    {
        rightLegTargetIK.position = rightLegBase.position + (-transform.up) / 2;
        rightArmTargetIK.position = rightArmBase.position + (-transform.up + transform.right) / 2;

        leftLegTargetIK.position = leftLegBase.position + (-transform.up) / 2;
        leftArmTargetIK.position = rightArmBase.position + (-transform.up + -transform.right) / 2;
    }
}
