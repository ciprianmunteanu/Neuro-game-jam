using Godot;
using System;
using System.Collections.Generic;

public partial class MapController : Node
{
    private readonly CombatNodeController combatNodeController = new();

    public override void _Ready()
    {
        combatNodeController.StartEncounter(this);
    }
}
