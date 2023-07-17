using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : CombatBaseState
{
    private float holdTimer;
    private bool hasTurnedOffHitbox;

    public float timeToTurnOffHitbox = 0.1f;

    private Vector2 startPos;
    
    //public float holdTimer;
    public override void EnterState(CombatStateManager combat, float number, string str, Vector2 vector)
    {
        combat.playerSpriteAnim.SetGrabSpriteToIdx(combat.playerSpriteAnim.grabFrameStartup.Count - 1);

        startPos = combat.rb.position;

        hasTurnedOffHitbox = false;
        combat.canMove = false;

        holdTimer = 0;
    }

    public override void UpdateState(CombatStateManager combat)
    {
        holdTimer += Time.deltaTime;

        combat.rb.AddForce((startPos - combat.rb.position) * 1000f * Time.deltaTime, ForceMode2D.Impulse);

        if (holdTimer >= timeToTurnOffHitbox && !hasTurnedOffHitbox)
        {
            hasTurnedOffHitbox = true;
            combat.grabHitbox.SetActive(false);
        }
    }

    public override void LateUpdateState(CombatStateManager combat)
    {
        //combat.playerSpriteAnim.HoldAnimUpdate();
    }

    public override void OnTriggerStay(CombatStateManager combat, Collider2D collider)
    {
        
    }

    public override void OnTriggerExit(CombatStateManager combat, Collider2D collider)
    {
        
    }
    public override void ForcedOutOfState(CombatStateManager combat)
    {
        if (!hasTurnedOffHitbox)
        {
            hasTurnedOffHitbox = true;
            combat.grabHitbox.SetActive(false);
        }
        combat.canMove = true;
        combat.isStuck = false;
    }
}
