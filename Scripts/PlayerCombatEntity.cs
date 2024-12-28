using Godot;
using System.Diagnostics;
using System.Linq;

public partial class PlayerCombatEntity : CombatEntity
{

    public override void TakeTurn(TurnManager turnManager)
    {
        if(UiController.Instance != null)
        {
            UiController.Instance.CombatMenu.Show();
        }
    }


}
