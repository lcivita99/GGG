using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeState : CombatBaseState
{
    public float timeToExitPipe = 1.5f;
    private bool pushedOut;
    private bool becameVuln;
    float pipeSide;
    Vector2 leftPipeExit = new Vector2(-10f, 7.69f);
    Vector2 rightPipeExit = new Vector2(10f, 7.69f);
    float timer;

    Vector2 dashDirection = new Vector2(0, -1f);
    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        //Debug.Log("IM IN A PIPE!");
        pushedOut = false;
        becameVuln = false;
        combat.playerSpriteRenderer.enabled = false;
        pipeSide = number;
        combat.mainCollider.enabled = false;
        if (pipeSide == 1)
        {
            combat.transform.position = leftPipeExit;
        } else if (pipeSide == 2)
        {
            combat.transform.position = rightPipeExit;
        }

        timer = 0;
        combat.canMove = false;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        timer += Time.deltaTime;

        if (timer >= timeToExitPipe && !pushedOut)
        {
            combat.canMove = true;
            combat.isStuck = false;
            pushedOut = true;
            combat.playerSpriteRenderer.enabled = true;
            combat.playerSpriteTargetTransform.up = Vector3.down;
            combat.playerSpriteAnim.SetTransform();
            combat.rb.AddForce(combat.dashStrength * dashDirection, ForceMode2D.Impulse);
        }
        else if (timer >= timeToExitPipe + 0.1f && !becameVuln)
        {
            becameVuln = true;
            combat.BecomeVulnerable();
            //combat.SwitchState(combat.IdleState);
        }
        else if (timer >= timeToExitPipe + 0.3f)
        {
            //combat.BecomeVulnerable();
            combat.SwitchState(combat.IdleState);
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        combat.playerSpriteAnim.PipeAnimUpdate();
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
    }
}