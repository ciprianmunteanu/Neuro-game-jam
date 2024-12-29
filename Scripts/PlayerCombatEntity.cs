using Godot;
using System.Diagnostics;
using System.Linq;

public partial class PlayerCombatEntity : CombatEntity
{
    public PlayerCombatEntity() : base(PlayerManager.Stats)
    {
        IsEnemy = false;
    }

    public static PlayerCombatEntity Instance { get; set; }

    public override void _Ready()
    {
        Instance = this;

        base._Ready();
    }

    public override void TakeTurn()
    {
        if(UiController.Instance != null)
        {
            UiController.Instance.CombatMenu.Show();
        }
    }
}
