using Godot;
using System.Collections.Generic;

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
    public Button MainMenuSettingsButton { get; set; }
    [Export]
    public Button MainMenuExitButton { get; set; }

    [Export]
    public Control SettingsMenu { get; set; }
    [Export]
    public CheckButton FullScreenButton { get; set; }
    [Export]
    public Button SettingsBackButton { get; set; }

    [Export]
    public ColorRect ActionPointRect1 { get; set; }
    [Export]
    public ColorRect ActionPointRect2 { get; set; }


    [Export]
    public Label SkillDescriptionLabel { get; set; }

    [Export]
    public Label ItemStatsLabel { get; set; }

    private bool isMapShown = false;
    private GameState CurrentGameState = GameState.MAIN_MENU;

    private Button SkillsButton { get; set; }
    private Button PassButton { get; set; }
    private Button SkillsBackButton { get; set; }

    public override void _Ready()
    {
        SelectTargetPrompt.Hide();
        MapMenu.Hide();
        CombatMenu.Hide();
        RewardsMenu.Hide();
        InventoryScreen.Hide();
        MainMenuScreen.Show();
        SettingsMenu.Hide();

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

        MainMenuSettingsButton.Pressed += () =>
        {
            MainMenuScreen.Hide();
            SettingsMenu.Show();
        };

        SettingsBackButton.Pressed += () =>
        {
            MainMenuScreen.Show();
            SettingsMenu.Hide();
        };

        FullScreenButton.Toggled += (toggleMode) => DisplayServer.WindowSetMode(toggleMode ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
    }

    private void Play()
    {
        MapController.GenerateMap();
        MainMenuScreen.Hide();
        ShowMap(true);
        CurrentGameState = GameState.PLAYING;
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

    public void RefreshActionPointIndicator()
    {
        int playerActionsLeft = PlayerCombatEntity.Instance.ActionsLeft;
        ActionPointRect1.Color = new Color(0.5f, 0.5f, 0.5f);
        ActionPointRect2.Color = new Color(0.5f, 0.5f, 0.5f);
        if (playerActionsLeft > 0)
        {
            ActionPointRect1.Color = new Color(0, 1, 0);
        }
        if(playerActionsLeft > 1)
        {
            ActionPointRect2.Color = new Color(0, 1, 0);
        }

    }

    private void PopulateCombatMenu()
    {
        Vector2 buttonPosition = new Vector2(0, 0);

        var basicAttackCA = new CombatAction() { Cooldown = 1, Name = "Attack", Description = "Deal 100% AD damage" };
        basicAttackCA.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 1 });
        BasicAttackButton = new CombatActionButton(basicAttackCA);
        PositionCombatButton(BasicAttackButton, ref buttonPosition);
        BasicCombatActionsMenu.AddChild(BasicAttackButton);

        SkillsButton = new Button()
        {
            Text = "Skills"
        };
        PositionCombatButton(SkillsButton, ref buttonPosition);
        SkillsButton.Pressed += () =>
        {
            SkillsMenu.Show();
            BasicCombatActionsMenu.Hide();
        };

        BasicCombatActionsMenu.AddChild(SkillsButton);

        PassButton = new Button();
        PositionCombatButton(PassButton, ref buttonPosition);
        PassButton.Text = "Pass";
        PassButton.Pressed += () =>
        {
            CombatManager.PassTurn();
            CombatMenu.Hide();
            PlayerCombatEntity.Instance.OnTurnEnd();
        };
        BasicCombatActionsMenu.AddChild(PassButton);
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

        SkillsBackButton = new Button()
        {
            Text = "Back"
        };
        PositionCombatButton(SkillsBackButton, ref buttonPosition);
        SkillsMenu.AddChild(SkillsBackButton);
        SkillsBackButton.Pressed += () =>
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
        if ((SkillsBackButton?.IsHovered() ?? false) || (SkillsButton?.IsHovered() ?? false) || (PassButton?.IsHovered() ?? false))
        {
            SkillDescriptionLabel.Text = "";
        }
        if(CurrentGameState == GameState.PLAYING)
        {
            if (@event.IsActionPressed("Map"))
            {
                ShowMap(!isMapShown);
                if (isMapShown && isInventoryShown)
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
}
