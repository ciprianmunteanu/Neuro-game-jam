using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class CombatEncounterProvider
{
    public const string PLAYER = "res://Presets/PlayerCombatEntityPreset.tscn";
    public static Vector2 PlayerPosition = new Vector2(-518, 0);
    public static Vector2[][] EnemyPositions =
    {
        new Vector2[] { },
        new Vector2[] { new Vector2(518, 0) },
        new Vector2[] { new Vector2(518, -140), new Vector2(518, 140) },
        new Vector2[] { new Vector2(518, -330), new Vector2(518, 0), new Vector2(518, 330) },

    };
    private const string ENEMY1 = "res://Presets/EnemyCombatEntityPreset.tscn";

    // Possible encounters
    private static CombatEncounter[] Level1Encounters =
    {
        new CombatEncounter(new List<string>() { ENEMY1, ENEMY1} ),
        new CombatEncounter(new List<string>() { ENEMY1, ENEMY1, ENEMY1} ),
    };

    private static Random random = new();


    public static CombatEncounter GetEncounter(int level)
    {
        var randIndex = random.Next(0, Level1Encounters.Count());
        return Level1Encounters.ElementAt(randIndex);
    }
}