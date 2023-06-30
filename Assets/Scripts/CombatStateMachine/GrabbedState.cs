using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedState : CombatBaseState
{
    private Vector2 locationToBe;

    public float grabbedTimer;

    private bool throwAsap;
    public override void EnterState(CombatStateManager combat, float number)
    {
        combat.canMove = false;
        combat.playerAttackingYouManager.SwitchState(combat.playerAttackingYouManager.HoldState);

        grabbedTimer = 0;

        throwAsap = false;
        
        locationToBe = combat.playerAttackingYouManager.transform.position + (combat.gameObject.transform.position - combat.playerAttackingYouManager.transform.position).normalized;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        grabbedTimer += Time.deltaTime;

        combat.rb.AddForce((locationToBe - combat.rb.position) * 400 * Time.deltaTime, ForceMode2D.Impulse);

        if (combat.playerAttackingYouManager.lightAttackButton.wasPressedThisFrame || combat.playerAttackingYouManager.heavyAttackButton.wasPressedThisFrame)
        {
            throwAsap = true;
            if (grabbedTimer > combat.playerAttackingYouManager.HoldState.timeToTurnOffHitbox)
            {
                if (combat.playerAttackingYouManager.grabHitbox.activeSelf)
                {
                    combat.playerAttackingYouManager.shield.SetActive(false);
                }
                combat.playerAttackingYouManager.SwitchState(combat.playerAttackingYouManager.ThrowState);
                combat.SwitchState(combat.HitstunState, combat.throwDamage);
            }
        }

        if (grabbedTimer > combat.playerAttackingYouManager.HoldState.timeToTurnOffHitbox && throwAsap)
        {
            if (combat.playerAttackingYouManager.grabHitbox.activeSelf)
            {
                combat.playerAttackingYouManager.grabHitbox.SetActive(false);
            }
            combat.playerAttackingYouManager.SwitchState(combat.playerAttackingYouManager.ThrowState);
            combat.SwitchState(combat.HitstunState, combat.throwDamage);
        }

        if (grabbedTimer >= combat.playerAttackingYouManager.holdLength)
        {
            combat.playerAttackingYouManager.SwitchState(combat.playerAttackingYouManager.IdleState);
            combat.SwitchState(combat.IdleState);
        }

        CheckGrabberThrow(combat);
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void HitOutOfState(CombatStateManager combat)
    {
        Physics2D.IgnoreCollision(combat.playerAttackingYouManager.mainCollider, combat.mainCollider, false);
        combat.playerAttackingYouManager.SwitchState(combat.playerAttackingYouManager.IdleState);
    }

    public void CheckGrabberThrow(CombatStateManager combat)
    {
        if (combat.playerAttackingYouManager.lightAttackButton.wasPressedThisFrame || combat.playerAttackingYouManager.heavyAttackButton.wasPressedThisFrame)
        {
            if (combat.leftStick.ReadValue().magnitude > 0.169f)
            {
                combat.SwitchState(combat.HitstunState, combat.throwDamage);
                combat.playerAttackingYouManager.SwitchState(combat.ThrowState);
            }
        }
    }
}
