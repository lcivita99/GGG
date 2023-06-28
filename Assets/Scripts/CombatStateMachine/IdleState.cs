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

        //if (takeLightDamageTimer >= combat.attackTriggerTime)
        //{
        //    takeLightDamageTimer = 0;
        //    combat.SwitchState(combat.HitstunState, combat.lightAttackDamage);
        //} 
        //if (takeHeavyDamageTimer >= combat.attackTriggerTime)
        //{
        //    takeHeavyDamageTimer = 0;
        //    combat.SwitchState(combat.HitstunState, combat.heavyAttackDamage);
        //}
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        //// light attack - take damage
        //if (collider.gameObject.layer.Equals(6))
        //{
        //    takeLightDamageTimer += Time.deltaTime;
        //}


        //// heavy attack - take damage
        //if (collider.gameObject.layer.Equals(7))
        //{
        //    takeHeavyDamageTimer += Time.deltaTime;
        //}
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        //if (collider.gameObject.layer.Equals(6) || collider.gameObject.layer.Equals(7))
        //{
        //    takeLightDamageTimer = 0f;
        //    takeHeavyDamageTimer = 0f;
        //}
    }

    public override void HitOutOfState(CombatStateManager combat)
    {
        
    }
}
