using System;

public partial class PlayerCombatEntity : CombatEntity
{
    public PlayerCombatEntity() : base(PlayerManager.Stats)
    {
        IsEnemy = false;

        WeaponItem = PlayerManager.WeaponItem;
        ArmorItem = PlayerManager.ArmorItem;

        SpriteResourcePath = "res://Assets/Neuro.png";
    }

    public static PlayerCombatEntity Instance { get; set; }

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        if(UiController.Instance != null)
        {
            UiController.Instance.RefreshSkillButtonState();
            UiController.Instance.CombatMenu.Show();
        }
    }
}
