public class DamageCombatAction : CombatAction
{
    public float Damage { get; set; } = 0;

    protected override void DoEffect(CombatEntity target)
    {
        target.TakeDamage(Damage);
    }
}
