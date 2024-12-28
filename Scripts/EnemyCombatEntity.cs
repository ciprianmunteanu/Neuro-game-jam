using Godot;
using System;
using System.Diagnostics;

public partial class EnemyCombatEntity : CombatEntity
{
    public override void TakeTurn()
    {
        PlayerCombatEntity.Instance.TakeDamage(1);
        TurnManager.Instance.PassTurn();
    }
}
