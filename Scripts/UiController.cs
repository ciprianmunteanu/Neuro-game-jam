using Godot;
using System;
using System.Collections.Generic;

public partial class UiController : Control
{
    public static UiController Instance { get; set; }

    [Export]
    public Control CombatMenu { get; set; }

    [Export]
    public Control SelectTargetPrompt { get; set; }

    private List<Button> buttons = new();

    public override void _Ready()
    {
        Instance = this;

        SelectTargetPrompt.Hide();

        PopulateCombatMenu();
    }

    private void PopulateCombatMenu()
    {
        var attackButton = new CombatActionButton(new DamageCombatAction() { Damage = 1 });
        attackButton.Position = new Vector2(-933, 440);
        attackButton.Size = new Vector2(180, 74);
        attackButton.Text = "Attack";
        CombatMenu.AddChild(attackButton);
        buttons.Add(attackButton);

        var passButton = new CombatActionButton(new PassCombatAction());
        passButton.Position = new Vector2(-733, 440);
        passButton.Size = new Vector2(180, 74);
        passButton.Text = "Pass";
        CombatMenu.AddChild(passButton);
        buttons.Add(passButton);
    }

    public void SetEnabled(bool enabled)
    {
        foreach(var button in buttons)
        {
            button.Disabled = !enabled;
        }
    }
}
