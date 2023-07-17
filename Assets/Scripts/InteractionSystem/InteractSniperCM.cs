using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSniperCM : InteractableObject
{
    public CribmateManager cribmate;
    public override void FinishChannelling(CombatStateManager combat)
    {
        cribmate = GetComponent<CribmateManager>();

        combat.currencyManager.ChangeCurrency(-cribmate.stats.cost);
        // switch to bigger hitbox & add damage

        // ! float IS THE PLACEABLE ID
        combat.SwitchState(combat.PlacingState, 1);

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
