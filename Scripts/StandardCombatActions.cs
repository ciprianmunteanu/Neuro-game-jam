// These things are meant to be stateless for the most part

public class DamageCombatAction : CombatAction
{
    public double DamageMultiplier { get; set; } = 1;

    protected override void DoEffect(CombatEntity user, CombatEntity target)
    {
        double userDamageMod = 1;
        foreach(var effect in user.CombatEffects)
        {
            userDamageMod *= effect.DamageAmp;
        }
        double targetDefenseMod = 1;
        foreach (var effect in target.CombatEffects)
        {
            targetDefenseMod *= effect.DefenseAmp;
        }

        target.TakeDamage(user.Stats.AttackDamage * DamageMultiplier * userDamageMod / targetDefenseMod);
    }
}

/// <summary>
/// This is for buffs and debuffs
/// </summary>
public class ApplyEffectCombatAction: CombatAction
{
    public CombatEffect Effect { get; set; } = new CombatEffect();

    protected override void DoEffect(CombatEntity user, CombatEntity target)
    {
        target.CombatEffects.Add(Effect);
    }
}

/// <summary>
/// In this case, the target would be the thing to summon, and we assume it's already summoned, but hidden?
/// </summary>
public class SummonCombatAction : CombatAction
{
    protected override void DoEffect(CombatEntity user, CombatEntity target)
    {
        target.Show();
    }
}

public class HealingCombatAction : CombatAction
{
    public double HealingAmount { get; set; } = 0;

    protected override void DoEffect(CombatEntity user, CombatEntity target)
    {
        target.Stats.CurrentHealth += HealingAmount;
        if(target.Stats.CurrentHealth > target.Stats.MaxHealth)
        {
            target.Stats.CurrentHealth = target.Stats.MaxHealth;
        }
    }
}