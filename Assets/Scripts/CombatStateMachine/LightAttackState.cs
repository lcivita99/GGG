using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackState : CombatBaseState
{
    private float attackTimer;
    private bool turnedHitboxOn;
    private bool turnedHitboxOff;
    public override void EnterState(CombatStateManager combat)
    {
        combat.circleSprite.color = Color.green;
        attackTimer = 0;

        


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

        // active move
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
    }
}
