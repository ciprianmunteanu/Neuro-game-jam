using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class CombatEncounterProvider
{
    public static Vector2 PlayerPosition = new Vector2(-300, 0);
    public static Vector2[][] EnemyPositions =
    {
        new Vector2[] { },
        new Vector2[] { new Vector2(300, 0) },
        new Vector2[] { new Vector2(300, -140), new Vector2(300, 140) },
        new Vector2[] { new Vector2(300, -330), new Vector2(300, 0), new Vector2(300, 330) },

    };

    // Possible encounters
    private static CombatEncounter[] Level1Encounters =
    {
        new CombatEncounter(new List<Type>() { typeof(GhostEnemy) } ),
    };

    private static Random random = new();


    public static CombatEncounter GetEncounter(int level)
    {
        var randIndex = random.Next(0, Level1Encounters.Count());
        return Level1Encounters.ElementAt(randIndex);
    }
}