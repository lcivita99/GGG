using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : CombatBaseState
{
    private Vector2 shieldKnockbackDir;
    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        combat.canMove = false;

        combat.shield.SetActive(true);

        // number = shield knockback strength

        //if (number > 0)
        //{
        //    shieldKnockbackDir = (combat.transform.position - combat.playerAttackingYouManager.transform.position).normalized;
        //}

        combat.rb.AddForce(vector * number, ForceMode2D.Impulse);

        combat.shield.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
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

        else if (combat.heavyAttackButton.wasPressedThisFrame)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.HeavyAttackState);
        }

        else if (combat.dashButton.wasPressedThisFrame)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.DashState);
        }

        else if (combat.leftBumper.wasPressedThisFrame)
        {
            combat.canMove = true;
            combat.shield.SetActive(false);
            combat.SwitchState(combat.GrabState);
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        combat.playerSpriteAnim.ShieldAnimUpdate();
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
