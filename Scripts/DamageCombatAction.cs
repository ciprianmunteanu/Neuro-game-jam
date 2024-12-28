public class DamageCombatAction : ICombatAction
{
    public bool RequiresTarget() => true;

    public float Damage { get; set; } = 0;

    public void Do(CombatEntity target)
    {
        target.TakeDamage(Damage);
    }

    public void Do() { }
}
