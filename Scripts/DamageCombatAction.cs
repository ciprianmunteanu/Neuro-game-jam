public class DamageCombatAction : ICombatAction
{
    public float Damage { get; set; } = 0;

    public void Do(CombatEntity target)
    {
        target.TakeDamage(Damage);
    }
}
