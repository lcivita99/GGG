using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedState : CombatBaseState
{
    private Vector2 locationToBe;
    public override void EnterState(CombatStateManager combat, float number)
    {
        combat.canMove = false;
        combat.otherPlayerCombatManager.SwitchState(combat.otherPlayerCombatManager.HoldState);

        
        locationToBe = combat.otherPlayer.transform.position + (combat.gameObject.transform.position - combat.otherPlayer.transform.position).normalized;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        combat.rb.AddForce((locationToBe - combat.rb.position) * 400 * Time.deltaTime, ForceMode2D.Impulse);
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
