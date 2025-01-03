﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public record Rewards
{
    public int Gold { get; set; } = 0;
    public IEnumerable<Item> Items { get; set; } = new List<Item>();
    public int Healing { get; set; } = 0;
}

public abstract class MapNodeController
{
    public event Action OnRoomClear;

    private Rewards rewards;

    public virtual void StartEncounter(Node rootNode)
    {
        RoomCleared();
    }
    public virtual void CleanupEncounter() { }

    private static Type[] RewardableItemTypes =
    {
        typeof(Harpoon), typeof(BananaRum), typeof(Drones),
        typeof(RobotBody), typeof(ClownOutfit)
    };
    protected static Random random = new(Guid.NewGuid().GetHashCode());

    private Func<CombatEntityStats>[] itemModifierList =
    {
        () => new CombatEntityStats()
        {
            AttackDamage = 10
        },
        () => new CombatEntityStats()
        {
            MaxHealth = 10,
            CurrentHealth = 10
        },
        () => new CombatEntityStats()
        {
            Speed = 10
        },
    };

    protected virtual void RoomCleared()
    {
        rewards = GetRewards();
        DisplayRewards(rewards);

        var otherB = UiController.Instance.RewardsMenuOkButton;

        var aaaaaaaa = new Button()
        {
            Size = otherB.Size,
            Position = otherB.Position,
            Text = otherB.Text
        };
        otherB.GetParent().AddChild(aaaaaaaa);
        otherB.Hide();
        otherB.QueueFree();

        UiController.Instance.RewardsMenuOkButton = aaaaaaaa;

        UiController.Instance.RewardsMenuOkButton.Pressed += ClaimRewards;

        CleanupEncounter();

        OnRoomClear?.Invoke();
    }

    private void DisplayRewards(Rewards rewards)
    {
        bool skippedTitle = false;
        foreach(var c in UiController.Instance.RewardsMenu.GetChildren())
        {
            if(c is Label labelChild)
            {
                if(!skippedTitle)
                {
                    skippedTitle = true;
                    continue;
                }
                labelChild.Hide();
                labelChild.QueueFree();
            }
        }

        const int labelSizeY = 50;
        const int labelPositionX = 50;
        int currentY = 100;

        if(rewards.Items != null && rewards.Items.Count() > InventoryController.Instance.GetNumberOfEmptyInventorySlots())
        {
            var goldLabel = new Label() { Text = "You can't carry this many items!", Position = new Vector2(labelPositionX, currentY), Size = new Vector2(200, labelSizeY) };
            UiController.Instance.RewardsMenu.AddChild(goldLabel);
            currentY += labelSizeY;
        }

        if (rewards.Gold > 0)
        {
            var goldLabel = new Label() { Text = $"{rewards.Gold} gold", Position = new Vector2(labelPositionX, currentY), Size = new Vector2(200, labelSizeY) };
            UiController.Instance.RewardsMenu.AddChild(goldLabel);
            currentY += labelSizeY;
        }

        if (rewards.Healing > 0)
        {
            var goldLabel = new Label() { Text = $"Healed {rewards.Healing} HP", Position = new Vector2(labelPositionX, currentY), Size = new Vector2(200, labelSizeY) };
            UiController.Instance.RewardsMenu.AddChild(goldLabel);
            currentY += labelSizeY;
        }

        foreach (var item in rewards.Items ?? new List<Item>())
        {
            var itemLabel = new Label() { Text = $"{item.Name}", Position = new Vector2(labelPositionX, currentY), Size = new Vector2(200, labelSizeY) };
            UiController.Instance.RewardsMenu.AddChild(itemLabel);
            currentY += labelSizeY;
        }

        UiController.Instance.RewardsMenu.Show();
        UiController.Instance.CombatMenu.Hide();
    }

    private void ClaimRewards()
    {
        foreach(var item in rewards.Items)
        {
            InventoryController.Instance.AddItem(item);
        }

        if(rewards.Healing > 0)
        {
            PlayerManager.UpdateStats(PlayerManager.Stats + new CombatEntityStats() { CurrentHealth = rewards.Healing });
        }

        MapController.Instance.EnableConnectedMapButtons();

        UiController.Instance.RewardsMenu.Hide();
        UiController.Instance.MapMenu.Show();
    }

    protected Item GetRandomItemReward()
    {
        // step 1: Choose the base item
        var baseItemType = RewardableItemTypes[random.Next(RewardableItemTypes.Count())];
        var item = Activator.CreateInstance(baseItemType) as Item;

        // step 2: roll for rarity
        int nrOfModifiers = GetRandomItemRarity();

        List<int> modifierIndexes = new() { 0, 1, 2 };
        for(int i = 0; i<nrOfModifiers; i++)
        {
            int modifierIndex = modifierIndexes[random.Next(modifierIndexes.Count)];
            item.StatModifiers = item.StatModifiers + itemModifierList[modifierIndex].Invoke();
            modifierIndexes.Remove(modifierIndex);
        }

        return item;
    }

    protected virtual int GetRandomItemRarity()
    {
        var randNr = random.Next(100);
        if (randNr < 40)
        {
            return 1;
        }
        if (randNr < 70)
        {
            return 2;
        }
        if (randNr < 90)
        {
            return 3;
        }
        return 3;
    }

    protected abstract Rewards GetRewards();
}
