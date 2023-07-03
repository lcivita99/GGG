using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitstunState : CombatBaseState
{
    public float hitstunTimer;
    private Vector2 hitDirection;
    private Vector2 DI;
    private float DIStrength = 0.25f;

    public float currentHitstunDuration;
    public float currentInitialHitstunDuration;

    public float moveID;

    private bool wasKnockedBack;

    private Color startColor = new Color(1, 0.7f, 0.7f, 1);
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        combat.bufferString = "";
        //Debug.Log("entered hitstun" + Time.deltaTime);
        moveID = number;
        
        
        hitstunTimer = 0;

        //Debug.Log(moveID);

        // Light attack case
        if (moveID == combat.lightAttackDamage)
        {
            currentHitstunDuration = combat.lightAttackTotalHitstunLength;
            currentInitialHitstunDuration = combat.lightAttackInitialHitstunLength;
            combat.health -= moveID * combat.lightAttackDamageMultiplier;
            hitDirection = (combat.transform.position - combat.playerAttackingYouManager.transform.position).normalized;
        }

        // heavy attack case
        else if (moveID == combat.heavyAttackDamage)
        {
            currentHitstunDuration = combat.heavyAttackTotalHitstunLength;
            currentInitialHitstunDuration = combat.heavyAttackInitialHitstunLength;
            combat.health -= moveID * combat.heavyAttackDamageMultiplier;
            hitDirection = (combat.transform.position - combat.playerAttackingYouManager.transform.position).normalized;
        }

        // throw case
        else if (moveID == combat.throwDamage)
        {
            currentHitstunDuration = combat.throwTotalHitstunLength;
            currentInitialHitstunDuration = 0;
            Physics2D.IgnoreCollision(combat.mainCollider, combat.playerAttackingYouManager.mainCollider, true);
            combat.health -= moveID * combat.throwDamageMultiplier;

            //change hit direction if in throw mode
            if (combat.playerAttackingYouManager.leftStick.ReadValue().magnitude >= 0.169f)
            {
                hitDirection = combat.playerAttackingYouManager.leftStick.ReadValue().normalized;
            }
            else
            {
                hitDirection = (combat.playerAttackingYouManager.playerSpriteTargetTransform.up).normalized;
            }
        }

        //combat.takeHeavyDamageTimer = 0;
        combat.playerAttackingYouManager.attackTimerStuck = true;
        combat.canMove = false;
        combat.isStuck = true;

        

        wasKnockedBack = false;

        combat.UpdateHealthUI();
    }

    public override void UpdateState(CombatStateManager combat)
    {
        hitstunTimer += Time.deltaTime;

        if (hitstunTimer >= combat.throwDuration &&
            Physics2D.GetIgnoreCollision(combat.mainCollider, combat.playerAttackingYouManager.mainCollider))
        {
            Physics2D.IgnoreCollision(combat.mainCollider, combat.playerAttackingYouManager.mainCollider, false);
        }

        // light
        if (moveID == combat.lightAttackDamage)
        {
            if (hitstunTimer >= combat.lightAttackInitialHitstunLength && !wasKnockedBack)
            {
                wasKnockedBack = true;
                combat.isStuck = false;
                combat.playerAttackingYouManager.attackTimerStuck = false;
                AddKnockback(combat, combat.lightAttackKnockbackStrength);
            }

            if (wasKnockedBack)
            {
                LerpFromRed(combat, combat.lightAttackInitialHitstunLength, combat.lightAttackTotalHitstunLength);
            }

            if (hitstunTimer >= combat.lightAttackTotalHitstunLength)
            {
                combat.canMove = true;
                combat.playerSpriteRenderer.color = Color.white;
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
                combat.playerAttackingYouManager.attackTimerStuck = false;
                AddKnockback(combat, combat.heavyAttackKnockbackStrength);
            }

            if (wasKnockedBack)
            {
                LerpFromRed(combat, combat.heavyAttackInitialHitstunLength, combat.heavyAttackTotalHitstunLength);
            }

            if (hitstunTimer >= combat.heavyAttackTotalHitstunLength)
            {
                combat.canMove = true;
                combat.playerSpriteRenderer.color = Color.white;
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
                combat.playerAttackingYouManager.attackTimerStuck = false;

                AddKnockback(combat, combat.throwKnockbackStrength);
            }

            if (wasKnockedBack)
            {
                LerpFromRed(combat, 0, combat.throwTotalHitstunLength);
            }


            if (hitstunTimer >= combat.throwTotalHitstunLength)
            {
                combat.canMove = true;
                combat.playerSpriteRenderer.color = Color.white;
                combat.SwitchState(combat.IdleState);
            }
        }
        // clank
        else if (moveID == 0)
        {
            if (hitstunTimer >= combat.clankHitstunDuration && !wasKnockedBack)
            {
                Debug.Log("entered clank state RED ALERT");

                wasKnockedBack = true;
                combat.isStuck = false;
                combat.playerAttackingYouManager.attackTimerStuck = false;
                AddKnockback(combat, combat.clankKnockbackStrength);

                combat.canMove = true;
                combat.SwitchState(combat.IdleState);
                // add knockback
            }
        }

        // buffer system
        if (hitstunTimer > currentHitstunDuration - combat.bufferSize)
        {
            combat.UpdateBufferInput();
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
        combat.isStuck = false;
        combat.playerAttackingYouManager.attackTimerStuck = false;
        Physics2D.IgnoreCollision(combat.mainCollider, combat.playerAttackingYouManager.mainCollider, false);
        combat.playerSpriteRenderer.color = Color.white;
    }

    private void AddKnockback(CombatStateManager combat, float hitStrength)
    {
        DI = combat.leftStick.ReadValue();
        if (DI.magnitude > 1)
        {
            DI = DI.normalized;
        }
        combat.playerSpriteRenderer.color = startColor;

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

    private void LerpFromRed(CombatStateManager combat, float initialHitstunLength, float totalHitstunLength)
    {
        combat.playerSpriteRenderer.color += Time.deltaTime * (Color.green + Color.blue) * 1.5f * (totalHitstunLength - initialHitstunLength);
    }
}
