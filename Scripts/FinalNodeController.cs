using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class FinalNodeController : CombatNodeController
{
    protected override CombatEncounter GetEncounter()
    {
        return CombatEncounterProvider.GetBossEncounter(1);
    }

    public override void StartEncounter(Node rootNode)
    {
        base.StartEncounter(rootNode);

        // register the ghost
        var ghostData = new GhostData("Dummy", new List<Item>());
        ghostData.Items.Add(PlayerManager.ArmorItem);
        ghostData.Items.Add(PlayerManager.WeaponItem);
        SaveGhostData(ghostData);
    }

    private void SaveGhostData(GhostData ghostData)
    {
        List<GhostData> existingData = GetExistingGhostData();

        using var ghostDataFile = FileAccess.Open("user://ghostData.json", FileAccess.ModeFlags.Write);
        existingData.Add(ghostData);
        var jsonData = JsonSerializer.Serialize(existingData);
        ghostDataFile.StoreLine(jsonData);
    }

    private List<GhostData> GetExistingGhostData()
    {
        List<GhostData> existingData = new();

        using var ghostDataFile = FileAccess.Open("user://ghostData.json", FileAccess.ModeFlags.Read);
        if(ghostDataFile != null)
        {
            var line = ghostDataFile.GetLine();

            if (!string.IsNullOrEmpty(line))
            {
                existingData = JsonSerializer.Deserialize<GhostData[]>(line).ToList();
            }
        }

        return existingData;
    }

    protected override void RoomCleared()
    {
        MapController.Instance.GenerateMap();

        base.RoomCleared();
    }


    protected override Rewards GetRewards()
    {
        var itemReward = new Sword();
        return new Rewards()
        {
            Items = new List<Item>() { itemReward }
        };
    }
}