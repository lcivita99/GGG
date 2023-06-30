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
    public override void HitOutOfState(CombatStateManager combat)
    {
        combat.grabHitbox.SetActive(false);
    }
}
