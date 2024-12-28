using Godot;
using System;
using System.Collections.Generic;

public partial class MapController : Node
{
    private readonly CombatNodeController combatNodeController = new();

    public void GenerateMap(Control mapRoot)
    {
        var testButton = new Button();
        mapRoot.AddChild(testButton);
        testButton.Text = "[WIP]";
        testButton.Size = new Vector2(50, 50);
        testButton.Position = new Vector2(0, 0);
        testButton.Pressed += () =>
        {
            combatNodeController.StartEncounter(this);
            UiController.Instance.ShowMap(false);
        };
    }
}
