
using Godot;
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

        spawnedPlayer = GD.Load<PackedScene>(CombatEncounterProvider.PLAYER).Instantiate() as CombatEntity;
        rootNode.AddChild(spawnedPlayer);
        spawnedPlayer.Position = CombatEncounterProvider.PlayerPosition;
        var enemyPositions = CombatEncounterProvider.EnemyPositions[encounter.EnemyPresets.Count()];
        List<CombatEntity> combatEntities = new() { spawnedPlayer };
        for (int i = 0; i< encounter.EnemyPresets.Count(); i++)
        {
            var enemy = GD.Load<PackedScene>(encounter.EnemyPresets[i]).Instantiate() as CombatEntity;
            rootNode.AddChild(enemy);
            enemy.Position = enemyPositions[i];
            enemy.CurrentHealth = enemy.MaxHealth;
            enemy.HealthBar.Value = enemy.CurrentHealth / enemy.MaxHealth;
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
}
