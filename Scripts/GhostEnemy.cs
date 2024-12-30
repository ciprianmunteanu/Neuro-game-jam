using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

internal record GhostData(string PlayerName, List<Item> Items);

public partial class GhostEnemy : EnemyCombatEntity
{
    public const float StatModifier = 0.5f;

    public GhostEnemy() : base(new CombatEntityStats())
    {
        ActionsPerTurn = 2;

        SpriteResourcePath = "res://Assets/Ghost.png";

        Stats = PlayerManager.StartingPlayerStats;

        var deserializedGhostData = LoadGhostData();

        foreach(var item in deserializedGhostData.Items)
        {
            Stats = Stats + item.BaseStats;
            if(item.StatModifiers != null)
            {
                Stats = Stats + item.StatModifiers;
            }
            CombatActions.AddRange(item.Skills);

            if(item.Type == ItemType.WEAPON)
            {
                WeaponItem = item;
            }
            if(item.Type == ItemType.ARMOR)
            {
                ArmorItem = item;
            }
        }

        Stats = Stats * StatModifier;
        // Cap speed so you don't get punished for stacking speed
        if(Stats.Speed > 20)
        {
            Stats.Speed = 20;
        }

        AddBasicAttack();
    }

    private GhostData LoadGhostData()
    {
        // todo load all the saved ghosts, not just the first one
        using var ghostDataFile = FileAccess.Open("user://ghostData.json", FileAccess.ModeFlags.Read);
        string line = "";
        if (ghostDataFile != null) { 
            line = ghostDataFile.GetLine();
        }
        if (String.IsNullOrEmpty(line))
        {
            GhostData dummyGhostData = new GhostData("Dummy", new List<Item>());
            dummyGhostData.Items.Add(new Harpoon());
            dummyGhostData.Items.Add(new RobotBody());
            return dummyGhostData;
        }

        var ghostData = JsonSerializer.Deserialize<GhostData[]>(line);

        if(ghostData.Any())
        {
            var chosenGhostData = ghostData.ElementAt(new Random().Next(ghostData.Count()));

            // have a dict with the default versions of items
            // we store to disk only the name ( to find it in the dict with) and the stat modifiers
            // everything else we load from the dict
            foreach (var item in chosenGhostData.Items)
            {
                var baseItem = AllItems.ItemsList.Find(i => i.Name == item.Name);
                if (baseItem != null)
                {
                    item.LoadFromBaseItem(baseItem);
                }
            }

            return chosenGhostData;
        }


        GhostData dummyGhostData2 = new GhostData("Dummy", new List<Item>());
        dummyGhostData2.Items.Add(new Harpoon());
        dummyGhostData2.Items.Add(new RobotBody());
        return dummyGhostData2;
    }
}