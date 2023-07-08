using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeState : CombatBaseState
{
    public float timeToExitPipe = 1.5f;
    private bool pushedOut;
    float pipeSide;
    Vector2 leftPipeExit = new Vector2(-10f, 7.69f);
    float timer;

    Vector2 dashDirection = new Vector2(0, -1f);
    public override void EnterState(CombatStateManager combat, float number, string str)
    {
        //Debug.Log("IM IN A PIPE!");
        pushedOut = false;
        combat.playerSpriteRenderer.enabled = false;
        pipeSide = number;
        combat.mainCollider.enabled = false;
        if (pipeSide == 1)
        {
            combat.transform.position = leftPipeExit;
        }

        timer = 0;
        combat.canMove = false;
        //combat.circleSprite.color = Color.blue;


        //if (combat.leftStick.ReadValue().magnitude > 0.1f)
        //{
        //    //dashDirection = combat.leftStick.ReadValue().normalized;
        //    combat.rb.AddForce(combat.dashStrength * dashDirection, ForceMode2D.Impulse);
        //}
        //else
        //{
        //    //dashDirection = combat.playerSpriteTargetTransform.up;
        //    combat.rb.AddForce(combat.dashStrength * dashDirection, ForceMode2D.Impulse);
        //}

        //combat.bufferString = "";
        //Debug.Log(combat.dashStrength);


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
            combat.rb.AddForce(combat.dashStrength * dashDirection, ForceMode2D.Impulse);
        } else if (timer >= timeToExitPipe + 0.25f)
        {
            combat.BecomeVulnerable();
            combat.SwitchState(combat.IdleState);
        }

        //if (timer >= combat.dashLength)
        //{
        //    combat.playerSpriteRenderer.enabled = true;
        //    combat.SwitchState(combat.IdleState);
        //}

        //else if (combat.lightAttackButton.wasPressedThisFrame)
        //{
        //    combat.SwitchState(combat.LightAttackState);
        //}

        //else if (combat.heavyAttackButton.wasPressedThisFrame)
        //{
        //    combat.SwitchState(combat.HeavyAttackState);
        //}

        //else if (combat.leftBumper.wasPressedThisFrame)
        //{
        //    combat.SwitchState(combat.GrabState);
        //}   
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