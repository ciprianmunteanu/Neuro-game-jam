using System.Collections.Generic;


/// <summary>
/// PLACEHOLDER
/// </summary>
public class Sword : Item
{
    public Sword() : base(new CombatEntityStats() { AttackDamage = 20 }, "Sword", ItemType.WEAPON)
    {
        var slashSkill = new CombatAction() { Name = "Slash", Cooldown = 2};
        var damageCombatAction = new DamageCombatAction() { DamageMultiplier = 2 };
        Skills.Add(slashSkill);
    }
}

/// <summary>
/// PLACEHOLDER
/// </summary>
public class Armor : Item
{
    public Armor() : base(new CombatEntityStats() { MaxHealth = 20, CurrentHealth = 20 }, "Armor", ItemType.ARMOR)
    {
        var slashSkill = new CombatAction() { Name = "Slash", Cooldown = 2 };
        var damageCombatAction = new DamageCombatAction() { DamageMultiplier = 2 };
        Skills.Add(slashSkill);
    }
}

// ==================== WEAPONS ====================

public class Harpoon : Item
{
    public Harpoon() : base(new CombatEntityStats() { AttackDamage = 20 }, "Harpoon", ItemType.WEAPON)
    {
        SpritePath = "res://Assets/Weapon.png";

        // TODO this is supposed to use 2 actions
        var s1 = new CombatAction() { Name = "Shoot", Cooldown = 3, ActionCost = 2 };
        s1.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 4f });
        Skills.Add(s1);

        var s2 = new CombatAction() { Name = "Reel in", Cooldown = 2 };
        s2.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 0, Duration = 1 } });
        Skills.Add(s2);
    }
}

public class BananaRum : Item
{
    public BananaRum() : base(new CombatEntityStats() { AttackDamage = 15, MaxHealth = 10, CurrentHealth = 10 }, "Banana rum", ItemType.WEAPON)
    {
        var s1 = new CombatAction() { Name = "Chug", Cooldown = 1 };
        s1.CombatActionEffects.Add(new HealingCombatAction() { HealingAmount = 5 });
        s1.CombatActionEffects.Add(new CleanseCombatAction());
        s1.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 1.2, Duration = 2 } });
        Skills.Add(s1);

        var s2 = new CombatAction() { Name = "Throw", Cooldown = 2 };
        s2.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 1.5 });
        s2.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 0.7, Duration = 2 } });
        Skills.Add(s2);
    }
}

public partial class DroneSummon : EnemyCombatEntity
{
    public DroneSummon() : base(new CombatEntityStats() { AttackDamage = 10, MaxHealth = 10, CurrentHealth = 10, Speed = 1})
    {
        AddBasicAttack();
    }
}
public class Drones : Item
{
    public Drones() : base(new CombatEntityStats() { AttackDamage = 10, MaxHealth = 10, CurrentHealth = 10, Speed = 10 }, "Drone controller", ItemType.WEAPON)
    {
        var s1 = new CombatAction() { Name = "Call drone", Cooldown = 1 };
        s1.CombatActionEffects.Add(new SummonCombatAction() { SummonCombatEnityType = typeof(DroneSummon) });
        Skills.Add(s1);
    }
}

// ==================== ARMOR ====================

public class RobotBody : Item
{
    public RobotBody() : base(new CombatEntityStats() { AttackDamage = 10, MaxHealth = 20, CurrentHealth = 20 }, "Crazy fucking robot body", ItemType.ARMOR)
    {
        var s1 = new CombatAction() { Name = "I don't need anybody", Cooldown = 5 };
        s1.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 1.5, DefenseAmp = 1.25, Duration = 2 } });
        Skills.Add(s1);

        var s2 = new CombatAction() { Name = "Destroy you", Cooldown = 2, ActionCost = 2 };
        //s2.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 4 });
        // TODO somehow make it check for buffs and remove the buff after use
        // until then, this is a generic damage skill
        s2.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 2 });
        Skills.Add(s2);
    }
}

public class ClownOutfit : Item
{
    public ClownOutfit() : base(new CombatEntityStats() { MaxHealth = 30, CurrentHealth = 30 }, "Clown outfit", ItemType.ARMOR)
    {
        // TODO this is supposed to remove actions, figure out later how to do that
        var s1 = new CombatAction() { Name = "Clowning around", Cooldown = 3 };
        s1.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 0, Duration = 1 } });
        Skills.Add(s1);

        // TODO this is supposed to be AOE def down
        var s2 = new CombatAction() { Name = "Honk", Cooldown = 2 };
        s2.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DefenseAmp = 0.66, Duration = 2 } });
        Skills.Add(s2);
    }
}


public static class AllItems
{
    public static List<Item> ItemsList { get; private set; } = new List<Item>()
    {
        new Harpoon(),
        new BananaRum(),
        new Drones(),
        new RobotBody(),
        new ClownOutfit()
    };
}