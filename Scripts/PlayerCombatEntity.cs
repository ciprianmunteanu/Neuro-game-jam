using Godot;
using System;

public partial class PlayerCombatEntity : Node2D, CombatEntity
{
    public int Speed { get; set ; }

    public override void _Ready()
    {
        GD.Print("Hi");
    }

    public void TakeTurn()
    {
        
    }
}
