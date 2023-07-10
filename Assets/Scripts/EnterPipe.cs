using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPipe : MonoBehaviour
{
    public LayerMask targetLayer;
    public int pipeSide;
    private CombatStateManager player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checking if collision is in target layer
        if((targetLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            player = collision.transform.parent.GetComponent<CombatStateManager>();
            player.canMove = false;
            player.isStuck = true;

            player.SwitchState(player.PipeState, pipeSide);
            
        }
    }
}
