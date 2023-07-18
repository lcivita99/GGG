using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : CombatBaseState
{
    public float timer;
    public float respawnLength;
    public Vector2 respawnPos = new Vector2(0, 9.81f);

    
    public bool gotFreeCribmate;
    public float channelTimer;
    public bool channelling;
    public float timeToChannel;
    public GameObject channellingObj;
    private InteractableObject curIOScript;

    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        timer = 0f;
        gotFreeCribmate = false;
        channelling = false;
        combat.canMove = false;
        combat.isStuck = true;
        combat.health = 0f;
        combat.mainCollider.enabled = true;
        combat.invulnerableCollider.SetActive(true);
        combat.transform.position = respawnPos;
        //combat.mainCollider.enabled = false;
        //combat.invulnerableCollider.SetActive(false);
        for (int i = 0; i < combat.allPlayers.Count; i++)
        {
            Physics2D.IgnoreCollision(combat.invulnerableCollider.GetComponent<Collider2D>(), combat.allPlayers[i].invulnerableCollider.GetComponent<Collider2D>(), true);
        }
        combat.BecomeInvulnerable();
        combat.playerSpriteRenderer.enabled = true;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;

        if (timer <= respawnLength)
        {
            // fill health
            if (combat.health < 100)
                combat.health = Mathf.Floor((timer / respawnLength) * 100);
            combat.healthBarVisuals.UpdateUI();

            // become visible
            combat.playerSpriteRenderer.color = Color.Lerp(Color.clear, Color.white, timer / respawnLength);
        }
        else
        {
            if (combat.health != 100)
            {
                combat.health = 100;
                combat.healthBarVisuals.UpdateUI();
            }

            // allow move & rotate
            if (!combat.canMove)
            {
                combat.canMove = true;
            }
            if (combat.isStuck)
            {
                combat.isStuck = false;
            }
        }




        if (combat.cross.wasPressedThisFrame && !gotFreeCribmate)
        {
            Debug.Log(gotFreeCribmate);
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
                gotFreeCribmate = true;
                curIOScript.FinishChannelling(combat, false);
            }
        }


        // when condition is met
        //for (int i = 0; i < combat.allPlayers.Count; i++)
        //{
        //    Physics2D.IgnoreCollision(combat.invulnerableCollider.GetComponent<Collider2D>(), combat.allPlayers[i].invulnerableCollider.GetComponent<Collider2D>(), false);
        //}
        //combat.BecomeVulnerable();
        //combat.SwitchState(combat.PipeState);
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        combat.playerSpriteAnim.RespawnAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {

    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        throw new System.NotImplementedException();
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        for (int i = 0; i < combat.allPlayers.Count; i++)
        {
            Physics2D.IgnoreCollision(combat.invulnerableCollider.GetComponent<Collider2D>(), combat.allPlayers[i].invulnerableCollider.GetComponent<Collider2D>(), false);
        }
        combat.BecomeVulnerable();
    }
}
