using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTheFlag : MonoBehaviour
{
    public CombatStateManager[] CSMs;
    private void Awake()
    {

    }
    private void OnEnable()
    {
        CSMs = FindObjectsOfType<CombatStateManager>();
        foreach (CombatStateManager CSM in CSMs)
        {
            //Debug.Log("KeyChanged");
            CSM.IdleState.hasChannelKey = false;
        }
    }
}
