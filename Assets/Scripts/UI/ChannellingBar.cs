using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannellingBar : MonoBehaviour
{
    private CombatStateManager combatStateManager;

    private float scaleMultiplier = 0.5f;
    private float channelCompletion;

    void Start()
    {
        combatStateManager = GetComponentInParent<CombatStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (combatStateManager.IdleState.channelling && combatStateManager.currentState == combatStateManager.IdleState)
        {
            channelCompletion = combatStateManager.IdleState.channelTimer / combatStateManager.IdleState.timeToChannel;
            transform.localScale = new Vector3(channelCompletion * scaleMultiplier, 1, 1);
        } else
        {
            transform.localScale = Vector3.zero;
        }
    }
}
