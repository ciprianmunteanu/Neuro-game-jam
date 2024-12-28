public class PassCombatAction : ICombatAction
{
    public bool RequiresTarget() => false;

    public void Do(CombatEntity target)
    {
    }

    public void Do()
    {
        TurnManager.Instance.PassTurn();
        //UiController.Instance.CombatMenu.Hide();
    }
}