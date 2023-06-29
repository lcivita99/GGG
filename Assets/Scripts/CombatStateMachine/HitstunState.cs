using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitstunState : CombatBaseState
{
    private float hitstunTimer;
    private Vector2 hitDirection;
    private Vector2 DI;
    private float DIStrength = 0.25f;

    private float moveID;

    private bool wasKnockedBack;
    public override void EnterState(CombatStateManager combat, float number)
    {
        //Debug.Log("entered hitstun" + Time.deltaTime);
        moveID = number;
        combat.health -= moveID;
        combat.UpdateHealthUI();
        hitstunTimer = 0;

        //combat.takeHeavyDamageTimer = 0;

        combat.canMove = false;
        combat.isStuck = true;

        hitDirection = (combat.transform.position - combat.otherPlayer.transform.position).normalized;

        wasKnockedBack = false;

    }

    public override void UpdateState(CombatStateManager combat)
    {
        hitstunTimer += Time.deltaTime;



        // light
        if (moveID == combat.lightAttackDamage)
        {
            if (hitstunTimer >= combat.lightAttackInitialHitstunLength && !wasKnockedBack)
            {
                wasKnockedBack = true;
                combat.isStuck = false;
                AddKnockback(combat, combat.lightAttackKnockbackStrength);
            }

            if (hitstunTimer >= combat.lightAttackTotalHitstunLength)
            {
                combat.canMove = true;
                combat.SwitchState(combat.IdleState);
            }
        }
        // heavy
        else if (moveID == combat.heavyAttackDamage)
        {
            if (hitstunTimer >= combat.heavyAttackInitialHitstunLength && !wasKnockedBack)
            {
                //combat.otherPlayerCombatManager.HeavyAttackState.canHit = false;
                wasKnockedBack = true;
                combat.isStuck = false;
                AddKnockback(combat, combat.heavyAttackKnockbackStrength);
            }

            if (hitstunTimer >= combat.heavyAttackTotalHitstunLength)
            {
                combat.canMove = true;
                combat.SwitchState(combat.IdleState);
            }
        }
        // throw
        else if (moveID == combat.throwDamage)
        {
            if (!wasKnockedBack)
            {
                wasKnockedBack = true;
                combat.isStuck = false;
                if (combat.otherPlayerCombatManager.leftStick.ReadValue().magnitude >= 0.169f)
                {
                    hitDirection = combat.otherPlayerCombatManager.leftStick.ReadValue().normalized;
                }
                AddKnockback(combat, combat.throwKnockbackStrength);
            }
            
            
            if (hitstunTimer >= combat.throwTotalHitstunLength)
            {
                combat.canMove = true;
                combat.SwitchState(combat.IdleState);
            }
        }
        // clank
        else if (moveID == 0)
        {
            if (hitstunTimer >= combat.clankHitstunDuration && !wasKnockedBack)
            {
                wasKnockedBack = true;
                combat.isStuck = false;
                AddKnockback(combat, combat.clankKnockbackStrength);

                combat.canMove = true;
                combat.SwitchState(combat.IdleState);

                // add knockback
            }
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
        combat.isStuck = false;
    }

    private void AddKnockback(CombatStateManager combat, float hitStrength)
    {
        DI = combat.leftStick.ReadValue();
        if (DI.magnitude > 1)
        {
            DI = DI.normalized;
        }

        combat.rb.AddForce((hitDirection + DI*DIStrength) * hitStrength, ForceMode2D.Impulse);
    }

    private void AddThrowKnockback(CombatStateManager combat, float hitStrength)
    {
        DI = combat.leftStick.ReadValue();
        if (DI.magnitude > 1)
        {
            DI = DI.normalized;
        }

        combat.rb.AddForce((hitDirection + DI * DIStrength) * hitStrength, ForceMode2D.Impulse);
    }
}
