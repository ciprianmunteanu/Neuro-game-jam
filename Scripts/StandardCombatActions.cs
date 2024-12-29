// These things are meant to be stateless for the most part

using System.Collections.Generic;

public class DamageCombatAction : ICombatActionEffect
{
    public double DamageMultiplier { get; set; } = 1;

    public void DoEffect(CombatEntity user, CombatEntity target)
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
public class ApplyEffectCombatAction: ICombatActionEffect
{
    public CombatEffect Effect { get; set; } = new CombatEffect();

    public void DoEffect(CombatEntity user, CombatEntity target)
    {
        target.CombatEffects.Add(Effect);
    }
}

/// <summary>
/// In this case, the target would be the thing to summon, and we assume it's already summoned, but hidden?
/// </summary>
public class SummonCombatAction : ICombatActionEffect
{
    public void DoEffect(CombatEntity user, CombatEntity target)
    {
        target.Show();
    }
}

public class HealingCombatAction : ICombatActionEffect
{
    public double HealingAmount { get; set; } = 0;

    public void DoEffect(CombatEntity user, CombatEntity target)
    {
        target.Stats.CurrentHealth += HealingAmount;
        if(target.Stats.CurrentHealth > target.Stats.MaxHealth)
        {
            target.Stats.CurrentHealth = target.Stats.MaxHealth;
        }
    }
}


public class CleanseCombatAction : ICombatActionEffect
{
    public void DoEffect(CombatEntity user, CombatEntity target)
    {
        target.CombatEffects.RemoveAll(ce => ce.IsBuff == false);
    }
}