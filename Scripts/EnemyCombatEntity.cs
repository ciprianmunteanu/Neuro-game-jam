using Godot;
using System;
using System.Diagnostics;

public partial class EnemyCombatEntity : CombatEntity
{
    public override void TakeTurn()
    {
        Debug.WriteLine("Enemy's turn");
    }
}
