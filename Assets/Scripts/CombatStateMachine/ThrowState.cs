using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : CombatBaseState
{
    public float attackTimer;

    //private List<CombatStateManager> playersYouAreGrabbing;
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        attackTimer = 0;
        //playersYouAreGrabbing = combat.PlayersYouAreAttacking();
        // TODO turn this off when exiting state
        // ignore all players you are grabbing
        //for (int i = 0; i < playersYouAreGrabbing.Count; i++)
        //{
        //    Physics2D.IgnoreCollision(combat.mainCollider, playersYouAreGrabbing[i].mainCollider, true);
        //}
        
    }

    public override void UpdateState(CombatStateManager combat)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= combat.throwDuration)
        {
            //for (int i = 0; i < playersYouAreGrabbing.Count; i++)
            //{
            //    Physics2D.IgnoreCollision(combat.mainCollider, playersYouAreGrabbing[i].mainCollider, false);
            //}
            combat.SwitchState(combat.IdleState);
        }

        if (attackTimer >= combat.throwDuration - combat.bufferSize)
        {
            combat.UpdateBufferInput();
        }
        // buffer system
        
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        throw new System.NotImplementedException();
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        //for (int i = 0; i < playersYouAreGrabbing.Count; i++)
        //{
        //    Physics2D.IgnoreCollision(combat.mainCollider, playersYouAreGrabbing[i].mainCollider, false);
        //}
    }
}
