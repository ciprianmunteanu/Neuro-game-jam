public interface ICombatAction
{
    public bool RequiresTarget();
    public void Do(CombatEntity target);
    public void Do();
}