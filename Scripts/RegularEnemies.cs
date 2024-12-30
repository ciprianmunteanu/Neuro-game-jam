public partial class TrashMob : EnemyCombatEntity
{
    public TrashMob() : base(new CombatEntityStats()
    {
        MaxHealth = 10,
        CurrentHealth = 10,
        AttackDamage = 7,
        Speed = 2
    })
    {
        SpriteResourcePath = "res://Assets/EvilFumo.png";
        AddBasicAttack();
    }
}

public partial class DifficultTrashMob : EnemyCombatEntity
{
    public DifficultTrashMob() : base(new CombatEntityStats()
    {
        MaxHealth = 20,
        CurrentHealth = 20,
        AttackDamage = 15,
        Speed = 2
    })
    {
        SpriteResourcePath = "res://Assets/Evil.png";
        AddBasicAttack();
    }
}