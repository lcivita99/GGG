using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : CombatBaseState
{
    private Vector2 shieldKnockbackDir;
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        combat.canMove = false;
        combat.shield.SetActive(true);

        // number = shield knockback strength

        if (number > 0)
        {
            shieldKnockbackDir = (combat.transform.position - combat.playerAttackingYouManager.transform.position).normalized;
        }

        combat.rb.AddForce(shieldKnockbackDir * number, ForceMode2D.Impulse);
    }

    public override void UpdateState(CombatStateManager combat)
    {
        if (!combat.rightTrigger.isPressed)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.IdleState);
        }
        // exit shield with input
        // TODO figure out way to not repeat the first two lines of code in each thingy
        if (combat.lightAttackButton.wasPressedThisFrame)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.LightAttackState);
        }

        if (combat.heavyAttackButton.wasPressedThisFrame)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.HeavyAttackState);
        }

        if (combat.dashButton.wasPressedThisFrame)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.DashState);
        }

        if (combat.leftBumper.wasPressedThisFrame)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.GrabState);
        }
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        combat.shield.SetActive(false);
    }
}
