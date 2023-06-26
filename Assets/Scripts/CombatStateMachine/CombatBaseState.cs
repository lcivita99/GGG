using UnityEngine;

public abstract class CombatBaseState
{
    public abstract void EnterState(CombatStateManager combat);

    public abstract void UpdateState(CombatStateManager combat);


    // ? public virtual void HandleInput(CombatStateManager combat) { }
    // ? public virtual void LogicUpdate(CombatStateManager combat) { }

    // ? public virtual void PhysicsUpdate(CombatStateManager combat) { }
    // ? public virtual void Exit() { }
}
