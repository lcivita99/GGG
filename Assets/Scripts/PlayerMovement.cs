using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public int playerNumber;

    private Gamepad gamepad;

    private Vector2 moveDir;
    [SerializeField] private float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gamepad = Gamepad.all[playerNumber - 1];

        SetMoveDir();
    }

    // Update is called once per frame
    void Update()
    {
        SetMoveDir();



    }

    private void FixedUpdate()
    {
        MovePlayer();
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
