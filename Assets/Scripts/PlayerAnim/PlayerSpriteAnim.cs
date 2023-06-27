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
    //private bool InState<State>()
    //{
    //    return combatStateManager.currentState.GetType() == typeof(State);
    //}


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
            
        if (combatStateManager.currentState == combatStateManager.IdleState)
        {
            WalkUpdateTarget();

            WalkSwitchIKTarget(leftArmTargetIK, leftArmTarget);
            WalkSwitchIKTarget(leftLegTargetIK, leftLegTarget);
            WalkSwitchIKTarget(rightLegTargetIK, rightLegTarget);
            WalkSwitchIKTarget(rightArmTargetIK, rightArmTarget);
        }
        else if (combatStateManager.currentState == combatStateManager.LightAttackState)
        {
            WalkUpdateTarget();

            // animate punching arm
            RightPunch();

            // update walk except for attacking hands
            WalkSwitchIKTarget(leftLegTargetIK, leftLegTarget);
            WalkSwitchIKTarget(rightLegTargetIK, rightLegTarget);
        }
        else if (combatStateManager.currentState == combatStateManager.HeavyAttackState)
        {
            WalkUpdateTarget();
            FullBodyPunch();
        }
        else if (combatStateManager.currentState == combatStateManager.DashState)
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
        rightArmTargetIK.position = rightArmBase.position + (-transform.up + transform.right/2) / 2;

        leftLegTargetIK.position = leftLegBase.position + (-transform.up) / 2;
        leftArmTargetIK.position = leftArmBase.position + (-transform.up + -transform.right/2) / 2;
    }

    void RightPunch()
    {
        if (combatStateManager.LightAttackState.attackTimer < combatStateManager.lightAttackStartup / 2)
        {
            // charge animation
            // right arm
            IKToTarget(rightArmTargetIK,
                rightArmBase.position + transform.right - transform.up / 2,
                //combatStateManager.lightAttackStartup);
                10);

            //left arm
            IKToTarget(leftArmTargetIK,
                leftArmBase.position - transform.right - transform.up / 2,
                //combatStateManager.lightAttackStartup);
                10);

        } else
        {
            // punch animation
            // right arm
            IKToTarget(rightArmTargetIK,
                leftArmBase.position + transform.up/2 + transform.right/4,
                //combatStateManager.lightAttackActiveHitboxDuration);
                15);

            // left arm
            IKToTarget(leftArmTargetIK,
                rightArmBase.position + transform.up/2 - transform.right/4,
                //combatStateManager.lightAttackActiveHitboxDuration);
                15);
        }
    }

    void FullBodyPunch()
    {
        if (combatStateManager.HeavyAttackState.attackTimer < combatStateManager.heavyAttackStartup/2)
        {
            // charge animation
            // right arm
            IKToTarget(rightArmTargetIK,
                rightArmBase.position + (-transform.up + transform.right / 2) / 2,
                //combatStateManager.heavyAttackStartup);
                10);

            // left arm
            IKToTarget(leftArmTargetIK,
                leftArmBase.position + (-transform.up + -transform.right / 2) / 2,
                //combatStateManager.heavyAttackStartup);
                10);

            // right leg
            IKToTarget(rightLegTargetIK,
                rightLegBase.position + (-transform.up) / 2,
                //combatStateManager.heavyAttackStartup);
                10);

            // left leg
            IKToTarget(leftLegTargetIK,
                leftLegBase.position + (-transform.up) / 2,
                //combatStateManager.heavyAttackStartup);
                10);
        }
        else
        {
            // attack animation
            // right arm
            IKToTarget(rightArmTargetIK,
                rightArmBase.position + (transform.up + transform.right / 2) / 2,
                //combatStateManager.heavyAttackActiveHitboxDuration/2);
                10);

            // left arm
            IKToTarget(leftArmTargetIK,
                leftArmBase.position + (transform.up + -transform.right / 2) / 2,
                //combatStateManager.heavyAttackActiveHitboxDuration/2);
                10);

            // right leg
            IKToTarget(rightLegTargetIK,
                rightLegBase.position + (transform.up) / 4,
                //combatStateManager.heavyAttackActiveHitboxDuration/2);
                10);

            // left leg
            IKToTarget(leftLegTargetIK,
                leftLegBase.position + (transform.up) / 4,
                //combatStateManager.heavyAttackActiveHitboxDuration/2);
                10);
        }
    }

    private void IKToTarget(Transform IKTrans, Vector3 target, float speed)
    {
        IKTrans.position += new Vector3(combatStateManager.rb.velocity.x, combatStateManager.rb.velocity.y) * Time.deltaTime;

        //float distanceToTarget = Vector3.Distance(IKTrans.position, target);
        //float speed = distanceToTarget / transitionLength;

        IKTrans.position = Vector3.MoveTowards(IKTrans.position, target,
            speed * Time.deltaTime);
    }


}
