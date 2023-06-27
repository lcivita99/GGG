using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpriteAnim : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Vector3 moveDir;
    private PlayerMovement playerMovement;

    [SerializeField] GameObject player;
    [SerializeField] GameObject targetTransform;

    [SerializeField] private Transform rightLegBase;
    [SerializeField] private Transform leftLegBase;
    [SerializeField] private Transform rightArmBase;
    [SerializeField] private Transform leftArmBase;
    
    [SerializeField] private Transform rightLegTarget;
    [SerializeField] private Transform leftLegTarget;
    [SerializeField] private Transform rightArmTarget;
    [SerializeField] private Transform leftArmTarget;
    
    [SerializeField] private Transform rightLegTargetIK;
    [SerializeField] private Transform leftLegTargetIK;
    [SerializeField] private Transform rightArmTargetIK;
    [SerializeField] private Transform leftArmTargetIK;

    private Vector3 rightLegBaseIKOffset = new Vector3(0.2f, -1, 0) / 2;
    private Vector3 leftLegIKOffset = new Vector3(0.2f, -1, 0) / 2;

    //private Gamepad gamepad;
    void Start()
    {
        //moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerRb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        InvokeRepeating("SetTransform", 0.2f, 0.07f);
        //gamepad = Gamepad.all[0];
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = playerMovement.gamepad.leftStick.ReadValue();
        if (moveDir.magnitude > 1)
        {
            moveDir = moveDir.normalized;
        }
        targetTransform.transform.up = Vector3.Lerp(targetTransform.transform.up, moveDir, Time.deltaTime * 25);
        targetTransform.transform.position = player.transform.position;
        //if (moveDir.magnitude > 0.1f)
        //{
        //    transform.up = Vector3.Lerp(transform.up, moveDir.normalized, Time.deltaTime * 25);
        //}

        WalkUpdateTarget();

        WalkSwitchIKTarget(leftArmTargetIK, leftArmTarget);
        WalkSwitchIKTarget(leftLegTargetIK, leftLegTarget);
        WalkSwitchIKTarget(rightLegTargetIK, rightLegTarget);
        WalkSwitchIKTarget(rightArmTargetIK, rightArmTarget);
    }

    private void SetTransform()
    {
        transform.position = targetTransform.transform.position;
        transform.rotation = targetTransform.transform.rotation;
        
    }

    void WalkSwitchIKTarget(Transform IKTransform, Transform IKConstant)
    {
        if (Vector3.Distance(IKTransform.position, IKConstant.position) > 1)
        {
            IKTransform.position = IKConstant.position;
        }
    }

    void WalkUpdateTarget()
    {
        rightLegTarget.position = rightLegBase.position + (-transform.up + transform.right) / 2 + moveDir;
        rightArmTarget.position = rightArmBase.position + (transform.up + transform.right) / 2 + moveDir;
        leftLegTarget.position = leftLegBase.position + (-transform.up + -transform.right) / 2 + moveDir;
        leftArmTarget.position = leftArmBase.position + (transform.up + -transform.right) / 2 + moveDir;
    }
}
