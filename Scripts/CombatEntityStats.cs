public record CombatEntityStats
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public int AttackDamage { get; set; }

    public static CombatEntityStats operator -(CombatEntityStats a, CombatEntityStats b) =>
        new CombatEntityStats()
        {
            MaxHealth = a.MaxHealth - b.MaxHealth,
            CurrentHealth = a.CurrentHealth - b.CurrentHealth,
            AttackDamage = a.AttackDamage - b.AttackDamage,
        };

    public static CombatEntityStats operator +(CombatEntityStats a, CombatEntityStats b) =>
        new CombatEntityStats()
        {
            MaxHealth = a.MaxHealth + b.MaxHealth,
            CurrentHealth = a.CurrentHealth + b.CurrentHealth,
            AttackDamage = a.AttackDamage + b.AttackDamage,
        };


}