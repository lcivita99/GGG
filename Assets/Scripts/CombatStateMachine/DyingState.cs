using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingState : CombatBaseState
{
    public float timer;
    public float dyingLength;
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        Debug.Log("Dedge");
        combat.canMove = false;
        combat.isStuck = true;
        timer = 0f;
        combat.health = 0f;
        Debug.Log("Dedge");
        combat.UpdateHealthUI();
        combat.mainCollider.enabled = false;
        combat.invulnerableCollider.SetActive(false);

    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;
        if (timer >= dyingLength) combat.SwitchState(combat.DeadState);

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
