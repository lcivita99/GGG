using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterState : CombatBaseState
{
    public float splatterTimer;

    public float splatterLength = 0.5f;

    public Vector2 splatterDirection;

    public bool hasBounced;
    public override void EnterState(CombatStateManager combat, float number)
    {
        splatterTimer = 0;
        combat.canMove = false;
        combat.isStuck = true;

        hasBounced = false;

        combat.splatterSpriteAnim.transform.up = splatterDirection;
        combat.splatterSpriteAnim.SetActive(true);

        combat.playerSpriteRenderer.color = Color.clear;
        combat.mainCollider.enabled = false;

        combat.bufferString = "";
    }

    public override void UpdateState(CombatStateManager combat)
    {
        splatterTimer += Time.deltaTime;

        if (splatterTimer >= splatterLength / 2 && !hasBounced)
        {
            hasBounced = true;
            combat.rb.AddForce(splatterDirection * combat.dashStrength, ForceMode2D.Impulse);
        }

        if (splatterTimer >= splatterLength)
        {
            combat.mainCollider.enabled = true;
            combat.canMove = true;
            combat.isStuck = false;
            combat.playerSpriteRenderer.color = Color.white;
            combat.splatterSpriteAnim.SetActive(false);
            
            combat.SwitchState(combat.IdleState, combat.splatterInvulnerableTime);
        }
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        combat.mainCollider.enabled = true;
        combat.canMove = true;
        combat.isStuck = false;
        combat.playerSpriteRenderer.color = Color.white;
        combat.splatterSpriteAnim.SetActive(false);
    }
}
