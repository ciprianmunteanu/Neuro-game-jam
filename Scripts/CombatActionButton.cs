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
            UiController.Instance.SetEnabled(!value);
            if (value)
            {
                UiController.Instance.SelectTargetPrompt.Show();
            }
            else
            {
                UiController.Instance.SelectTargetPrompt.Hide();
            }
        }
    }

    public CombatActionButton(CombatAction action)
    {
        combatAction = action;
        combatAction.OnCooldownChanged += OnActionCooldownChanged;
        Text = combatAction.Name;
    }

    private void OnActionCooldownChanged(int newCooldown)
    {
        if(newCooldown <= 0)
        {
            Disabled = false;
            Text = combatAction.Name;
        }
        else
        {
            Disabled = true;
            Text = $"{combatAction.Name} ({newCooldown})";
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
                        combatAction.Do(combatEntity, GetTree().Root);

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
        combatAction.OnCooldownChanged -= OnActionCooldownChanged;
    }
}
