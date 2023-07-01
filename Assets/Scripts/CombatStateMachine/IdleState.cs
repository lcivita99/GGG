using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CombatBaseState
{
    //public float takeLightDamageTimer;
    //public float takeHeavyDamageTimer;


    public override void EnterState(CombatStateManager combat, float number)
    {
        //lightDamageTimer = 0f;
        combat.circleSprite.color = Color.red;
        //Debug.Log(combat.bufferString);

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
        
    }
}
