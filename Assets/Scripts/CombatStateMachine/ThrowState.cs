using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : CombatBaseState
{
    public override void EnterState(CombatStateManager combat)
    {

    }

    public override void UpdateState(CombatStateManager combat)
    {
        
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        // light attack - take damage
        if (collider.gameObject.layer.Equals(6))
        {
            combat.health -= 10;
        }
        // heavy attack - take damage
        if (collider.gameObject.layer.Equals(7))
        {
            combat.health -= 20;
        }
    }
}
