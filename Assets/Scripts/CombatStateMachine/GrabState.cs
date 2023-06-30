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

        // reset bools
        turnedHitboxOn = false;
        turnedHitboxOff = false;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        attackTimer += Time.deltaTime;

        // startup
        if (attackTimer >= combat.grabStartup && !turnedHitboxOn)
        {
            turnedHitboxOn = true;
            combat.grabHitbox.SetActive(true);

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

        // deactivate move
        if (attackTimer >= combat.grabStartup + combat.grabActiveHitboxDuration && !turnedHitboxOff)
        {
            turnedHitboxOff = true;
            combat.grabHitbox.SetActive(false);
        }

        // endlag + return to Idle
        if (attackTimer > combat.grabDuration)
        {
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

    public override void HitOutOfState(CombatStateManager combat)
    {
        combat.grabHitbox.SetActive(false);
        // TODO combat.playerSpriteAnim.SetLightSpriteToIdx(combat.playerSpriteAnim.lightFrameStartup.Count - 1);
    }
}
