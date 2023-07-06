using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : CombatBaseState
{
    public float timer;
    public float respawnLength;
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        timer = 0f;
        combat.canMove = true;
        combat.isStuck = false;
        combat.mainCollider.enabled = true;
        combat.invulnerableCollider.SetActive(true);
        combat.transform.position = Vector2.zero;
        //combat.mainCollider.enabled = false;
        //combat.invulnerableCollider.SetActive(false);

    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;
        combat.health = Mathf.Floor((timer / respawnLength) * 100);
        combat.UpdateHealthUI();


        if (timer >= respawnLength)
        {
            combat.SwitchState(combat.IdleState);
        }

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
