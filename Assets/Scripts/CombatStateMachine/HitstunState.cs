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

    public string moveID;
    public string attackMod;

    private bool wasKnockedBack;

    private Color startColor = new Color(1, 0.7f, 0.7f, 1);

    public float bulletHitstunLength = 0.3f;
    public float bulletDamage = 5;
    public float bulletPushStrength = 150;

    public float sniperHitstunLength = 0.4f;
    public float sniperDamage = 15;
    public float sniperPushStrength = 400;


    //TODO: TEST:
    // Declare the delegate (if using non-generic pattern).
    public delegate void HitEventHandler(CombatStateManager combat);

    // Declare the event using the delegate.
    public event HitEventHandler OnHit;



    public bool IsPlayerAttack()
    {
        return moveID != "bullet" && moveID != "sniper";
    }
    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        //Debug.Log("entered hitstun");
        combat.bufferString = "";
        //Debug.Log("entered hitstun" + Time.deltaTime);
        moveID = str;
        
        
        hitstunTimer = 0;

        OnHit?.Invoke(combat);

        //Debug.Log(moveID);

        // Light attack case
        if (moveID == "lightAttack")
        {
            currentHitstunDuration = combat.lightAttackTotalHitstunLength;
            currentInitialHitstunDuration = combat.lightAttackInitialHitstunLength;
            combat.health -= combat.playerAttackingYouManager.lightAttackDamage + combat.playerAttackingYouManager.lightAttackDamageBonus;
            combat.healthBarVisuals.UpdateUI();
            hitDirection = (combat.transform.position - combat.playerAttackingYouManager.transform.position).normalized;
        }

        // heavy attack case
        else if (moveID == "heavyAttack")
        {
            currentHitstunDuration = combat.heavyAttackTotalHitstunLength;
            currentInitialHitstunDuration = combat.heavyAttackInitialHitstunLength;
            combat.health -= combat.playerAttackingYouManager.heavyAttackDamage + combat.playerAttackingYouManager.heavyAttackDamageBonus;
            combat.healthBarVisuals.UpdateUI();
            hitDirection = (combat.transform.position - combat.playerAttackingYouManager.transform.position).normalized;
        }

        // throw case
        else if (moveID == "throw")
        {
            currentHitstunDuration = combat.throwTotalHitstunLength;
            currentInitialHitstunDuration = 0;
            Physics2D.IgnoreCollision(combat.mainCollider, combat.playerAttackingYouManager.mainCollider, true);
            combat.health -= combat.playerAttackingYouManager.throwDamage * combat.throwDamageMultiplier;
            combat.healthBarVisuals.UpdateUI();


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

        else if (moveID == "bullet")
        {
            currentHitstunDuration = bulletHitstunLength;
            currentInitialHitstunDuration = 0;
            hitDirection = vector;
            combat.health -= bulletDamage;
            combat.healthBarVisuals.UpdateUI();
        }

        else if (moveID == "sniper")
        {
            currentHitstunDuration = sniperHitstunLength;
            currentInitialHitstunDuration = 0;
            hitDirection = vector;
            combat.health -= sniperDamage;
            combat.healthBarVisuals.UpdateUI();
        }

        //combat.takeHeavyDamageTimer = 0;
        if (IsPlayerAttack())
        {
            combat.playerAttackingYouManager.attackTimerStuck = true;
        }
        
        combat.canMove = false;
        combat.isStuck = true;

        

        wasKnockedBack = false;

        //combat.UpdateHealthUI();
    }

    public override void UpdateState(CombatStateManager combat)
    {
        hitstunTimer += Time.deltaTime;

        if (combat.playerAttackingYouManager != null)
        {
            if (hitstunTimer >= combat.throwDuration &&
                Physics2D.GetIgnoreCollision(combat.mainCollider, combat.playerAttackingYouManager.mainCollider))
            {
                Physics2D.IgnoreCollision(combat.mainCollider, combat.playerAttackingYouManager.mainCollider, false);
            }
        }


        // light
        if (moveID == "lightAttack")
        {
            if (hitstunTimer >= combat.lightAttackInitialHitstunLength && !wasKnockedBack)
            {
                wasKnockedBack = true;
                combat.isStuck = false;
                combat.playerAttackingYouManager.attackTimerStuck = false;
                if (combat.health <= 0) combat.SwitchState(combat.DyingState, 0, "", hitDirection);
                else 
                {
                    //combat.healthBarVisuals.UpdateUI();
                    AddKnockback(combat, combat.lightAttackKnockbackStrength);
                }
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
        else if (moveID == "heavyAttack")
        {
            if (hitstunTimer >= combat.heavyAttackInitialHitstunLength && !wasKnockedBack)
            {
                //combat.otherPlayerCombatManager.HeavyAttackState.canHit = false;
                wasKnockedBack = true;
                combat.isStuck = false;
                combat.playerAttackingYouManager.attackTimerStuck = false;
                if (combat.health <= 0) combat.SwitchState(combat.DyingState, 0, "", hitDirection);
                else
                {
                    //combat.healthBarVisuals.UpdateUI();
                    AddKnockback(combat, combat.heavyAttackKnockbackStrength);
                }

                
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
        else if (moveID == "throw")
        {
            if (!wasKnockedBack)
            {
                wasKnockedBack = true;
                combat.isStuck = false;
                combat.playerAttackingYouManager.attackTimerStuck = false;
                if (combat.health <= 0) combat.SwitchState(combat.DyingState, 0, "", hitDirection);
                else
                {
                    //combat.healthBarVisuals.UpdateUI();
                    AddKnockback(combat, combat.throwKnockbackStrength);
                }


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

        else if (moveID == "bullet")
        {
            if (!wasKnockedBack)
            {
                wasKnockedBack = true;
                combat.isStuck = false;
                if (combat.health <= 0) combat.SwitchState(combat.DyingState, 0, "", hitDirection);
                else
                {
                    //combat.healthBarVisuals.UpdateUI();


                    AddKnockback(combat, bulletPushStrength);
                }


            }

            if (wasKnockedBack)
            {
                LerpFromRed(combat, 0, bulletHitstunLength);
            }


            if (hitstunTimer >= bulletHitstunLength)
            {
                combat.canMove = true;
                combat.playerSpriteRenderer.color = Color.white;
                combat.SwitchState(combat.IdleState);
            }
        }

        else if (moveID == "sniper")
        {
            if (!wasKnockedBack)
            {
                wasKnockedBack = true;
                combat.isStuck = false;
                if (combat.health <= 0) combat.SwitchState(combat.DyingState, 0, "", hitDirection);
                else
                {
                    //combat.healthBarVisuals.UpdateUI();

                    AddKnockback(combat, sniperPushStrength);
                }


            }

            if (wasKnockedBack)
            {
                LerpFromRed(combat, 0, sniperHitstunLength);
            }


            if (hitstunTimer >= sniperHitstunLength)
            {
                combat.canMove = true;
                combat.playerSpriteRenderer.color = Color.white;
                combat.SwitchState(combat.IdleState);
            }
        }


        // clank
        else if (moveID == "")
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

    public override void LateUpdateState(CombatStateManager combat)
    {
        //combat.playerSpriteAnim.HitstunAnimUpdate();
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
        if (combat.playerAttackingYouManager != null)
        {
            combat.playerAttackingYouManager.attackTimerStuck = false;
            Physics2D.IgnoreCollision(combat.mainCollider, combat.playerAttackingYouManager.mainCollider, false);
        }
       
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
