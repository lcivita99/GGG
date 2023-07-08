using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterState : CombatBaseState
{
    public float splatterTimer;

    public float splatterLength = 0.5f;

    public Vector2 splatterDirection;

    public float bounceStrength;
    public float bounceMultiplier;
    public float weakBounceMultiplier = 1;
    public float strongBounceMultiplier = 1.469f;
    public bool hasBounced;

    public string bounceType;

    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        bounceStrength = number;
        splatterTimer = 0;
        combat.canMove = false;
        combat.isStuck = true;

        bounceType = str;
        if (bounceType == "hitstun")
        {
            combat.BecomeInvulnerable(splatterLength + combat.splatterInvulnerableTime);
            if (combat.HitstunState.moveID == combat.lightAttackDamage)
            {
                bounceMultiplier = weakBounceMultiplier;
            }
            else if (combat.HitstunState.moveID == combat.throwDamage || combat.HitstunState.moveID == combat.heavyAttackDamage)
            {
                bounceMultiplier = strongBounceMultiplier;
            }
        }

        else if (bounceType == "dash")
        {
            combat.BecomeInvulnerable(splatterLength);
            bounceMultiplier = strongBounceMultiplier;
        }
        

        hasBounced = false;

        combat.splatterSpriteAnim.transform.up = splatterDirection;
        combat.splatterSpriteAnim.SetActive(true);

        combat.playerSpriteRenderer.enabled = false;
        //combat.mainCollider.enabled = false;

        

        combat.bufferString = "";
    }

    public override void UpdateState(CombatStateManager combat)
    {
        splatterTimer += Time.deltaTime;

        if (bounceType == "hitstun")
        {
            if (splatterTimer >= splatterLength / 1.2f && !hasBounced)
            {
                hasBounced = true;
                combat.isStuck = false;
                combat.rb.AddForce(splatterDirection * combat.dashStrength * bounceMultiplier * (1 - bounceStrength), ForceMode2D.Impulse);
            }

            if (splatterTimer >= splatterLength)
            {
                //combat.mainCollider.enabled = true;
                combat.canMove = true;
                combat.isStuck = false;
                combat.playerSpriteRenderer.enabled = true;
                combat.splatterSpriteAnim.SetActive(false);

                combat.SwitchState(combat.IdleState, combat.splatterInvulnerableTime);
            }
        }

        else if (bounceType == "dash")
        {
            if (splatterTimer >= splatterLength / 1.2f && !hasBounced)
            {
                hasBounced = true;
                combat.isStuck = false;
                float dotProduct = Vector2.Dot(combat.DashState.dashDirection, splatterDirection);
                Vector2 reflectionDirection = combat.DashState.dashDirection - 2f * dotProduct * splatterDirection;
                splatterDirection = reflectionDirection;
                combat.rb.AddForce(splatterDirection * combat.dashStrength * bounceMultiplier * (1 - bounceStrength), ForceMode2D.Impulse);
            }

            if (splatterTimer >= splatterLength)
            {
                //combat.mainCollider.enabled = true;
                combat.canMove = true;
                combat.isStuck = false;
                combat.playerSpriteRenderer.enabled = true;
                combat.splatterSpriteAnim.SetActive(false);

                combat.SwitchState(combat.IdleState);
            }
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        //combat.playerSpriteAnim.SplatterAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        combat.mainCollider.enabled = true;
        combat.canMove = true;
        combat.isStuck = false;
        combat.playerSpriteRenderer.enabled = true;
        combat.splatterSpriteAnim.SetActive(false);
    }
}
