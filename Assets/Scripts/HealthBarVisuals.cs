using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarVisuals : MonoBehaviour
{
    [SerializeField] private GameObject healthObj;
    [SerializeField] private GameObject delayedHealthObj;
    [SerializeField] private CombatStateManager combatStateManager;

    private float healthScale;
    //private float timer;
    //private float delayedHealthDelay = 0.5f;
    private float delayedHealthSpeed = 1;

    void Start()
    {
        //timer = 0;
        UpdateUI();
    }

    void Update()
    {
        //if (timer < delayedHealthDelay)
        //{
        //    timer += Time.deltaTime;
        //}
        //else
        //{
        //    if (delayedHealthObj.transform.localScale != healthObj.transform.localScale)
        //    {
        //        delayedHealthObj.transform.localScale = Vector3.MoveTowards(
        //            delayedHealthObj.transform.localScale,
        //            healthObj.transform.localScale,
        //            Time.deltaTime * delayedHealthSpeed
        //            );
        //    }
        //}
        
        if (!(combatStateManager.currentState == combatStateManager.HitstunState ||
            combatStateManager.currentState == combatStateManager.GrabbedState))
        {
            delayedHealthObj.transform.localScale = Vector3.MoveTowards(
                delayedHealthObj.transform.localScale,
                healthObj.transform.localScale,
                Time.deltaTime * delayedHealthSpeed
                );
        }
    }

    public void UpdateUI()
    {
        //timer = 0;
        healthScale = combatStateManager.health / 100;
        healthObj.transform.localScale = Vector3.one - Vector3.right * (1 - healthScale);
    }
}
