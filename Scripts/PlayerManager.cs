using System.Collections.Generic;

public static class PlayerManager
{
    public static float MaxHp { get; set; } = 30;
    public static float CurrentHp { get; set; } = 30;
    public static List<ICombatAction> CombatActions { get; set; }
}