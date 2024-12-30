using Godot;
using System.Collections.Generic;
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
        using var ghostDataFile = FileAccess.Open("user://ghostData.json", FileAccess.ModeFlags.Write);
        var jsonData = JsonSerializer.Serialize(ghostData);
        ghostDataFile.StoreLine(jsonData);
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