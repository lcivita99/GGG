using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CombatBaseState
{

    private float invulnerableTime;


    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        invulnerableTime = number;

        if (invulnerableTime > 0)
        {
            // become invulnerable for specified length
            //Debug.Log("Knows to have invulnerable time");
            invulnerableTime = number;
            combat.BecomeInvulnerable(invulnerableTime);
        }

        combat.canMove = true;
        combat.isStuck = false;
        if (combat.bufferString != "")
        {
            combat.SwitchState(combat.bufferDictionary[combat.bufferString]);
            combat.bufferString = "";
        }
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

        if (combat.rightTrigger.isPressed)
        {
            combat.SwitchState(combat.ShieldState);
        }

        if (combat.leftBumper.wasPressedThisFrame)
        {
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
        combat.StopCoroutine(combat.InvulnerableDelay(invulnerableTime));
        combat.BecomeVulnerable();
    }

    
}
