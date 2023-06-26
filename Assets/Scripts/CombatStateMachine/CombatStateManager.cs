using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateManager : MonoBehaviour
{

    CombatBaseState currentState;
    public IdleState IdleState = new IdleState();
    public LightAttackState LightAttackState = new LightAttackState();
    //public HeavyAttackState HeavyAttackState = new HeavyAttackState();
    //public HeavyAttackState HeavyAttackState = new HeavyAttackState();


    // add an instance of every state. 

    public SpriteRenderer circleSprite;

    public float lightAttackDuration = 0.3f;

    void Start()
    {
        circleSprite = GetComponent<SpriteRenderer>();


        currentState = IdleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }


    public void SwitchState(CombatBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
