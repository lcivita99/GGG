using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingState : CombatBaseState
{
    public float timer;

    public float maxPlacingTime = 50f;

    public int curX;
    public int curY;

    public GameObject turretPlaced;
    public GameObject turretHologram;

    public int placingID;

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
        //if (combat.leftStick.ReadValue().x > 0.1f && timer > 0.2f)
        //    {
        //        curX++;
        //        timer = 0f;
        //    }
        //else if (combat.leftStick.ReadValue().x < 0.1f && timer > 0.2f)
        //{
        //    curX--;
        //    timer = 0f;
        //}
    }
}
