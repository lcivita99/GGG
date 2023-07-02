using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterAnim : MonoBehaviour
{
    private Animator animator;

    private CombatStateManager combatStateManager;

    private void OnEnable()
    {
        animator.Play("idle");
    }

    private void OnDisable()
    {
        animator.StopPlayback();
    }

    void Start()
    {
        combatStateManager = transform.parent.GetComponent<CombatStateManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
