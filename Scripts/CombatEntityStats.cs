public record CombatEntityStats
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
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


}