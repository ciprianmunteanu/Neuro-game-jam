// These things are meant to be stateless for the most part

using Godot;
using System;
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

public class SummonCombatAction : ICombatActionEffect
{
    public Type SummonCombatEnityType { get; set; }

    public void DoEffect(CombatEntity user, CombatEntity target)
    {
        var summon = Activator.CreateInstance(SummonCombatEnityType) as CombatEntity;
        summon.IsEnemy = user.IsEnemy;
        int nrOfOtherFriendlySummons = CombatManager.CombatEntities.FindAll(ce => ce.IsSummon && ce.IsEnemy == summon.IsEnemy).Count;
        if(nrOfOtherFriendlySummons >= CombatEncounterProvider.MAX_NR_SUMMONS)
        {
            // we reached max
            summon.Hide();
            summon.QueueFree();
            return;
        }

        user.AddChild(summon);
        Vector2 pos;
        if(summon.IsEnemy)
        {
            pos = CombatEncounterProvider.EnemySummonPositions[0];
        }
        else
        {
            pos = CombatEncounterProvider.PlayerSummonPositions[0];
        }
        summon.GlobalPosition = pos;
        summon.IsSummon = true;

        CombatManager.CombatEntities.Add(summon);
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