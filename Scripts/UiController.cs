using Godot;

enum GameState { MAIN_MENU, PLAYING }

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

    private CombatActionButton BasicAttackButton;

    [Export]
    public Control SelectTargetPrompt { get; set; }

    [Export]
    public Control MainMenuScreen { get; set; }
    [Export]
    public Button MainMenuPlayButton { get; set; }
    [Export]
    public Button MainMenuExitButton { get; set; }

    private bool isMapShown = false;
    private GameState CurrentGameState = GameState.MAIN_MENU;

    public override void _Ready()
    {
        SelectTargetPrompt.Hide();
        MapMenu.Hide();
        CombatMenu.Hide();
        RewardsMenu.Hide();
        InventoryScreen.Hide();
        MainMenuScreen.Show();

        MainMenuPlayButton.Pressed += Play;
        MainMenuExitButton.Pressed += Quit;

        Instance = this;

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

    private void Play()
    {
        MapController.GenerateMap();
        MainMenuScreen.Hide();
        ShowMap(true);
    }

    private void Quit()
    {
        GetTree().Quit();
    }

    public void RefreshSkillButtonState()
    {
        foreach (var child in SkillsMenu.GetChildren())
        {
            if (child is CombatActionButton actionButton)
            {
                actionButton.RefreshButtonState();
            }
        }

        BasicAttackButton.RefreshButtonState();
    }

    private void PopulateCombatMenu()
    {
        Vector2 buttonPosition = new Vector2(0, 0);

        var basicAttackCA = new CombatAction() { Cooldown = 1, Name = "Attack" };
        basicAttackCA.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 1 });
        BasicAttackButton = new CombatActionButton(basicAttackCA);
        PositionCombatButton(BasicAttackButton, ref buttonPosition);
        BasicCombatActionsMenu.AddChild(BasicAttackButton);

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

        var passButton = new Button();
        PositionCombatButton(passButton, ref buttonPosition);
        passButton.Text = "Pass";
        passButton.Pressed += () =>
        {
            CombatManager.PassTurn();
            CombatMenu.Hide();
            PlayerCombatEntity.Instance.OnTurnEnd();
        };
        BasicCombatActionsMenu.AddChild(passButton);
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
            if(isMapShown && isInventoryShown)
            {
                ShowInventory(false);
            }
        }

        if (@event.IsActionPressed("Inventory"))
        {
            ShowInventory(!isInventoryShown);
            if (isMapShown && isInventoryShown)
            {
                ShowMap(false);
            }
        }
    }
}
