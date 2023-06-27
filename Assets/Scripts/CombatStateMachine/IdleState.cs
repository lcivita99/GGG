using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CombatBaseState
{
    public override void EnterState(CombatStateManager combat)
    {
        combat.circleSprite.color = Color.red;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        if (combat.lightAttackButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.LightAttackState);
        }

        if (combat.heavyAttackButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.HeavyAttackState);
        }

        if (combat.dashButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.DashState);
        }
    }
}
