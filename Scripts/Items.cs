﻿using System.Collections.Generic;


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
    public Armor() : base(new CombatEntityStats() { MaxHealth = 50, CurrentHealth = 50 }, "Armor", ItemType.ARMOR)
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
        SpritePath = "res://Assets/Harpoon.png";

        var s1 = new CombatAction() { Name = "Shoot", Cooldown = 3, ActionCost = 2 , AnimationResourcePath = "res://Assets/Weapon_animations/Harpoon/ShootAnimation.tres", Description = "Deals 300% AD damage" };
        s1.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 3f });
        Skills.Add(s1);

        var s2 = new CombatAction() { Name = "Reel in", Cooldown = 2, AnimationResourcePath = "res://Assets/Weapon_animations/Harpoon/ReelInAnimation.tres", Description = "Reduces the target's damage to 0 for 1 turn"};
        s2.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 0, Duration = 1 } });
        Skills.Add(s2);
    }
}

public class BananaRum : Item
{
    public BananaRum() : base(new CombatEntityStats() { AttackDamage = 15, MaxHealth = 25, CurrentHealth = 25 }, "Banana rum", ItemType.WEAPON)
    {
        SpritePath = "res://Assets/Rum.png";

        var s1 = new CombatAction() { Name = "Chug", Cooldown = 1, AnimationResourcePath = "res://Assets/Weapon_animations/Rum/ChugAnimation.tres", Description = "Heals 5HP, cleanses debuffs and reduces incoming damage by 30% for 2 turns", ShouldTargetOpponent = false };
        s1.CombatActionEffects.Add(new HealingCombatAction() { HealingAmount = 5 });
        s1.CombatActionEffects.Add(new CleanseCombatAction());
        s1.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 1.2, Duration = 2 } });
        Skills.Add(s1);

        var s2 = new CombatAction() { Name = "Throw", Cooldown = 2, AnimationResourcePath = "res://Assets/Weapon_animations/Rum/Throw.tres", Description = "Deals 70% AD damage, reduces the target's damage by 30% for 2 turns" };
        s2.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 1.5 });
        s2.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 0.7, Duration = 2 } });
        Skills.Add(s2);
    }
}

public partial class DroneSummon : EnemyCombatEntity
{
    public DroneSummon() : base(new CombatEntityStats() { AttackDamage = 20, MaxHealth = 50, CurrentHealth = 50, Speed = 1})
    {
        SpriteResourcePath = "res://Assets/Drone.png";
        AddBasicAttack();
    }
}
public class Drones : Item
{
    public Drones() : base(new CombatEntityStats() { AttackDamage = 10, MaxHealth = 25, CurrentHealth = 25, Speed = 10 }, "Drone controller", ItemType.WEAPON)
    {
        SpritePath = "res://Assets/DroneRemote.png";
        var s1 = new CombatAction() { Name = "Call drone", Cooldown = 1, AnimationResourcePath = "res://Assets/Weapon_animations/Drone/SummonAnimation.tres", Description = "Summons a drone that fights for you. The drone has 20 AD, 20 HP and 1 speed" };
        s1.CombatActionEffects.Add(new SummonCombatAction() { SummonCombatEnityType = typeof(DroneSummon) });
        Skills.Add(s1);
    }
}

// ==================== ARMOR ====================

public class RobotBody : Item
{
    public RobotBody() : base(new CombatEntityStats() { AttackDamage = 10, MaxHealth = 50, CurrentHealth = 50 }, "Crazy fucking robot body", ItemType.ARMOR)
    {
        SpritePath = "res://Assets/RobotBody.png";

        var s1 = new CombatAction() { Name = "I don't need anybody", Cooldown = 5, AnimationResourcePath = "res://Assets/Weapon_animations/RobotBody/AnybodyAnimation.tres", Description = "Increases target's damage and defense by 50% for 2 turns.", ShouldTargetOpponent = false };
        s1.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 1.5, DefenseAmp = 1.5, Duration = 2 } });
        Skills.Add(s1);

        var s2 = new CombatAction() { Name = "Destroy you", Cooldown = 2, ActionCost = 2, AnimationResourcePath = "res://Assets/Weapon_animations/RobotBody/DestroyAnimation.tres", Description = "Deals 300% AD damage" };
        //s2.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 4 });
        // TODO somehow make it check for buffs and remove the buff after use
        // until then, this is a generic damage skill
        s2.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 3 });
        Skills.Add(s2);
    }
}

public class ClownOutfit : Item
{
    public ClownOutfit() : base(new CombatEntityStats() { MaxHealth = 75, CurrentHealth = 75 }, "Clown outfit", ItemType.ARMOR)
    {
        SpritePath = "res://Assets/Clown.png";

        // TODO this is supposed to remove actions, figure out later how to do that
        var s1 = new CombatAction() { Name = "Clowning around", Cooldown = 3, AnimationResourcePath = "res://Assets/Weapon_animations/ClownSuit/ClowningAnimation.tres", Description = "Reduces the target's damage to 0 for 1 turn" };
        s1.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DamageAmp = 0, Duration = 1 } });
        Skills.Add(s1);

        // TODO this is supposed to be AOE def down
        var s2 = new CombatAction() { Name = "Honk", Cooldown = 3, AnimationResourcePath = "res://Assets/Weapon_animations/ClownSuit/HonkAnimation.tres", Description = "Reduces the target's defense by 33% for 3 turns" };
        s2.CombatActionEffects.Add(new ApplyEffectCombatAction() { Effect = new CombatEffect() { DefenseAmp = 0.66, Duration = 3 } });
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