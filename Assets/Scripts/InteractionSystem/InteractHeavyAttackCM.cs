using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHeavyAttackCM : InteractableObject
{
    public CribmateManager cribmate;
    public override void FinishChannelling(CombatStateManager combat, bool idleState)
    {

        cribmate = GetComponent<CribmateManager>();

        if (idleState)
        {
            combat.currencyManager.ChangeCurrency(-cribmate.stats.cost);
        }
        // switch to bigger hitbox & add damage
        combat.curHeavyAttackHitbox = combat.UpgradeAttack(combat.curHeavyAttackHitbox, combat.heavyAttackHitbox);

        combat.lightAttackDamageBonus += 10;

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
