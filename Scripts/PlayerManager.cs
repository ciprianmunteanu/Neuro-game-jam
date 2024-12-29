using Godot;
using System.Collections.Generic;
using System.Reflection;

public static class PlayerManager
{
    public static readonly CombatEntityStats StartingPlayerStats = new CombatEntityStats()
    {
        MaxHealth = 30,
        CurrentHealth = 30,
        AttackDamage = 10,
        Speed = 10
    };

    public static string PlayerWeaponSpritePath { get; set; }
    public static string PlayerArmorSpritePath { get; set; }

    public static CombatEntityStats Stats { get; private set; } = StartingPlayerStats;

    public static List<CombatAction> CombatActions { get; set; }

    private static PropertyInfo[] statsPropertyInfo;

    public static void InitStatsDisplay()
    {
        Vector2 pos = new Vector2() { X = 50, Y = 50 };
        statsPropertyInfo = typeof(CombatEntityStats).GetProperties();
        foreach (var statType in statsPropertyInfo)
        {
            var label = new Label()
            {
                Text = $"{statType.Name}: {statType.GetValue(Stats)}",
                Position = pos
            };
            UiController.Instance.StatsDisplay.AddChild(label);
            pos.Y += 50;
        }
    }

    public static void UpdateStats(CombatEntityStats newStats)
    {
        Stats = newStats;

        var children = UiController.Instance.StatsDisplay.GetChildren();

        for (int i=0; i < statsPropertyInfo.Length && i < children.Count; i++)
        {
            if (children[i] is Label label)
            {
                label.Text = $"{statsPropertyInfo[i].Name}: {statsPropertyInfo[i].GetValue(Stats)}";
            }
        }
    }
}