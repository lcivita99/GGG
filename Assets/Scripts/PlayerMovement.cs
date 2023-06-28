using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private CombatStateManager combatStateManager;

    public int playerNumber;

    public Gamepad gamepad;

    private Vector2 moveDir;
    [SerializeField] private float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gamepad = Gamepad.all[playerNumber - 1];

        combatStateManager = GetComponent<CombatStateManager>();

        SetMoveDir();
    }

    void Update()
    {
        SetMoveDir();
    }

    private void FixedUpdate()
    {
        if (combatStateManager.canMove) MovePlayer();

        if (combatStateManager.isStuck) rb.velocity = Vector2.zero;
    }

    private void SetMoveDir()
    {
        moveDir = gamepad.leftStick.ReadValue();
        if (moveDir.magnitude > 1)
        {
            moveDir = moveDir.normalized;
        }
    }

    private void MovePlayer()
    {
        rb.AddForce(moveDir * moveSpeed, ForceMode2D.Force);
    }
}
