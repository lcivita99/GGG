using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : CombatBaseState
{
    public float attackTimer;
    public override void EnterState(CombatStateManager combat, float number)
    {
        attackTimer = 0;
        // TODO turn this off when exiting state
        Physics2D.IgnoreCollision(combat.mainCollider, combat.otherPlayerCombatManager.mainCollider, true);
    }

    public override void UpdateState(CombatStateManager combat)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= combat.throwDuration)
        {
            Physics2D.IgnoreCollision(combat.mainCollider, combat.otherPlayerCombatManager.mainCollider, false);
            combat.SwitchState(combat.IdleState);
        }
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        throw new System.NotImplementedException();
    }
    public override void HitOutOfState(CombatStateManager combat)
    {
        Physics2D.IgnoreCollision(combat.mainCollider, combat.otherPlayerCombatManager.mainCollider, false);
    }
}
