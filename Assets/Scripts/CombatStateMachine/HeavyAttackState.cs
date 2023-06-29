using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : CombatBaseState
{
    public float attackTimer;
    public bool turnedHitboxOn;
    public bool turnedHitboxOff;
    public bool canHit;
    public override void EnterState(CombatStateManager combat, float number)
    {
        canHit = true;
        combat.circleSprite.color = Color.yellow;
        attackTimer = 0;

        combat.canMove = false;

        combat.playerSpriteAnim.SetHeavySpriteToIdx(0);
        //combat.playerSpriteAnim.heavyIndex = combat.playerSpriteAnim.heavyFrameStartup.Count - 1;

        // reset bools
        turnedHitboxOn = false;
        turnedHitboxOff = false;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        attackTimer += Time.deltaTime;

        // startup
        if (attackTimer >= combat.heavyAttackStartup && !turnedHitboxOn)
        {
            turnedHitboxOn = true;
            combat.heavyAttackHitbox.SetActive(true);
            // set move direction
            if (combat.leftStick.ReadValue().magnitude >= 0.1f)
            {
                combat.heavyAttackHitbox.transform.up = combat.leftStick.ReadValue().normalized;
            }
            else
            {
                combat.heavyAttackHitbox.transform.up = combat.playerSpriteTargetTransform.up;
            }
        }

        // active move
        if (attackTimer >= combat.heavyAttackStartup + combat.heavyAttackActiveHitboxDuration && !turnedHitboxOff)
        {
            turnedHitboxOff = true;
            
            combat.heavyAttackHitbox.SetActive(false);
        }

        // endlag + return to Idle
        if (attackTimer > combat.heavyAttackDuration)
        {
            combat.canMove = true;
            combat.SwitchState(combat.IdleState);
        }

        if (attackTimer > combat.heavyAttackDuration - combat.bufferSize)
        {
            combat.UpdateBufferInput();
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
        combat.canMove = true;
        combat.heavyAttackHitbox.SetActive(false);
        combat.playerSpriteAnim.SetHeavySpriteToIdx(combat.playerSpriteAnim.heavyFrameStartup.Count - 1);
    }
}
