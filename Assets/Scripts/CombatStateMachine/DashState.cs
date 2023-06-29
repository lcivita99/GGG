using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : CombatBaseState
{
    private float dashTimer;
    public override void EnterState(CombatStateManager combat, float number)
    {
        dashTimer = 0;
        combat.canMove = false;
        combat.circleSprite.color = Color.blue;
        if (combat.leftStick.ReadValue().magnitude > 0.1f)
        {
            combat.rb.AddForce(combat.dashStrength * combat.leftStick.ReadValue().normalized, ForceMode2D.Impulse);
        } else
        {
            combat.rb.AddForce(combat.dashStrength * combat.playerSpriteTargetTransform.up, ForceMode2D.Impulse);
        }
        
        combat.bufferString = "";
        //Debug.Log(combat.dashStrength);
    }

    public override void UpdateState(CombatStateManager combat)
    {
        dashTimer += Time.deltaTime;

        if (dashTimer >= combat.dashLength)
        {
            combat.SwitchState(combat.IdleState);
        }

        else if (combat.lightAttackButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.LightAttackState);
        }

        else if (combat.heavyAttackButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.HeavyAttackState);
        }

        else if (combat.leftBumper.wasPressedThisFrame)
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

    public override void HitOutOfState(CombatStateManager combat)
    {
        
    }
}
