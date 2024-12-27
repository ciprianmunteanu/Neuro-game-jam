using Godot;
using System;
using System.Collections.Generic;

public partial class MapController : Node
{
    [Export]
    public Node2D RootNode { get; set; }

    public override void _Ready()
    {
        var currentMapNode = new CombatNodeController();
        currentMapNode.StartEncounter(RootNode);
    }
}
