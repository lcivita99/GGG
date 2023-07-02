using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : CombatBaseState
{
    private float holdTimer;
    private bool hasTurnedOffHitbox;

    public float timeToTurnOffHitbox = 0.1f;
    
    //public float holdTimer;
    public override void EnterState(CombatStateManager combat, float number)
    {
        combat.playerSpriteAnim.SetGrabSpriteToIdx(combat.playerSpriteAnim.grabFrameStartup.Count - 1);

        hasTurnedOffHitbox = false;
        combat.canMove = false;

        holdTimer = 0;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        holdTimer += Time.deltaTime;

        if (holdTimer >= timeToTurnOffHitbox && !hasTurnedOffHitbox)
        {
            hasTurnedOffHitbox = true;
            combat.grabHitbox.SetActive(false);
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
        if (!hasTurnedOffHitbox)
        {
            hasTurnedOffHitbox = true;
            combat.grabHitbox.SetActive(false);
        }
    }
}
