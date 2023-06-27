using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CombatBaseState
{
    public float lightDamageTimer = 0f;

    public override void EnterState(CombatStateManager combat)
    {
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
        //
        lightDamageTimer += Time.deltaTime;


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

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {

        // light attack - take damage
        if (collider.gameObject.layer.Equals(6) && lightDamageTimer > combat.lightAttackDuration)
        {
            combat.health -= 10;
            lightDamageTimer = 0f;
        }
        // heavy attack - take damage
        if (collider.gameObject.layer.Equals(7))
        {
            combat.health -= 20;
        }
    }

}
