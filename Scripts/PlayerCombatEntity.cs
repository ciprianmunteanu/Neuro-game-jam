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
        base._Ready();

        Instance = this;

        if(PlayerManager.PlayerWeaponSpritePath != null)
        {
            WeaponSprite.Texture = GD.Load<Texture2D>(PlayerManager.PlayerWeaponSpritePath);
        }

        if(PlayerManager.PlayerArmorSpritePath != null)
        {
            ArmorSprite.Texture = GD.Load<Texture2D>(PlayerManager.PlayerArmorSpritePath);
        }
    }

    public override void TakeTurn()
    {
        if( UiController.Instance != null)
        {
            UiController.Instance.CombatMenu.Show();
        }
    }
}
