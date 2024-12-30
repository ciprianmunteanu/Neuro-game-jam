using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class CombatManager
{
    public static event Action OnCombatStart;
    public static event Action OnCombatClear;
    public static event Action OnNewTurn;

    public static List<CombatEntity> CombatEntities;
    private static int crtCombatEntityIndex = 0;

    private static bool isCombatOver = false;

    public static void StartCombat(List<CombatEntity> combatEntities)
    {
        isCombatOver = false;
        if (!combatEntities.Any())
        {
            throw new Exception("Can't start combat with 0 entities");
        }

        CombatEntities = combatEntities.OrderByDescending(c => c.Stats.Speed).ToList();

        crtCombatEntityIndex = -1;

        OnCombatStart.Invoke();

        PassTurn();
    }

    public static void PassTurn()
    {
        if (!isCombatOver && CombatEntities.Any(c => c.IsEnemy) == false)
        {
            isCombatOver = true;
            OnCombatClear?.Invoke();
            foreach (var ent in CombatEntities)
            {
                ent.Hide();
                ent.QueueFree();
            }
        }
        else
        {
            crtCombatEntityIndex += 1;
            if (crtCombatEntityIndex >= CombatEntities.Count())
            {
                crtCombatEntityIndex = 0;

                // re-order in between turns in case speed changed or creatures were summoned
                CombatEntities = CombatEntities.OrderByDescending(c => c.Stats.Speed).ToList();

                OnNewTurn?.Invoke();
            }

            CombatEntities.ElementAt(crtCombatEntityIndex).TakeTurn();
        }
    }

    public static void RemoveEntityFromCombat(CombatEntity entity)
    {
        CombatEntities.Remove(entity);

        if (!isCombatOver && CombatEntities.Any(c => c.IsEnemy) == false)
        {
            isCombatOver = true;
            OnCombatClear?.Invoke();
            foreach(var ent in CombatEntities)
            {
                ent.Hide();
                ent.QueueFree();
            }
        }
    }
}
