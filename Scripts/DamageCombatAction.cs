public class DamageCombatAction : CombatAction
{
    public double DamageMultiplier { get; set; } = 0;

    protected override void DoEffect(CombatEntity user, CombatEntity target)
    {
        target.TakeDamage(user.Stats.AttackDamage * DamageMultiplier);
    }
}
