public record CombatEntityStats
{
    public double MaxHealth { get; set; }
    public double CurrentHealth { get; set; }
    public int AttackDamage { get; set; }
    public int Speed { get; set; }

    public static CombatEntityStats operator -(CombatEntityStats a, CombatEntityStats b) =>
        new CombatEntityStats()
        {
            MaxHealth = a.MaxHealth - b.MaxHealth,
            CurrentHealth = a.CurrentHealth - b.CurrentHealth,
            AttackDamage = a.AttackDamage - b.AttackDamage,
            Speed = a.Speed - b.Speed
        };

    public static CombatEntityStats operator +(CombatEntityStats a, CombatEntityStats b) =>
        new CombatEntityStats()
        {
            MaxHealth = a.MaxHealth + b.MaxHealth,
            CurrentHealth = a.CurrentHealth + b.CurrentHealth,
            AttackDamage = a.AttackDamage + b.AttackDamage,
            Speed = a.Speed + b.Speed
        };

    public static CombatEntityStats operator *(CombatEntityStats stats, double mul) =>
        new CombatEntityStats()
        {
            MaxHealth = stats.MaxHealth * mul,
            CurrentHealth = stats.CurrentHealth * mul,
            AttackDamage = (int)(stats.AttackDamage * mul),
            Speed = (int)(stats.Speed * mul)
        };
}