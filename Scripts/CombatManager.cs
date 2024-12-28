using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class CombatManager
{
    private static IOrderedEnumerable<CombatEntity> m_combatEntities;
    private static int crtCombatEntityIndex = 0;

    public static void StartCombat(List<CombatEntity> combatEntities)
    {
        if (!combatEntities.Any())
        {
            throw new Exception("Can't start combat with 0 entities");
        }

        m_combatEntities = combatEntities.OrderByDescending(c => c.Speed);

        crtCombatEntityIndex = -1;
        PassTurn();
    }

    public static void PassTurn()
    {
        crtCombatEntityIndex += 1;
        if (crtCombatEntityIndex >= m_combatEntities.Count())
        {
            crtCombatEntityIndex = 0;
        }

        m_combatEntities.ElementAt(crtCombatEntityIndex).TakeTurn();
    }
}
