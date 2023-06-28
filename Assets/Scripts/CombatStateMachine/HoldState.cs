using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : CombatBaseState
{
    public override void EnterState(CombatStateManager combat, float number)
    {

    }

    public override void UpdateState(CombatStateManager combat)
    {

    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        throw new System.NotImplementedException();
    }
    public override void HitOutOfState(CombatStateManager combat)
    {
        throw new System.NotImplementedException();
    }
}
