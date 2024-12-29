using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

internal record GhostData(string PlayerName, List<Item> Items);

public partial class GhostEnemy : EnemyCombatEntity
{
    public const float StatModifier = 0.5f;

    public GhostEnemy() : base(new CombatEntityStats())
    {
        SpriteResourcePath = "res://Assets/Ghost.png";

        Stats = PlayerManager.StartingPlayerStats;

        /*
        var dummyGhostData = new GhostData("Dummy", new List<Item>());
        dummyGhostData.Items.Add(new Harpoon());
        SaveGhostData(dummyGhostData);
        */

        var deserializedGhostData = LoadGhostData();

        foreach(var item in deserializedGhostData.Items)
        {
            Stats = Stats + item.BaseStats;
            if(item.StatModifiers != null)
            {
                Stats = Stats + item.StatModifiers;
            }
            CombatActions.AddRange(item.Skills);
        }

        Stats = Stats * StatModifier;
        // Cap speed so you don't get punished for stacking speed
        if(Stats.Speed > 20)
        {
            Stats.Speed = 20;
        }

        AddBasicAttack();
    }

    private void SaveGhostData(GhostData ghostData)
    {
        using var ghostDataFile = FileAccess.Open("user://ghostData.json", FileAccess.ModeFlags.Write);
        var jsonData = JsonSerializer.Serialize(ghostData);
        ghostDataFile.StoreLine(jsonData);
    }

    private GhostData LoadGhostData()
    {
        // todo load all the saved ghosts, not just the first one
        using var ghostDataFile = FileAccess.Open("user://ghostData.json", FileAccess.ModeFlags.Read);
        var line = ghostDataFile.GetLine();
        var ghostData = JsonSerializer.Deserialize<GhostData>(line);

        // have a dict with the default versions of items
        // we store to disk only the name ( to find it in the dict with) and the stat modifiers
        // everything else we load from the dict
        foreach (var item in ghostData.Items)
        {
            var baseItem = AllItems.ItemsList.Find(i => i.Name == item.Name);
            if(baseItem != null)
            {
                item.LoadFromBaseItem(baseItem);
            }
        }

        return ghostData;
    }
}