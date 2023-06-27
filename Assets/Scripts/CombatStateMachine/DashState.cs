using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : CombatBaseState
{
    private float dashTimer;
    public override void EnterState(CombatStateManager combat)
    {
        dashTimer = 0;
        combat.circleSprite.color = Color.blue;
        combat.rb.AddForce(combat.dashStrength * combat.leftStick.ReadValue().normalized, ForceMode2D.Impulse);
        //Debug.Log(combat.dashStrength);
    }

    public override void UpdateState(CombatStateManager combat)
    {
        dashTimer += Time.deltaTime;

        if (dashTimer >= combat.dashLength)
        {
            combat.SwitchState(combat.IdleState);
        }

        if (combat.lightAttackButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.LightAttackState);
        }

        if (combat.heavyAttackButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.HeavyAttackState);
        }
    }
}
