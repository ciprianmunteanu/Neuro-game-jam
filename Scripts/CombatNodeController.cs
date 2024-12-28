
using Godot;
using System.Collections.Generic;
using System.Linq;

public class CombatNodeController : MapNodeController
{
    public void StartEncounter(Node rootNode)
    {
        // spawn the player and the enemies at the correct location on the screen
        // create the turn manager and pass those in
        // start combat
        var encounter = CombatEncounterProvider.GetEncounter(1);

        var player = GD.Load<PackedScene>(CombatEncounterProvider.PLAYER).Instantiate() as CombatEntity;
        rootNode.AddChild(player);
        player.Position = CombatEncounterProvider.PlayerPosition;
        var enemyPositions = CombatEncounterProvider.EnemyPositions[encounter.EnemyPresets.Count()];
        List<CombatEntity> combatEntities = new() { player };
        for (int i = 0; i< encounter.EnemyPresets.Count(); i++)
        {
            var enemy = GD.Load<PackedScene>(encounter.EnemyPresets[i]).Instantiate() as CombatEntity;
            rootNode.AddChild(enemy);
            enemy.Position = enemyPositions[i];
            enemy.CurrentHealth = enemy.MaxHealth;
            enemy.HealthBar.Value = enemy.CurrentHealth / enemy.MaxHealth;
            combatEntities.Add(enemy);
        }

        var turnManager = new TurnManager(combatEntities);
        turnManager.StartCombat();
    }
}
