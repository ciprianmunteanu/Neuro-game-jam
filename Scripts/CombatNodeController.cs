
using Godot;
using System.Linq;

public class CombatNodeController : MapNodeController
{
    public void StartEncounter(Node2D rootNode)
    {
        // spawn the player and the enemies at the correct location on the screen
        // create the turn manager and pass those in
        // start combat
        var encounter = CombatEncounterProvider.GetEncounter(1);

        var player = GD.Load<PackedScene>(CombatEncounterProvider.PLAYER).Instantiate() as Node2D;
        rootNode.AddChild(player);
        player.Position = CombatEncounterProvider.PlayerPosition;
        var enemyPositions = CombatEncounterProvider.EnemyPositions[encounter.EnemyPresets.Count()];
        for (int i = 0; i< encounter.EnemyPresets.Count(); i++)
        {
            var enemy = GD.Load<PackedScene>(encounter.EnemyPresets[i]).Instantiate() as Node2D;
            rootNode.AddChild(enemy);
            enemy.Position = enemyPositions[i];
        }
    }
}
