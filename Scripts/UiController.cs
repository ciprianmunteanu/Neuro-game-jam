using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UiController : Control
{
    public static UiController Instance { get; set; }

    [Export]
    public Control CombatMenu { get; set; }

    [Export]
    public Control MapMenu { get; set; }
    [Export]
    public MapController MapController { get; set; }

    [Export]
    public Control RewardsMenu { get; set; }

    [Export]
    public Control SelectTargetPrompt { get; set; }

    private List<Button> buttons = new();
    private bool isMapShown = false;

    public override void _Ready()
    {
        SelectTargetPrompt.Hide();
        MapMenu.Hide();
        CombatMenu.Hide();
        RewardsMenu.Hide();

        Instance = this;
        MapController.GenerateMap(MapMenu);

        PopulateCombatMenu();
    }

    private void PopulateCombatMenu()
    {
        var attackButton = new CombatActionButton(new DamageCombatAction() { Damage = 10 });
        attackButton.Position = new Vector2(-933, 440);
        attackButton.Size = new Vector2(180, 74);
        attackButton.Text = "Attack";
        CombatMenu.AddChild(attackButton);
        buttons.Add(attackButton);

        var passButton = new Button();
        passButton.Position = new Vector2(-733, 440);
        passButton.Size = new Vector2(180, 74);
        passButton.Text = "Pass";
        passButton.Pressed += CombatManager.PassTurn;
        passButton.Pressed += CombatMenu.Hide;
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

    public void ShowMap(bool shown)
    {
        isMapShown = shown;
        if (isMapShown)
        {
            MapMenu.Show();
        }
        else
        {
            MapMenu.Hide();
            
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("Map"))
        {
            ShowMap(!isMapShown);
        }
    }
}
