using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class EnemyCombatEntity : CombatEntity
{
    public List<CombatAction> CombatActions { get; set; } = new();

    private Random random = new();
    private CombatAction chosenCombatAction;

    public EnemyCombatEntity(CombatEntityStats stats) : base(stats)
    {
    }

    public override void TakeTurn()
    {
        chosenCombatAction = CombatActions.ElementAt(random.Next(CombatActions.Count));
        chosenCombatAction.OnActionDone += OnCombatActionDone;
        chosenCombatAction.Do(this, PlayerCombatEntity.Instance, this);
    }

    private void OnCombatActionDone()
    {
        OnTurnEnd();
        CombatManager.PassTurn();
        chosenCombatAction.OnActionDone -= OnCombatActionDone;
    }

    protected override void OnDeath()
    {
        CombatManager.RemoveEntityFromCombat(this);
        
        // TODO death animation
        Hide();

        QueueFree();
    }

    protected void AddBasicAttack()
    {
        var basicAttackCA = new CombatAction() { Cooldown = 1 };
        basicAttackCA.CombatActionEffects.Add(new DamageCombatAction() { DamageMultiplier = 1 });
        CombatActions.Add(basicAttackCA);
    }
}
