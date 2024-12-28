using Godot;
using System;
using System.Diagnostics;

public partial class EnemyCombatEntity : CombatEntity
{
    public override void TakeTurn()
    {
        var combatAction = new DamageCombatAction() { Damage = 1 };
        combatAction.OnActionDone += CombatManager.PassTurn;
        combatAction.Do(PlayerCombatEntity.Instance, this);
    }

    protected override void OnDeath()
    {
        CombatManager.RemoveEntityFromCombat(this);
        
        // TODO death animation
        Hide();

        QueueFree();
    }
}
