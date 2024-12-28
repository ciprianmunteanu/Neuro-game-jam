using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class TurnManager
{
    public static TurnManager Instance { get; set; }

    private readonly IOrderedEnumerable<CombatEntity> m_combatEntities;
    private int crtCombatEntityIndex = 0;

    public TurnManager(List<CombatEntity> combatEntities)
    {
        if(!combatEntities.Any())
        {
            throw new Exception("Can't start combat with 0 entities");
        }

        m_combatEntities = combatEntities.OrderByDescending(c => c.Speed);

        Instance = this;
    }

    public void StartCombat()
    {
        crtCombatEntityIndex = -1;
        PassTurn();
    }

    public void PassTurn()
    {
        crtCombatEntityIndex += 1;
        if (crtCombatEntityIndex >= m_combatEntities.Count())
        {
            crtCombatEntityIndex = 0;
        }

        m_combatEntities.ElementAt(crtCombatEntityIndex).TakeTurn();
    }
}
