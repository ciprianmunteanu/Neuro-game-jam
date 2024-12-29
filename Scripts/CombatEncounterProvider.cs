using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class CombatEncounterProvider
{
    public const int MAX_NR_SUMMONS = 4;

    public static Vector2 PlayerPosition = new Vector2(-500, 0);
    public static Vector2[][] EnemyPositions =
    {
        new Vector2[] { },
        new Vector2[] { new Vector2(500, 0) },
        new Vector2[] { new Vector2(500, -140), new Vector2(500, 140) },
        new Vector2[] { new Vector2(500, -330), new Vector2(500, 0), new Vector2(500, 330) },

    };

    public static Vector2[] PlayerSummonPositions =
    {
        new Vector2(-750, -300),
        new Vector2(-750, -100),
        new Vector2(-750, 100),
        new Vector2(-750, 300)
    };

    public static Vector2[] EnemySummonPositions =
    {
        new Vector2(750, -300),
        new Vector2(750, -100),
        new Vector2(750, 100),
        new Vector2(750, 300)
    };

    // Possible encounters
    private static CombatEncounter[] Level1Encounters =
    {
        new CombatEncounter(new List<Type>() { typeof(GhostEnemy) } ),
        new CombatEncounter(new List<Type>() { typeof(TrashMob), typeof(TrashMob), typeof(TrashMob) } ),
        new CombatEncounter(new List<Type>() { typeof(TrashMob), typeof(DifficultTrashMob) } ),
    };

    private static Random random = new();


    public static CombatEncounter GetEncounter(int level)
    {
        var randIndex = random.Next(0, Level1Encounters.Count());
        return Level1Encounters.ElementAt(randIndex);
    }
}