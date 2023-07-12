using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackState : CombatBaseState
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

        combat.circleSprite.color = Color.green;
        attackTimer = 0;
        combat.playerSpriteAnim.SetLightSpriteToIdx(0);
        //combat.playerSpriteAnim.lightIndex = combat.playerSpriteAnim.lightFrameStartup.Count - 1;

        // ! ! ! For some reason, triggers only check if there is on trigger enter for them if they are moving.
        // this is a fix for that. A very minor force that makes no difference.
        combat.rb.AddForce(combat.playerSpriteAnim.transform.up * 0.1f, ForceMode2D.Force);

        // reset bools
        turnedHitboxOn = false;
        turnedHitboxOff = false;

        // set move direction
        if (combat.leftStick.ReadValue().magnitude >= 0.1f)
        {
            combat.lightAttackHitbox[combat.curLightAttackHitbox].transform.up = combat.leftStick.ReadValue().normalized;
        }
        else
        {
            combat.lightAttackHitbox[combat.curLightAttackHitbox].transform.up = combat.playerSpriteTargetTransform.up;
        }
    }

    public override void UpdateState(CombatStateManager combat)
    {
        if (!combat.attackTimerStuck)
        {
            attackTimer += Time.deltaTime;
        }

        // startup
        if (attackTimer >= combat.lightAttackStartup && !turnedHitboxOn)
        {
            turnedHitboxOn = true;
            combat.lightAttackHitbox[combat.curLightAttackHitbox].SetActive(true);

            
        }

        // deactivate move
        if (attackTimer >= combat.lightAttackStartup + combat.lightAttackActiveHitboxDuration && !turnedHitboxOff)
        {
            turnedHitboxOff = true;
            combat.lightAttackHitbox[combat.curLightAttackHitbox].SetActive(false);
        }

        // endlag + return to Idle
        if (attackTimer >= combat.lightAttackDuration)
        {
            combat.SwitchState(combat.IdleState);
        }

        if (attackTimer >= combat.lightAttackDuration - combat.bufferSize)
        {
            combat.UpdateBufferInput();
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        combat.playerSpriteAnim.LightAttackAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void ForcedOutOfState(CombatStateManager combat)
    {
        combat.lightAttackHitbox[combat.curLightAttackHitbox].SetActive(false);
        combat.playerSpriteAnim.SetLightSpriteToIdx(combat.playerSpriteAnim.lightFrameStartup.Count - 1);
    }
}
