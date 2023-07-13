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
    private float moveSpeed = 500f;

    private int gridX;
    private int gridY;
    
    private int prevGridX;
    private int prevGridY;

    private GridManager gridManager;

    void Start()
    {
        gridManager = GridManager.instance;

        rb = GetComponent<Rigidbody2D>();
        gamepad = Gamepad.all[playerNumber - 1];

        combatStateManager = GetComponent<CombatStateManager>();

        SetMoveDir();

        gridManager.grid.GetXY(transform.position, out gridX, out gridY);
        gridManager.grid.SetPlaceableValue(gridX, gridY, false);

        prevGridX = gridX;
        prevGridY = gridY;
    }

    void Update()
    {
        SetMoveDir();
        PlayerGridUpdate(transform.position);
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

    public void PlayerGridUpdate(Vector2 position)
    {
        gridManager.grid.GetXY(transform.position, out gridX, out gridY);
        if (prevGridX != gridX || prevGridY != gridY)
        {
            gridManager.grid.SetPlaceableValue(prevGridX, prevGridY, true);
            gridManager.grid.SetPlaceableValue(gridX, gridY, false);
            prevGridX = gridX;
            prevGridY = gridY;
        }
    }
}
