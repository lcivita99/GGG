using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingState : CombatBaseState
{
    public float timer;
    public float dyingLength;
    public Vector2 dir;
    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        combat.canMove = false;
        combat.isStuck = true;
        timer = 0f;
        combat.health = 0f;
        combat.healthBarVisuals.UpdateUI();
        combat.mainCollider.enabled = false;
        combat.invulnerableCollider.SetActive(false);
        dir = vector;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;
        if (timer >= dyingLength)
        {
            
            combat.SwitchState(combat.DeadState, 0, "", dir);
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        //combat.playerSpriteAnim.DyingAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {

    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        throw new System.NotImplementedException();
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
    }
}
