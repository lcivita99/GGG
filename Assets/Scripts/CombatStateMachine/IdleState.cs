using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CombatBaseState
{

    private float invulnerableTime;
    public bool channelling;

    public float channelTimer;
    public float timeToChannel;
    public GameObject channellingObj;
    private InteractableObject curIOScript;

    private int curCost;

    public bool hasChannelKey = true;



    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
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
           
                channelling = false;
                combat.isStuck = false;
                combat.canMove = true;
                curIOScript.FinishChannelling(combat, true);
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
                    if (key.layer.Equals(16)) //cribmate
                    {
                        curCost = key.GetComponent<CribmateManager>().stats.cost;
                    }
                    else // its an objective
                    {
                        if (hasChannelKey && key.GetComponent<InteractableObject>().CanChannel(combat))
                        {
                            curCost = 0;
                        }
                        else
                        {
                            curCost = 200; //can't channel! JANK
                        }
                    }

                    if (combat.currencyManager.currency >= curCost)
                    {
                        channelling = true;
                        channellingObj = key;
                        curIOScript = channellingObj.GetComponent<InteractableObject>();
                        timeToChannel = curIOScript.timeToChannel;
                        break;
                    }
                    break;
                }
            }
        }

        if (combat.cross.wasReleasedThisFrame)
        {
 
            channelTimer = 0;
            combat.isStuck = false;
            combat.canMove = true;
            channelling = false;
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
        if (channelling)
        {
       
        }
        channelling = false;
        combat.StopCoroutine(combat.InvulnerableDelay(invulnerableTime));
        combat.BecomeVulnerable();
    }

    
}
