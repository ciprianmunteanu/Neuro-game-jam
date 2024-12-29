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
        var basicAttackCA = new CombatAction() { Cooldown = 1, Name = "Attack" };
        basicAttackCA.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 1 });
        CombatActions.Add(basicAttackCA);
    }
}