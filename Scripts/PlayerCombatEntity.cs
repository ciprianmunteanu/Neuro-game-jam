using Godot;
using System.Diagnostics;
using System.Linq;

public partial class PlayerCombatEntity : CombatEntity
{
    public static PlayerCombatEntity Instance { get; set; }

    public override void _Ready()
    {
        MaxHealth = PlayerManager.MaxHp;
        CurrentHealth = PlayerManager.CurrentHp;
        HealthBar.Value = CurrentHealth / MaxHealth;

        Instance = this;
    }

    public override void TakeTurn()
    {
        if(UiController.Instance != null)
        {
            UiController.Instance.CombatMenu.Show();
        }
    }
}
