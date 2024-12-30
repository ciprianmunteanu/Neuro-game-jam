using Godot;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

public static class PlayerManager
{
    public static CombatEntityStats StartingPlayerStats => new CombatEntityStats()
    {
        MaxHealth = 70,
        CurrentHealth = 70,
        AttackDamage = 10,
        Speed = 10
    };

    public static Item WeaponItem { get; set; }
    public static Item ArmorItem { get; set; }

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
        if(Stats.CurrentHealth > Stats.MaxHealth)
        {
            Stats.CurrentHealth = Stats.MaxHealth;
        }

        var children = UiController.Instance.StatsDisplay.GetChildren();

        for (int i=0; i < statsPropertyInfo.Length && i < children.Count; i++)
        {
            if (children[i] is Label label)
            {
                label.Text = $"{statsPropertyInfo[i].Name}: {statsPropertyInfo[i].GetValue(Stats)}";
            }
        }
    }

    public static void Restart()
    {
        InventoryController.Instance.Reset();
        UpdateStats(StartingPlayerStats);
        WeaponItem = null;
        ArmorItem = null;
        CombatActions = new List<CombatAction>();
    }
}