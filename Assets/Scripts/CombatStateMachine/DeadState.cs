using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : CombatBaseState
{
    public float timer;
    public float deadLength;

    public int coinsLost = 3;
    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        timer = 0f;
        combat.untargettable = true;
        combat.health = 0f;
        combat.healthBarVisuals.UpdateUI();
        combat.mainCollider.enabled = false;
        combat.invulnerableCollider.SetActive(false);

        // set up death anim
        combat.playerSpriteAnim.deathAnim.transform.up =
            vector;
        combat.playerSpriteAnim.deathAnim.transform.position = combat.transform.position;
        combat.playerSpriteRenderer.color = Color.clear;
        combat.playerSpriteRenderer.enabled = false;
        combat.playerSpriteAnim.deathAnim.SetActive(true);

        combat.currencyManager.ChangeCurrency(-coinsLost);

        // TODO Instantiate 3 coins at death

        combat.SpawnDeathCoins();
    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;


        if (timer >= deadLength)
        {
            
            combat.SwitchState(combat.RespawnState);
        }

    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        //combat.playerSpriteAnim.DeadAnimUpdate();
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