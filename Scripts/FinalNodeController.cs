using Godot;
using System.Collections.Generic;

public class FinalNodeController : CombatNodeController
{
    protected override CombatEncounter GetEncounter()
    {
        return CombatEncounterProvider.GetBossEncounter(1);
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