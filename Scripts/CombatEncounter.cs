using System;
using System.Collections.Generic;

public class CombatEncounter
{
    public List<Type> EnemyTypes { get; init; }

    public CombatEncounter(List<Type> enemyPresets)
    {
        EnemyTypes = enemyPresets;
    }

}
