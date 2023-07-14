using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingState : CombatBaseState
{
    public float timer;

    public float maxPlacingTime = 50f;

    public int curX;
    public int curY;

    public GameObject turretPlaced;
    public GameObject turretHologram;

    public int placingID;

    public float timeToChangeGrid = 0.1F;

    private Vector2 moveDir;

    bool restartTimer;


    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        timer = 0f;

        placingID = Mathf.FloorToInt(number);

        combat.canMove = false;
        combat.isStuck = true;

        GridManager.instance.grid.GetXY(Vector2.zero, out curX, out curY);

        turretHologram = PlacingVars.instance.holograms[placingID];
        turretHologram = combat.InstantiateHack(turretHologram, curX, curY);

        


    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;

        //turretHologram.transform.position = new Vector2(curX, curY);
        if (combat.cross.wasPressedThisFrame && GridManager.instance.grid.placeable[curX, curY])
        {
            // place turret
            turretPlaced = PlacingVars.instance.prefabs[placingID];
            //turretPlaced = combat.InstantiateHack(turretPlaced, curX, curY);
            GridManager.instance.InstantiatePrefab(turretPlaced, curX, curY);
            combat.DestroyHack(turretHologram);
            combat.SwitchState(combat.IdleState);
            //Switch state
        }
        UpdateXYGrid(combat);
        if (timer >= maxPlacingTime)
        {
            // TODO place item at nearest available spot

            combat.canMove = true;
            combat.isStuck = false;
            // switch back to idle
            // become invulnerable for 1 second
            Debug.Log("Switched");
            combat.SwitchState(combat.IdleState, 1);
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {

    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        combat.canMove = true;
        combat.isStuck = false;
        Debug.Log("Forced out");
    }

    private void UpdateXYGrid(CombatStateManager combat)
    {
        restartTimer = false;
        moveDir = combat.playerMovement.gamepad.leftStick.ReadValue();
        if (moveDir.magnitude > 1)
        {
            moveDir = moveDir.normalized;
        }

        if (moveDir.x > 0.3f && timer > timeToChangeGrid)
        {
            if (curX < GridManager.instance.grid.width - 1)
            {
                restartTimer = true;
                curX++;
                Vector2 newPosition = GridManager.instance.grid.GetWorldPosition(curX, curY);
                turretHologram.transform.position = newPosition;
                if (!GridManager.instance.grid.placeable[curX, curY])
                {

                    turretHologram.GetComponent<SpriteRenderer>().color = Color.red;
                }
                // turretHologram.transform.position.x += 1;
            }

        }
        if (moveDir.x < -0.3f && timer > timeToChangeGrid)
        {
            if (curX > 0)
            {
                restartTimer = true;
                curX--;
                Vector2 newPosition = GridManager.instance.grid.GetWorldPosition(curX, curY);
                turretHologram.transform.position = newPosition;
                if (!GridManager.instance.grid.placeable[curX, curY])
                {
                    turretHologram.GetComponent<SpriteRenderer>().color = Color.red;
                }
                // turretHologram.transform.position.x += 1;
            }

        }
        if (moveDir.y > 0.3f && timer > timeToChangeGrid)
        {
            if (curY < GridManager.instance.grid.height - 1)
            {
                restartTimer = true;
                curY++;
                Vector2 newPosition = GridManager.instance.grid.GetWorldPosition(curX, curY);
                turretHologram.transform.position = newPosition;
                if (!GridManager.instance.grid.placeable[curX, curY])
                {
                    turretHologram.GetComponent<SpriteRenderer>().color = Color.red;
                }
                // turretHologram.transform.position.x += 1;
            }

        }
        if (moveDir.y < -0.3f && timer > timeToChangeGrid)
        {
            if (curY > 0)
            {
                restartTimer = true;
                curY--;
                Vector2 newPosition = GridManager.instance.grid.GetWorldPosition(curX, curY);
                turretHologram.transform.position = newPosition;
                if (!GridManager.instance.grid.placeable[curX, curY])
                {
                    turretHologram.GetComponent<SpriteRenderer>().color = Color.red;
                }
                // turretHologram.transform.position.x += 1;
            }
        }
        if (GridManager.instance.grid.placeable[curX, curY]) turretHologram.GetComponent<SpriteRenderer>().color = Color.green;
        if (restartTimer) timer = 0f;
    }
}
