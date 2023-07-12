using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractLightAttackCM : InteractableObject
{
    public CribmateManager cribmate;   
    public override void FinishChannelling(CombatStateManager combat)
    {
        combat.curLightAttackHitbox = combat.UpgradeAttack(combat.curLightAttackHitbox, combat.lightAttackHitbox);

        cribmate = GetComponent<CribmateManager>();
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
        //SlotManager slot = GetComponent<SlotManager>();

        //cribmate.SwapOut();
        
    }
}
