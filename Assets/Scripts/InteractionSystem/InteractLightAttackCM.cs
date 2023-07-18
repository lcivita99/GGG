using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractLightAttackCM : InteractableObject
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
        combat.curLightAttackHitbox = combat.UpgradeAttack(combat.curLightAttackHitbox, combat.lightAttackHitbox);
        combat.playerSpriteAnim.UpgradeLightAttackSprites();

        combat.lightAttackDamageBonus += 5;

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
