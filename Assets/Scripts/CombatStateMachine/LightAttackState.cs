using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackState : CombatBaseState
{
    private float attackTimer;
    public override void EnterState(CombatStateManager combat)
    {
        combat.circleSprite.color = Color.green;
        attackTimer = 0;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > combat.lightAttackDuration)
        {
            combat.SwitchState(combat.IdleState);
        }
    }
}
