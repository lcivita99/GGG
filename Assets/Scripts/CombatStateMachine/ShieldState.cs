using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : CombatBaseState
{
    public override void EnterState(CombatStateManager combat, float number)
    {
        combat.canMove = false;
        combat.shield.SetActive(true);
    }

    public override void UpdateState(CombatStateManager combat)
    {
        if (!combat.rightTrigger.isPressed)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.IdleState);
        }
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void HitOutOfState(CombatStateManager combat)
    {
        combat.shield.SetActive(false);
    }
}
