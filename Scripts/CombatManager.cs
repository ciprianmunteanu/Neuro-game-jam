using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class CombatManager
{
    public static event Action OnCombatStart;
    public static event Action OnCombatClear;
    public static event Action OnNewTurn;

    private static List<CombatEntity> m_combatEntities;
    private static int crtCombatEntityIndex = 0;

    public static void StartCombat(List<CombatEntity> combatEntities)
    {
        if (!combatEntities.Any())
        {
            throw new Exception("Can't start combat with 0 entities");
        }

        m_combatEntities = combatEntities.OrderByDescending(c => c.Stats.Speed).ToList();

        crtCombatEntityIndex = -1;

        OnCombatStart.Invoke();

        PassTurn();
    }

    public static void PassTurn()
    {
        crtCombatEntityIndex += 1;
        if (crtCombatEntityIndex >= m_combatEntities.Count())
        {
            crtCombatEntityIndex = 0;

            OnNewTurn?.Invoke();
        }

        m_combatEntities.ElementAt(crtCombatEntityIndex).TakeTurn();
    }

    public static void RemoveEntityFromCombat(CombatEntity entity)
    {
        m_combatEntities.Remove(entity);
        if(m_combatEntities.Count() == 1)
        {
            // the player won
            OnCombatClear?.Invoke();
        }
    }
}
