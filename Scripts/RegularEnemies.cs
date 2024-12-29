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
        CombatActions.Add(new DamageCombatAction() { DamageMultiplier = 1 });
    }
}