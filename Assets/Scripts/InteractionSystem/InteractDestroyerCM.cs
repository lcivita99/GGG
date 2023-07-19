using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDestroyerCM : InteractableObject
{
    public CribmateManager cribmate;
    public override void FinishChannelling(CombatStateManager combat, bool idleState)
    {
        cribmate = GetComponent<CribmateManager>();

        if (idleState)
        {
            combat.currencyManager.ChangeCurrency(-cribmate.stats.cost);
            // switch to bigger hitbox & add damage
            // ! 0 IS THE TURRET ID
            combat.SwitchState(combat.PlacingState, 3, "idle");
        }
        else
        {
            combat.SwitchState(combat.PlacingState, 3, "respawn");
        }


        // replace cribmate
        ReplaceCribmate();
    }


    private void ReplaceCribmate()
    {
        //cribmate = GetComponent<CribmateManager>();
        if (cribmate.stats.slot == 0)
        {
            CheapSlotManager.instance.ChangeSlot();
        }
        else if (cribmate.stats.slot == 1)
        {
            MediumSlotManager.instance.ChangeSlot();
        }
        else if (cribmate.stats.slot == 2)
        {
            ExpensiveSlotManager.instance.ChangeSlot();
        }
    }
}