using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : CombatBaseState
{
    public float attackTimer;
    public bool turnedHitboxOn;
    public bool turnedHitboxOff;
    public List<bool> canHit;
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        for (int i = 0; i < canHit.Count; i++)
        {
            canHit[i] = true;
        }
        combat.circleSprite.color = Color.yellow;
        attackTimer = 0;

        combat.canMove = false;

        combat.playerSpriteAnim.SetHeavySpriteToIdx(0);
        //combat.playerSpriteAnim.heavyIndex = combat.playerSpriteAnim.heavyFrameStartup.Count - 1;

        // ! ! ! For some reason, triggers only check if there is on trigger enter for them if they are moving.
        // this is a fix for that. A very minor force that makes no difference.
        combat.rb.AddForce(combat.playerSpriteAnim.transform.up * 0.1f, ForceMode2D.Force);

        // reset bools
        turnedHitboxOn = false;
        turnedHitboxOff = false;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        if (!combat.attackTimerStuck)
        {
            attackTimer += Time.deltaTime;
        }

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
        if (attackTimer >= combat.heavyAttackDuration)
        {
            combat.canMove = true;
            combat.SwitchState(combat.IdleState);
        }

        if (attackTimer >= combat.heavyAttackDuration - combat.bufferSize)
        {
            combat.UpdateBufferInput();
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        combat.playerSpriteAnim.HeavyAttackAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        combat.canMove = true;
        combat.heavyAttackHitbox.SetActive(false);
        combat.playerSpriteAnim.SetHeavySpriteToIdx(combat.playerSpriteAnim.heavyFrameStartup.Count - 1);
    }
}
