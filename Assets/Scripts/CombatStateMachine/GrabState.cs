using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : CombatBaseState
{
    public float attackTimer;
    public bool turnedHitboxOn;
    public bool turnedHitboxOff;
    public List<bool> canHit;
    public override void EnterState(CombatStateManager combat, float number)
    {
        for (int i = 0; i < canHit.Count; i++)
        {
            canHit[i] = true;
        }
        attackTimer = 0;
        // TODO: combat.playerSpriteAnim.SetLightSpriteToIdx(0);

        // ! ! ! For some reason, triggers only check if there is on trigger enter for them if they are moving.
        // this is a fix for that. A very minor force that makes no difference.
        combat.rb.AddForce(combat.playerSpriteAnim.transform.up * 0.1f, ForceMode2D.Force);

        // reset bools
        turnedHitboxOn = false;
        turnedHitboxOff = false;

        // set move direction
        if (combat.leftStick.ReadValue().magnitude >= 0.1f)
        {
            combat.grabHitbox.transform.up = combat.leftStick.ReadValue().normalized;
        }
        else
        {
            combat.grabHitbox.transform.up = combat.playerSpriteTargetTransform.up;
        }
    }

    public override void UpdateState(CombatStateManager combat)
    {
        if (!combat.attackTimerStuck)
        {
            attackTimer += Time.deltaTime;
        }

        // startup
        if (attackTimer >= combat.grabStartup && !turnedHitboxOn)
        {
            turnedHitboxOn = true;
            combat.grabHitbox.SetActive(true);
        }

        // deactivate move
        if (attackTimer >= combat.grabStartup + combat.grabActiveHitboxDuration && !turnedHitboxOff)
        {
            turnedHitboxOff = true;
            combat.grabHitbox.SetActive(false);
        }

        // endlag + return to Idle
        if (attackTimer > combat.grabDuration)
        {
            combat.grabHitbox.SetActive(false);
            combat.SwitchState(combat.IdleState);
        }

        if (attackTimer > combat.grabDuration - combat.bufferSize)
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
        // just doing this to prevent activating hitbox after deactivating it
        turnedHitboxOn = true;

        combat.grabHitbox.SetActive(false);
        // TODO combat.playerSpriteAnim.SetLightSpriteToIdx(combat.playerSpriteAnim.lightFrameStartup.Count - 1);
    }
}
