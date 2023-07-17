using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStunState : CombatBaseState
{
    public float shieldStunTimer;

    public float shieldStunLength;

    public float shieldKnockbackStrength = 100f;

    public Vector2 dir;

    public CombatStateManager playerWhoPutYouInShieldStun;
    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        shieldStunTimer = 0;
        combat.isStuck = true;
        // make shield less transparent
        combat.shield.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);

        playerWhoPutYouInShieldStun = null;

        if (combat.playerAttackingYouManager != null)
        {
            playerWhoPutYouInShieldStun = combat.playerAttackingYouManager;
            playerWhoPutYouInShieldStun.attackTimerStuck = true;
        }
        


        //Debug.Log("Entered Shieldstun");
        shieldStunLength = number;
        dir = vector;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        shieldStunTimer += Time.deltaTime;

        if (shieldStunTimer >= shieldStunLength)
        {
            combat.shield.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            combat.isStuck = false;
            if (playerWhoPutYouInShieldStun != null)
            {
                playerWhoPutYouInShieldStun.attackTimerStuck = false;
            }
            
            combat.SwitchState(combat.ShieldState, shieldKnockbackStrength, "", dir);
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
        combat.shield.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        combat.isStuck = false;
        if (playerWhoPutYouInShieldStun != null)
        {
            playerWhoPutYouInShieldStun.attackTimerStuck = false;
        }
        
        //combat.playerAttackingYouManager
    }
}
