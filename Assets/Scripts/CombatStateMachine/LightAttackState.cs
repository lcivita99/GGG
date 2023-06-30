using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackState : CombatBaseState
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

        combat.circleSprite.color = Color.green;
        attackTimer = 0;
        combat.playerSpriteAnim.SetLightSpriteToIdx(0);
        //combat.playerSpriteAnim.lightIndex = combat.playerSpriteAnim.lightFrameStartup.Count - 1;

        // reset bools
        turnedHitboxOn = false;
        turnedHitboxOff = false;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        attackTimer += Time.deltaTime;

        // startup
        if (attackTimer >= combat.lightAttackStartup && !turnedHitboxOn)
        {
            turnedHitboxOn = true;
            combat.lightAttackHitbox.SetActive(true);

            // set move direction
            if (combat.leftStick.ReadValue().magnitude >= 0.1f)
            {
                combat.lightAttackHitbox.transform.up = combat.leftStick.ReadValue().normalized;
            }
            else
            {
                combat.lightAttackHitbox.transform.up = combat.playerSpriteTargetTransform.up;
            }
        }

        // deactivate move
        if (attackTimer >= combat.lightAttackStartup + combat.lightAttackActiveHitboxDuration && !turnedHitboxOff)
        {
            turnedHitboxOff = true;
            combat.lightAttackHitbox.SetActive(false);
        }

        // endlag + return to Idle
        if (attackTimer > combat.lightAttackDuration)
        {
            combat.SwitchState(combat.IdleState);
        }

        if (attackTimer > combat.lightAttackDuration - combat.bufferSize)
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
        combat.lightAttackHitbox.SetActive(false);
        combat.playerSpriteAnim.SetLightSpriteToIdx(combat.playerSpriteAnim.lightFrameStartup.Count - 1);
    }
}
