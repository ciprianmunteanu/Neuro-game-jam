using Godot;
using System;
using System.Diagnostics;

public partial class EnemyCombatEntity : CombatEntity
{
    public override void TakeTurn(TurnManager turnManager)
    {
        Debug.WriteLine("Enemy's turn");
    }
}
