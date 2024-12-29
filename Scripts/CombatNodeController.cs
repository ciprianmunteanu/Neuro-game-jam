﻿
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class CombatNodeController : MapNodeController
{
    private CombatEntity spawnedPlayer;

    public override void StartEncounter(Node rootNode)
    {
        // spawn the player and the enemies at the correct location on the screen
        // create the turn manager and pass those in
        // start combat
        var encounter = CombatEncounterProvider.GetEncounter(1);

        spawnedPlayer = new PlayerCombatEntity();
        rootNode.AddChild(spawnedPlayer);
        spawnedPlayer.Position = CombatEncounterProvider.PlayerPosition;
        var enemyPositions = CombatEncounterProvider.EnemyPositions[encounter.EnemyTypes.Count()];
        List<CombatEntity> combatEntities = new() { spawnedPlayer };
        for (int i = 0; i< encounter.EnemyTypes.Count(); i++)
        {
            //var enemy = new TrashMob();
            //var enemy = encounter.EnemyTypes.ElementAt(i).CreateInstance()
            var enemy = Activator.CreateInstance(encounter.EnemyTypes.ElementAt(i)) as CombatEntity;
            rootNode.AddChild(enemy);
            enemy.Position = enemyPositions[i];
            combatEntities.Add(enemy);
        }

        CombatManager.OnCombatClear += RoomCleared;
        CombatManager.StartCombat(combatEntities);
    }

    public override void CleanupEncounter()
    {
        spawnedPlayer.Hide();
        spawnedPlayer.QueueFree();
        CombatManager.OnCombatClear -= RoomCleared;
    }

    protected override Rewards GetRewards()
    {
        var itemReward = new Item(new CombatEntityStats() { }, "Sword + 1", ItemType.WEAPON);
        return new Rewards()
        {
            Items = new List<Item>() { itemReward }
        };
    }
}
