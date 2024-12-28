using Godot;
using System;

public partial class UiController : Control
{
    public static UiController Instance { get; set; }

    [Export]
    public Control CombatMenu { get; set; }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;

        CombatMenu.Hide();
        CombatMenu.Show();
    }

}
