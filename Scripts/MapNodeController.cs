using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public record Rewards
{
    public int Gold { get; set; } = 0;
    public IEnumerable<Item> Items { get; set; } = new List<Item>();
}

public abstract class MapNodeController
{
    public abstract void StartEncounter(Node rootNode);
    public abstract void CleanupEncounter();

    public event Action OnRoomClear;

    protected virtual void RoomCleared()
    {
        var rewards = GetRewards();
        DisplayRewards(rewards);

        UiController.Instance.RewardsMenuOkButton.Pressed += () => ClaimRewards(rewards);

        CleanupEncounter();

        OnRoomClear?.Invoke();
    }

    private void DisplayRewards(Rewards rewards)
    {
        const int labelSizeY = 50;
        const int labelPositionX = 50;
        int currentY = 100;
        if (rewards.Gold > 0)
        {
            var goldLabel = new Label() { Text = $"{rewards.Gold} gold", Position = new Vector2(labelPositionX, currentY), Size = new Vector2(200, labelSizeY) };
            UiController.Instance.RewardsMenu.AddChild(goldLabel);
            currentY += labelSizeY;
        }

        foreach (var item in rewards.Items)
        {
            var itemLabel = new Label() { Text = $"{item.Name}(item)", Position = new Vector2(labelPositionX, currentY), Size = new Vector2(200, labelSizeY) };
            UiController.Instance.RewardsMenu.AddChild(itemLabel);
            currentY += labelSizeY;
        }

        UiController.Instance.RewardsMenu.Show();
        UiController.Instance.CombatMenu.Hide();
    }

    private void ClaimRewards(Rewards rewards)
    {
        foreach(var item in rewards.Items)
        {
            InventoryController.Instance.AddItem(item);
        }
    }

    protected abstract Rewards GetRewards();
}
