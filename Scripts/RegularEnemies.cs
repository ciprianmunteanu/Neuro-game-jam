public partial class TrashMob : EnemyCombatEntity
{
    public TrashMob() : base(new CombatEntityStats()
    {
        MaxHealth = 10,
        CurrentHealth = 10,
        AttackDamage = 1,
        Speed = 1
    })
    {
        SpriteResourcePath = "res://Assets/Enemy.png";
        AddBasicAttack();
    }
}

public partial class DifficultTrashMob : EnemyCombatEntity
{
    public DifficultTrashMob() : base(new CombatEntityStats()
    {
        MaxHealth = 20,
        CurrentHealth = 20,
        AttackDamage = 3,
        Speed = 1
    })
    {
        SpriteResourcePath = "res://Assets/Enemy.png";
        AddBasicAttack();
    }
}