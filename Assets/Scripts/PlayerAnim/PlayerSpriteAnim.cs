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

    [SerializeField] private SpriteRenderer attackSpriteRenderer;
    [SerializeField] private Sprite[] lightAttackSprites;
    [SerializeField] private Sprite[] heavyAttackSprites;
    [SerializeField] private Sprite[] grabSprites;
    //[SerializeField] private Sprite[] grabbedSprites;
    public GameObject grabbedIndicator;

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

    public List<float> heavyFrameStartup = new List<float>();
    public int heavyIndex = 0;

    public List<float> lightFrameStartup = new List<float>();
    public int lightIndex = 0;

    public List<float> grabFrameStartup = new List<float>();
    public int grabIndex = 0;

    public void SetHeavySpriteToIdx(int idx)
    {
        if (attackSpriteRenderer.sprite != heavyAttackSprites[idx])
        {
            attackSpriteRenderer.sprite = heavyAttackSprites[idx];
        }
        heavyIndex = idx;
    }
    public void SetLightSpriteToIdx(int idx)
    {
        if (attackSpriteRenderer.sprite != lightAttackSprites[idx])
        {
            attackSpriteRenderer.sprite = lightAttackSprites[idx];
        }
        lightIndex = idx;
    }

    public void SetGrabSpriteToIdx(int idx)
    {
        if (attackSpriteRenderer.sprite != grabSprites[idx])
        {
            attackSpriteRenderer.sprite = grabSprites[idx];
        }
        grabIndex = idx;
    }

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        combatStateManager = player.GetComponent<CombatStateManager>();

        // low framerate effect
        // TODO: Is this the best way?
        InvokeRepeating("SetTransform", 0.2f, 0.07f);

        // heavy frame setting
        heavyFrameStartup.Add(combatStateManager.heavyAttackStartup / 6);
        heavyFrameStartup.Add(combatStateManager.heavyAttackStartup / 3);
        heavyFrameStartup.Add(5 * combatStateManager.heavyAttackStartup / 6);
        heavyFrameStartup.Add(combatStateManager.heavyAttackStartup);
        heavyFrameStartup.Add(combatStateManager.heavyAttackStartup +
            combatStateManager.heavyAttackActiveHitboxDuration / 1.5f);
        heavyFrameStartup.Add(combatStateManager.heavyAttackStartup +
           combatStateManager.heavyAttackActiveHitboxDuration);
        heavyFrameStartup.Add(combatStateManager.heavyAttackDuration);

        // light frame startup
        lightFrameStartup.Add(combatStateManager.lightAttackStartup / 3);
        lightFrameStartup.Add(5 * combatStateManager.lightAttackStartup / 6);
        lightFrameStartup.Add(combatStateManager.lightAttackStartup);
        lightFrameStartup.Add(combatStateManager.lightAttackStartup +
            combatStateManager.lightAttackActiveHitboxDuration / 1.5f);
        lightFrameStartup.Add(combatStateManager.lightAttackStartup +
            combatStateManager.lightAttackActiveHitboxDuration);
        lightFrameStartup.Add(combatStateManager.lightAttackDuration);

        // grab frame startup
        grabFrameStartup.Add(combatStateManager.grabStartup / 3);
        grabFrameStartup.Add(5 * combatStateManager.grabStartup / 6);
        grabFrameStartup.Add(combatStateManager.grabStartup);
        grabFrameStartup.Add(combatStateManager.grabStartup +
            combatStateManager.grabActiveHitboxDuration / 1.5f);
        grabFrameStartup.Add(combatStateManager.grabStartup +
            combatStateManager.grabActiveHitboxDuration);
        grabFrameStartup.Add(combatStateManager.grabDuration);
    }

    // Update is called once per frame
    void LateUpdate()
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

            UpdateLightAttackSprites();

            // animate punching arm
            RightPunch();

            // update walk except for attacking hands
            WalkSwitchIKTarget(leftLegTargetIK, leftLegTarget);
            WalkSwitchIKTarget(rightLegTargetIK, rightLegTarget);
        }
        else if (combatStateManager.currentState == combatStateManager.HeavyAttackState)
        {
            WalkUpdateTarget();
            UpdateHeavyAttackSprites();
            FullBodyPunch();
        }
        else if (combatStateManager.currentState == combatStateManager.DashState)
        {
            DashUpdateTarget();
        }
        else if (combatStateManager.currentState == combatStateManager.ShieldState )
        {
            WalkUpdateTarget();

            WalkSwitchIKTarget(leftArmTargetIK, leftArmTarget);
            WalkSwitchIKTarget(leftLegTargetIK, leftLegTarget);
            WalkSwitchIKTarget(rightLegTargetIK, rightLegTarget);
            WalkSwitchIKTarget(rightArmTargetIK, rightArmTarget);
        }
        else if (combatStateManager.currentState == combatStateManager.GrabbedState)
        {
            WalkUpdateTarget();

            if (!grabbedIndicator.activeSelf)
            {
                grabbedIndicator.SetActive(true);
            }

            WalkSwitchIKTarget(leftArmTargetIK, leftArmTarget);
            WalkSwitchIKTarget(leftLegTargetIK, leftLegTarget);
            WalkSwitchIKTarget(rightLegTargetIK, rightLegTarget);
            WalkSwitchIKTarget(rightArmTargetIK, rightArmTarget);
        }
        else if (combatStateManager.currentState == combatStateManager.GrabState)
        {
            WalkUpdateTarget();

            UpdateGrabSprites();

            WalkSwitchIKTarget(leftArmTargetIK, leftArmTarget);
            WalkSwitchIKTarget(leftLegTargetIK, leftLegTarget);
            WalkSwitchIKTarget(rightLegTargetIK, rightLegTarget);
            WalkSwitchIKTarget(rightArmTargetIK, rightArmTarget);
        }
        else
        {
            return;
        }
    }

    private void SetTransform()
    {
        transform.position = targetTransform.transform.position;
        // can't rotate if in hitstun, or freeze frame stuff
        if (!combatStateManager.isStuck && !combatStateManager.attackTimerStuck)
        {
            transform.rotation = targetTransform.transform.rotation;
        }
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

    private void LightCycleSprite()
    {

        if (combatStateManager.LightAttackState.attackTimer < lightFrameStartup[lightIndex])
        {
            // this is checking if it is already not in that sprite
            SetLightSpriteToIdx(lightIndex);
        }
        else if (combatStateManager.LightAttackState.attackTimer >= lightFrameStartup[lightIndex])
        {
            if (lightIndex < lightFrameStartup.Count)
            {
                lightIndex++;
            }
        }
    }
    private void UpdateLightAttackSprites()
    {
        LightCycleSprite();

        if (combatStateManager.LightAttackState.attackTimer < combatStateManager.lightAttackStartup)
        {
            attackSpriteRenderer.gameObject.transform.position = targetTransform.transform.position;
            attackSpriteRenderer.gameObject.transform.rotation = targetTransform.transform.rotation;
        } else
        {
            attackSpriteRenderer.gameObject.transform.position = combatStateManager.lightAttackHitbox.transform.position;
            attackSpriteRenderer.gameObject.transform.rotation = combatStateManager.lightAttackHitbox.transform.rotation;
        }
    }

    private void GrabCycleSprite()
    {

        if (combatStateManager.GrabState.attackTimer < grabFrameStartup[grabIndex])
        {
            // this is checking if it is already not in that sprite
            SetGrabSpriteToIdx(grabIndex);
        }
        else if (combatStateManager.GrabState.attackTimer >= grabFrameStartup[grabIndex])
        {
            if (grabIndex < grabFrameStartup.Count)
            {
                grabIndex++;
            }
        }
    }
    private void UpdateGrabSprites()
    {
        GrabCycleSprite();

        if (combatStateManager.GrabState.attackTimer < combatStateManager.grabStartup)
        {
            attackSpriteRenderer.gameObject.transform.position = targetTransform.transform.position;
            attackSpriteRenderer.gameObject.transform.rotation = targetTransform.transform.rotation;
        }
        else
        {
            attackSpriteRenderer.gameObject.transform.position = combatStateManager.grabHitbox.transform.position;
            attackSpriteRenderer.gameObject.transform.rotation = combatStateManager.grabHitbox.transform.rotation;
        }
    }


    //private void UpdateGrabbedSprites()
    //{
    //    GrabbedCycleSprites();

    //    attackSpriteRenderer.gameObject.transform.position = combatStateManager.transform.position;
    //    attackSpriteRenderer.gameObject.transform.rotation = combatStateManager.transform.rotation;
    //}

    //private void GrabbedCycleSprites()
    //{
    //    if (combatStateManager.GrabbedState.grabbedTimer < grabFrameStartup[grabIndex])
    //    {
    //        // this is checking if it is already not in that sprite
    //        SetGrabbedSpriteToIdx(grabbedIndex);
    //    }
    //    else if (combatStateManager.GrabState.attackTimer >= grabbedFrameStartup[grabbedIndex])
    //    {
    //        if (grabbedIndex < grabbedFrameStartup.Count)
    //        {
    //            grabbedIndex++;
    //        }
    //    }

    //    if (combatStateManager.GrabbedState.grabbedTimer >= grabbedFrameStartup[3])
    //    {
    //        grabbedIndex = 2;
    //    }
    //}

    private void HeavyCycleSprite()
    {
        
        if (combatStateManager.HeavyAttackState.attackTimer < heavyFrameStartup[heavyIndex])
        {
            // this is checking if it is already not in that sprite
            SetHeavySpriteToIdx(heavyIndex);
        } 
        else if (combatStateManager.HeavyAttackState.attackTimer >= heavyFrameStartup[heavyIndex])
        {
            if (heavyIndex < heavyFrameStartup.Count)
            {
                heavyIndex++;
            }
        }
    }

private void UpdateHeavyAttackSprites()
    {
        HeavyCycleSprite();

        if (combatStateManager.HeavyAttackState.attackTimer < combatStateManager.heavyAttackStartup)
        {
            attackSpriteRenderer.gameObject.transform.position = targetTransform.transform.position;
            attackSpriteRenderer.gameObject.transform.rotation = targetTransform.transform.rotation;
        }
        else
        {
            attackSpriteRenderer.gameObject.transform.position = combatStateManager.heavyAttackHitbox.transform.position;
            attackSpriteRenderer.gameObject.transform.rotation = combatStateManager.heavyAttackHitbox.transform.rotation;
        }
    }
}
