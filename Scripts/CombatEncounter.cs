using System.Collections.Generic;

public class CombatEncounter
{
    public List<string> EnemyPresets { get; init; }

    public CombatEncounter(List<string> enemyPresets)
    {
        EnemyPresets = enemyPresets;
    }

}
