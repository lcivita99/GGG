using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : CombatBaseState
{
    public float holdTimer;
    public override void EnterState(CombatStateManager combat, float number)
    {
        combat.canMove = false;
        combat.grabHitbox.SetActive(false);

        holdTimer = 0;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        holdTimer += Time.deltaTime;

        if (combat.lightAttackButton.wasPressedThisFrame || combat.heavyAttackButton.wasPressedThisFrame)
        {
            if (combat.leftStick.ReadValue().magnitude > 0.169f)
            {
                combat.otherPlayerCombatManager.SwitchState(combat.otherPlayerCombatManager.HitstunState, combat.throwDamage);
                combat.SwitchState(combat.ThrowState);
            }
        }

        if (holdTimer >= combat.holdLength)
        {
            combat.otherPlayerCombatManager.SwitchState(combat.otherPlayerCombatManager.IdleState);
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
        
    }
}
