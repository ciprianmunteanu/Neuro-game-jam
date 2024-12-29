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
    public Control BasicCombatActionsMenu { get; set; }
    [Export]
    public Control SkillsMenu { get; set; }

    [Export]
    public Control MapMenu { get; set; }
    [Export]
    public MapController MapController { get; set; }

    [Export]
    public Control RewardsMenu { get; set; }
    [Export]
    public Button RewardsMenuOkButton { get; set; }

    [Export]
    public Control InventoryScreen { get; set; }
    [Export]
    public Control StatsDisplay { get; set; }
    private bool isInventoryShown = false;


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
        InventoryScreen.Hide();

        Instance = this;
        MapController.GenerateMap(MapMenu);

        RewardsMenuOkButton.Pressed += RewardsMenu.Hide;
        RewardsMenuOkButton.Pressed += MapMenu.Show;

        PopulateCombatMenu();

        CombatManager.OnCombatStart += () =>
        {
            PopulateSkillMenu();
            BasicCombatActionsMenu.Show();
            SkillsMenu.Hide();
        };
        CombatManager.OnCombatClear += () =>
        {
            foreach(var child in SkillsMenu.GetChildren())
            {
                if(child is CombatActionButton actionButton)
                {
                    actionButton.Cleanup();
                }
                child.QueueFree();
            }
        };

        PlayerManager.InitStatsDisplay();
    }

    private void PopulateCombatMenu()
    {
        Vector2 buttonPosition = new Vector2(0, 0);

        var attackButton = new CombatActionButton(new DamageCombatAction() { Damage = PlayerManager.Stats.AttackDamage, Cooldown = 1, Name = "Attack" });
        PositionCombatButton(attackButton, ref buttonPosition);
        BasicCombatActionsMenu.AddChild(attackButton);
        buttons.Add(attackButton);

        var skillsButton = new Button()
        {
            Text = "Skills"
        };
        PositionCombatButton(skillsButton, ref buttonPosition);
        skillsButton.Pressed += () =>
        {
            SkillsMenu.Show();
            BasicCombatActionsMenu.Hide();
        };

        BasicCombatActionsMenu.AddChild(skillsButton);
        buttons.Add(skillsButton);

        var passButton = new Button();
        PositionCombatButton(passButton, ref buttonPosition);
        passButton.Text = "Pass";
        passButton.Pressed += CombatManager.PassTurn;
        passButton.Pressed += CombatMenu.Hide;
        BasicCombatActionsMenu.AddChild(passButton);
        buttons.Add(passButton);
    }

    private void PopulateSkillMenu()
    {
        Vector2 buttonPosition = new Vector2(0, 0);

        foreach (var item in InventoryController.Instance.GetEquippedItems())
        {
            foreach(var skill in item.Skills)
            {
                var skillButton = new CombatActionButton(skill);
                PositionCombatButton(skillButton, ref buttonPosition);
                SkillsMenu.AddChild(skillButton);
            }
        }

        var backButton = new Button()
        {
            Text = "Back"
        };
        PositionCombatButton(backButton, ref buttonPosition);
        SkillsMenu.AddChild(backButton);
        backButton.Pressed += () =>
        {
            SkillsMenu.Hide();
            BasicCombatActionsMenu.Show();
        };
    }

    private void PositionCombatButton(Button button, ref Vector2 position)
    {
        button.Position = position;
        button.Size = new Vector2(180, 74);

        position.X += 200;
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

    public void ShowInventory(bool shown)
    {
        isInventoryShown = shown;
        if (isInventoryShown)
        {
            InventoryScreen.Show();
        }
        else
        {
            InventoryScreen.Hide();

        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("Map"))
        {
            ShowMap(!isMapShown);
        }

        if (@event.IsActionPressed("Inventory"))
        {
            ShowInventory(!isInventoryShown);
        }
    }
}
