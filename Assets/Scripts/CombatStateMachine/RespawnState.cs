using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : CombatBaseState
{
    public float timer;
    public float respawnLength;
    public Vector2 respawnPos = new Vector2(0, 9.81f);
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        timer = 0f;
        combat.canMove = true;
        combat.isStuck = false;
        combat.health = 0f;
        combat.mainCollider.enabled = true;
        combat.invulnerableCollider.SetActive(true);
        combat.transform.position = respawnPos;
        //combat.mainCollider.enabled = false;
        //combat.invulnerableCollider.SetActive(false);
        for (int i = 0; i < combat.allPlayers.Count; i++)
        {
            Physics2D.IgnoreCollision(combat.invulnerableCollider.GetComponent<Collider2D>(), combat.allPlayers[i].invulnerableCollider.GetComponent<Collider2D>(), true);
        }

        combat.BecomeInvulnerable();
    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;
        if (combat.health < 100)
        combat.health = Mathf.Floor((timer / respawnLength) * 100);
        combat.UpdateHealthUI();

        // when condition is met
        //for (int i = 0; i < combat.allPlayers.Count; i++)
        //{
        //    Physics2D.IgnoreCollision(combat.invulnerableCollider.GetComponent<Collider2D>(), combat.allPlayers[i].invulnerableCollider.GetComponent<Collider2D>(), false);
        //}
        //combat.BecomeVulnerable();
        //combat.SwitchState(combat.PipeState);
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
        for (int i = 0; i < combat.allPlayers.Count; i++)
        {
            Physics2D.IgnoreCollision(combat.invulnerableCollider.GetComponent<Collider2D>(), combat.allPlayers[i].invulnerableCollider.GetComponent<Collider2D>(), false);
        }
        combat.BecomeVulnerable();
    }
}