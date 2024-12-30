using Godot;
using System.Diagnostics;
using System.Linq;

public partial class PlayerCombatEntity : CombatEntity
{
    public PlayerCombatEntity() : base(PlayerManager.Stats)
    {
        IsEnemy = false;

        WeaponItem = PlayerManager.WeaponItem;
        ArmorItem = PlayerManager.ArmorItem;
    }

    public static PlayerCombatEntity Instance { get; set; }

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public override void TakeTurn()
    {
        if( UiController.Instance != null)
        {
            UiController.Instance.CombatMenu.Show();
        }
    }
}
