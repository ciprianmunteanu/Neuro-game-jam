using Godot;
using System;
using System.Diagnostics;

public partial class EnemyCombatEntity : CombatEntity
{
    public override void TakeTurn()
    {
        var combatAction = new DamageCombatAction() { Damage = 1 };
        combatAction.OnActionDone += TurnManager.Instance.PassTurn;
        combatAction.Do(PlayerCombatEntity.Instance, this);
    }
}
