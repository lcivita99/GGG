using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedState : CombatBaseState
{
    private Vector2 locationToBe;

    public float grabbedTimer;

    private bool throwAsap;

    private CombatStateManager playerWhoGrabbedYou;
    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        combat.canMove = false;
        combat.playerAttackingYouManager.SwitchState(combat.playerAttackingYouManager.HoldState);

        playerWhoGrabbedYou = combat.playerAttackingYouManager;

        grabbedTimer = 0;

        throwAsap = false;
        
        locationToBe = playerWhoGrabbedYou.transform.position + (combat.gameObject.transform.position - playerWhoGrabbedYou.transform.position).normalized;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        grabbedTimer += Time.deltaTime;

        combat.rb.AddForce((locationToBe - combat.rb.position) * 400 * Time.deltaTime, ForceMode2D.Impulse);

        if (grabbedTimer < playerWhoGrabbedYou.HoldState.timeToTurnOffHitbox)
        {
            if (playerWhoGrabbedYou.lightAttackButton.wasPressedThisFrame || playerWhoGrabbedYou.heavyAttackButton.wasPressedThisFrame)
            {
                throwAsap = true;   
            }
            //if (grabbedTimer > playerWhoGrabbedYou.HoldState.timeToTurnOffHitbox)
            //{
            //    if (playerWhoGrabbedYou.grabHitbox.activeSelf)
            //    {
            //        playerWhoGrabbedYou.shield.SetActive(false);
            //    }
            //    playerWhoGrabbedYou.currentState.ForcedOutOfState(combat);
            //    playerWhoGrabbedYou.SwitchState(playerWhoGrabbedYou.ThrowState);
            //    combat.playerSpriteAnim.grabbedIndicator.SetActive(false);
            //    combat.SwitchState(combat.HitstunState, combat.throwDamage);
            //}
        }

        else
        {
            if (throwAsap || (playerWhoGrabbedYou.lightAttackButton.wasPressedThisFrame || playerWhoGrabbedYou.heavyAttackButton.wasPressedThisFrame))
            {
                combat.canMove = false;
                if (playerWhoGrabbedYou.grabHitbox.activeSelf)
                {
                    playerWhoGrabbedYou.grabHitbox.SetActive(false);
                }
                playerWhoGrabbedYou.currentState.ForcedOutOfState(playerWhoGrabbedYou);
                playerWhoGrabbedYou.SwitchState(playerWhoGrabbedYou.ThrowState);
                combat.playerSpriteAnim.grabbedIndicator.SetActive(false);
                //Debug.Log("ran twice");
                combat.SwitchState(combat.HitstunState, 0, "throw");
            }
            
        }

        if (grabbedTimer >= playerWhoGrabbedYou.holdLength)
        {
            playerWhoGrabbedYou.currentState.ForcedOutOfState(playerWhoGrabbedYou);
            playerWhoGrabbedYou.SwitchState(playerWhoGrabbedYou.IdleState);
            combat.playerSpriteAnim.grabbedIndicator.SetActive(false);
            combat.SwitchState(combat.IdleState);
        }

        //CheckGrabberThrow(combat);

        if (playerWhoGrabbedYou.currentState == playerWhoGrabbedYou.HitstunState)
        {
            //combat.canMove = true;
            Physics2D.IgnoreCollision(playerWhoGrabbedYou.mainCollider, combat.mainCollider, false);
            combat.playerSpriteAnim.grabbedIndicator.SetActive(false);
            combat.SwitchState(combat.IdleState);
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        combat.playerSpriteAnim.GrabbedAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        //combat.canMove = true;
        Physics2D.IgnoreCollision(playerWhoGrabbedYou.mainCollider, combat.mainCollider, false);
        combat.playerSpriteAnim.grabbedIndicator.SetActive(false);
        playerWhoGrabbedYou.currentState.ForcedOutOfState(playerWhoGrabbedYou);
        playerWhoGrabbedYou.SwitchState(playerWhoGrabbedYou.IdleState);
    }

    //public void CheckGrabberThrow(CombatStateManager combat)
    //{
    //    if (playerWhoGrabbedYou.lightAttackButton.wasPressedThisFrame || playerWhoGrabbedYou.heavyAttackButton.wasPressedThisFrame)
    //    {
    //        if (combat.leftStick.ReadValue().magnitude > 0.169f)
    //        {
    //            combat.SwitchState(combat.HitstunState, combat.throwDamage);
    //            playerWhoGrabbedYou.currentState.ForcedOutOfState(playerWhoGrabbedYou);
    //            playerWhoGrabbedYou.SwitchState(playerWhoGrabbedYou.ThrowState);
    //        }
    //    }
    //}
}
