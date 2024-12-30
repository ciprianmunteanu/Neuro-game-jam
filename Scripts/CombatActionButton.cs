using Godot;
using System;
using System.Linq;

public partial class CombatActionButton : Button
{
    private readonly CombatAction combatAction;
    private bool m_isSelectingTarget = false;
    private bool IsSelectingTarget {
        get => m_isSelectingTarget;
        set
        {
            m_isSelectingTarget = value;
            if (value)
            {
                UiController.Instance.SelectTargetPrompt.Show();
                UiController.Instance.CombatMenu.Hide();
            }
            else
            {
                UiController.Instance.SelectTargetPrompt.Hide();
                UiController.Instance.CombatMenu.Show();
            }
        }
    }

    public CombatActionButton(CombatAction action)
    {
        combatAction = action;
        Text = combatAction.Name;
    }


    public void RefreshButtonState()
    {
        if(combatAction.CheckIfUsable(PlayerCombatEntity.Instance))
        {
            Disabled = false;
            Text = combatAction.Name;
        }
        else
        {
            Disabled = true;
            Text = combatAction.Name;
            if(combatAction.RemainingCooldown > 0)
            {
                Text += $" ({combatAction.RemainingCooldown})";
            }
        }
    }

    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    protected void OnButtonPressed()
    {
        IsSelectingTarget = true;
        
    }

    public override void _Input(InputEvent @event)
    {
        if (IsSelectingTarget && @event is InputEventMouseButton mouseEvent && mouseEvent.IsPressed() && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            var spaceState = GetWorld2D().DirectSpaceState;
            var mousePos = GetGlobalMousePosition();
            var query = new PhysicsPointQueryParameters2D() { Position = mousePos, CollideWithAreas = true };
            var results = spaceState.IntersectPoint(query);
            foreach (var result in results)
            {
                if (result.Any())
                {
                    var col = result["collider"].Obj as Area2D;
                    if (col.GetParent() is CombatEntity combatEntity)
                    {
                        combatAction.Do(PlayerCombatEntity.Instance, combatEntity, GetTree().Root);

                        UiController.Instance.RefreshSkillButtonState();

                        IsSelectingTarget = false;
                    }
                }
            }
        }
    }


    /// <summary>
    /// A manual version of Dispose
    /// </summary>
    public void Cleanup()
    {
    }
}
