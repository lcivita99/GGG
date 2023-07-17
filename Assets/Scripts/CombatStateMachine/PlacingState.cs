using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingState : CombatBaseState
{
    public float timer;
    public float xTimer;
    public float yTimer;

    public float maxPlacingTime = 50f;

    public int curX;
    public int curY;

    public GameObject activePrefab;
    public GameObject hologramPrefab;
    public SpriteRenderer hologramSpriteRenderer;

    public int placingID;

    public float timeToChangeGrid = 0.1f;
    public float stickMagnitudeReq = 0.2f;

    private Vector2 moveDir;

    bool restartTimer;


    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        //timer = 0f;

        placingID = Mathf.FloorToInt(number);

        combat.canMove = false;
        combat.isStuck = true;

        GridManager.instance.grid.GetXY(Vector2.zero, out curX, out curY);

        hologramPrefab = PlacingVars.instance.holograms[placingID];
        hologramPrefab = combat.InstantiatePlaceableHack(hologramPrefab, curX, curY);

        hologramSpriteRenderer = hologramPrefab.GetComponent<SpriteRenderer>();
    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;

        //turretHologram.transform.position = new Vector2(curX, curY);
        if (combat.cross.wasPressedThisFrame && GridManager.instance.grid.placeable[curX, curY])
        {
            // place turret
            activePrefab = PlacingVars.instance.prefabs[placingID];
            
            // ! Instantiate and set team. These two must show up in this order, always together
            //activePrefab = combat.InstantiatePlaceableHack(activePrefab, curX, curY);
            activePrefab = GridManager.instance.InstantiatePrefab(activePrefab, curX, curY);
            if (activePrefab != null)
            {
                activePrefab.GetComponentInChildren<PlaceableObj>().SetTeam(combat.playerMovement.team);
                combat.DestroyHack(hologramPrefab);
                combat.SwitchState(combat.IdleState);
            }
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
        //restartTimer = false;
        moveDir = combat.playerMovement.gamepad.leftStick.ReadValue();
        if (moveDir.magnitude > 1)
        {
            moveDir = moveDir.normalized;
        }

        UpdateXTimer();
        UpdateYTimer();

        if (xTimer > timeToChangeGrid)
        {
            if (curX < GridManager.instance.grid.width - 1)
            {
                curX++;
                Vector2 newPosition = GridManager.instance.grid.GetWorldPosition(curX, curY);
                hologramPrefab.transform.position = newPosition;
                xTimer = 0;
                yTimer = 0;
            }

        }
        if (xTimer < -timeToChangeGrid)
        {
            if (curX > 0)
            {
                curX--;
                Vector2 newPosition = GridManager.instance.grid.GetWorldPosition(curX, curY);
                hologramPrefab.transform.position = newPosition;
                xTimer = 0;
                yTimer = 0;
            }

        }
        if (yTimer > timeToChangeGrid)
        {
            if (curY < GridManager.instance.grid.height - 1)
            {
                curY++;
                Vector2 newPosition = GridManager.instance.grid.GetWorldPosition(curX, curY);
                hologramPrefab.transform.position = newPosition;
                xTimer = 0;
                yTimer = 0;
            }

        }
        if (yTimer < -timeToChangeGrid)
        {
            if (curY > 0)
            {
                curY--;
                Vector2 newPosition = GridManager.instance.grid.GetWorldPosition(curX, curY);
                hologramPrefab.transform.position = newPosition;
                xTimer = 0;
                yTimer = 0;
            }
        }

        UpdateHologramColor();
    }

    private void UpdateXTimer()
    {
        if (moveDir.x > stickMagnitudeReq)
        {
            xTimer += Time.deltaTime * Mathf.Abs(moveDir.x) * 1.2f;
        }
        else if (moveDir.x < -stickMagnitudeReq)
        {
            xTimer -= Time.deltaTime * Mathf.Abs(moveDir.x) * 1.2f;
        }
        else if (xTimer != 0)
        {
            xTimer = 0;
        }
    }

    private void UpdateYTimer()
    {
        if (moveDir.y > stickMagnitudeReq)
        {
            yTimer += Time.deltaTime * Mathf.Abs(moveDir.y) * 1.2f;
        }
        else if (moveDir.y < -stickMagnitudeReq)
        {
            yTimer -= Time.deltaTime * Mathf.Abs(moveDir.y) * 1.2f;
        }
        else if (yTimer != 0)
        {
            yTimer = 0;
        }
    }

    private void UpdateHologramColor()
    {
        if (GridManager.instance.grid.placeable[curX, curY] && hologramSpriteRenderer.color != Color.white)
        {
            hologramSpriteRenderer.color = Color.white;
        }
        else if (!GridManager.instance.grid.placeable[curX, curY] && hologramSpriteRenderer.color != Color.red)
        {
            hologramSpriteRenderer.color = Color.red;
        }
    }
}
