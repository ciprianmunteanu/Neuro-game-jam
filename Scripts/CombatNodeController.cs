
using Godot;
using System.Collections.Generic;
using System.Linq;

public class CombatNodeController : MapNodeController
{
    private CombatEntity spawnedPlayer;

    public void StartEncounter(Node rootNode)
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

        CombatManager.OnCombatClear += OnRoomClear;
        CombatManager.StartCombat(combatEntities);
    }

    public void CleanupEncounter()
    {
        spawnedPlayer.Hide();
        spawnedPlayer.QueueFree();
    }

    private void OnRoomClear()
    {
        var reward = new Label() { Text = "10g", Position = new Vector2(50, 100), Size = new Vector2(200, 50) };
        UiController.Instance.RewardsMenu.AddChild(reward);
        UiController.Instance.RewardsMenu.Show();
    }

    public bool InProgress() => true;
}
