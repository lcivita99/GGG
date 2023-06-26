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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            combat.SwitchState(combat.LightAttackState);
        }
    }
}
