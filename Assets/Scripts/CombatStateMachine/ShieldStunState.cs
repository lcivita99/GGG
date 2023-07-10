using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStunState : CombatBaseState
{
    public float shieldStunTimer;

    public float shieldStunLength;

    public float shieldKnockbackStrength = 100f;

    public CombatStateManager playerWhoPutYouInShieldStun;
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        shieldStunTimer = 0;
        combat.isStuck = true;
        // make shield less transparent
        combat.shield.GetComponentInChildren<SpriteRenderer>().color += new Color(0, 0, 0, 0.2f);

        playerWhoPutYouInShieldStun = combat.playerAttackingYouManager;
        playerWhoPutYouInShieldStun.attackTimerStuck = true;


        //Debug.Log("Entered Shieldstun");
        shieldStunLength = number;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        shieldStunTimer += Time.deltaTime;

        if (shieldStunTimer >= shieldStunLength)
        {
            combat.shield.GetComponentInChildren<SpriteRenderer>().color -= new Color(0, 0, 0, 0.2f);
            combat.isStuck = false;
            playerWhoPutYouInShieldStun.attackTimerStuck = false;
            combat.SwitchState(combat.ShieldState, shieldKnockbackStrength);
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        //combat.playerSpriteAnim.ShieldStunAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        combat.shield.GetComponentInChildren<SpriteRenderer>().color -= new Color(0, 0, 0, 0.2f);
        combat.isStuck = false;
        playerWhoPutYouInShieldStun.attackTimerStuck = false;
        //combat.playerAttackingYouManager
    }
}
