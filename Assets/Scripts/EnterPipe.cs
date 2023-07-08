using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPipe : MonoBehaviour
{
    public LayerMask targetLayer;
    public float pipeSide;
    private CombatStateManager player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((targetLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            player = collision.transform.parent.GetComponent<CombatStateManager>();
            //
            player.canMove = false;
            player.isStuck = true;
            //player.SwitchState(player.PipeState, pipeSide);

            player.SwitchState(player.PipeState, pipeSide);
            //StartCoroutine(EnterPipeState());
            
        }
    }
    // Coroutine that waits for 2 seconds
    //private IEnumerator EnterPipeState()
    //{
    //    yield return new WaitForSeconds(2);

    //    player.canMove = true;
    //    player.isStuck = false;
    //    // Call the function after 2 seconds
    //    player.SwitchState(player.PipeState, pipeSide);
    //}

}
