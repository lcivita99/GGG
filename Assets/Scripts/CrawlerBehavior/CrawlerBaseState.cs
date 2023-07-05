using UnityEngine;

public abstract class CrawlerBaseState
{
    public abstract void EnterState(CrawlerStateManager crawler, float number = 0.0f, string str = "");

    public abstract void UpdateState(CrawlerStateManager crawler);

    public abstract void OnTriggerStay(CrawlerStateManager crawler, Collider2D collider);

    public abstract void OnTriggerExit(CrawlerStateManager crawler, Collider2D collider);
    public abstract void ForcedOutOfState(CrawlerStateManager crawler);

    // ? public virtual void HandleInput(CombatStateManager combat) { }
    // ? public virtual void LogicUpdate(CombatStateManager combat) { }

    // ? public virtual void PhysicsUpdate(CombatStateManager combat) { }
    // ? public virtual void Exit() { }
}