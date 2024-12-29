using Godot;
using System.Collections.Generic;
using System.Text.Json;

internal record GhostData(string PlayerName, List<Item> Items);

public partial class GhostEnemy : EnemyCombatEntity
{
    public GhostEnemy() : base(new CombatEntityStats())
    {
        SpriteResourcePath = "res://Assets/Ghost.png";
        
        var startingStats = PlayerManager.StartingPlayerStats;


        var dummyGhostData = new GhostData("Dummy", new List<Item>());
        dummyGhostData.Items.Add(new Item(new CombatEntityStats() { AttackDamage = 15}, "Ghost sword", ItemType.WEAPON));
        SaveGhostData(dummyGhostData);
        var deserializedGhostData = LoadGhostData();

        // load a player ghost
        // set the stats
        // set the items
        // set the sprite
    }

    private void SaveGhostData(GhostData ghostData)
    {
        using var ghostDataFile = FileAccess.Open("user://ghostData.json", FileAccess.ModeFlags.Write);
        var jsonData = JsonSerializer.Serialize(ghostData);
        ghostDataFile.StoreLine(jsonData);
    }

    private GhostData LoadGhostData()
    {
        using var ghostDataFile = FileAccess.Open("user://ghostData.json", FileAccess.ModeFlags.Read);
        var line = ghostDataFile.GetLine();
        var ghostData = JsonSerializer.Deserialize<GhostData>(line);
        return ghostData;
    }
}