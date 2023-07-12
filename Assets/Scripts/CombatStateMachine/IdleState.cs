using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CombatBaseState
{

    private float invulnerableTime;
    private bool channelling;

    private float channelTimer;
    private float timeToChannel;
    public GameObject channellingObj;
    private InteractableObject curIOScript;

    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        invulnerableTime = number;
        channelling = false;
        channelTimer = 0;

        if (invulnerableTime > 0)
        {
            // become invulnerable for specified length
            //Debug.Log("Knows to have invulnerable time");
            invulnerableTime = number;
            combat.BecomeInvulnerable(invulnerableTime);
        }

        combat.canMove = true;
        combat.isStuck = false;
        if (combat.bufferString != "")
        {
            combat.SwitchState(combat.bufferDictionary[combat.bufferString]);
            combat.bufferString = "";
        }
    }

    public override void UpdateState(CombatStateManager combat)
    {
        if (channelling)
        {
            combat.isStuck = true;
            combat.canMove = false;
            channelTimer += Time.deltaTime;

            if (channelTimer >= timeToChannel)
            {
                curIOScript.FinishChannelling(combat);
                channelling = false;
                combat.isStuck = false;
                combat.canMove = true;
            }
        }

        if (combat.lightAttackButton.wasPressedThisFrame)
        {
            combat.isStuck = false;
            combat.canMove = true;
            combat.SwitchState(combat.LightAttackState);
        }

        else if (combat.heavyAttackButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.HeavyAttackState);
        }

        else if (combat.dashButton.wasPressedThisFrame)
        {
            combat.SwitchState(combat.DashState);
        }

        else if (combat.rightTrigger.isPressed)
        {
            combat.isStuck = false;
            combat.canMove = true;
            combat.SwitchState(combat.ShieldState);
        }

        else if (combat.leftBumper.wasPressedThisFrame)
        {
            combat.SwitchState(combat.GrabState);
        }

        else if (combat.cross.wasPressedThisFrame)
        {
            channelTimer = 0;
            var keys = new List<GameObject>(combat.interaction.interactableObjs.Keys);

            foreach (GameObject key in keys)
            {
                var tuple = combat.interaction.interactableObjs[key];
                if (tuple.Item2)
                {
                    channelling = true;
                    channellingObj = key;
                    curIOScript = channellingObj.GetComponent<InteractableObject>();
                    timeToChannel = curIOScript.timeToChannel;
                }
            }
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        combat.playerSpriteAnim.IdleAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void ForcedOutOfState(CombatStateManager combat)
    {
        combat.isStuck = false;
        combat.canMove = true;
        combat.StopCoroutine(combat.InvulnerableDelay(invulnerableTime));
        combat.BecomeVulnerable();
    }

    
}
