using System.Collections.Generic;

public class Sword : Item
{
    public Sword() : base(new CombatEntityStats() { AttackDamage = 20 }, "Sword", ItemType.WEAPON)
    {
        var slashSkill = new DamageCombatAction() { Name = "Slash", Cooldown = 2, Damage = 200 };
        Skills.Add(slashSkill);
    }
}

public class Armor : Item
{
    public Armor() : base(new CombatEntityStats() { MaxHealth = 20, CurrentHealth = 20 }, "Armor", ItemType.ARMOR)
    {
        var slashSkill = new DamageCombatAction() { Name = "Body Slam", Cooldown = 1, Damage = 100 };
        Skills.Add(slashSkill);
    }
}

public static class AllItems
{
    public static List<Item> ItemsList { get; private set; } = new List<Item>()
    {
        new Sword(),
        new Armor()
    };
}