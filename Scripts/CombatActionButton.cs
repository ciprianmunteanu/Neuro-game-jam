using Godot;
using System.Linq;

public partial class CombatActionButton : Button
{
    private readonly ICombatAction combatAction;
    private bool isSelectingTarget = false;

    public CombatActionButton(ICombatAction action)
    {
        combatAction = action;
    }

    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    protected void OnButtonPressed()
    {
        // prompt to select targets
        UiController.Instance.SetEnabled(false);
        UiController.Instance.SelectTargetPrompt.Show();
        isSelectingTarget = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (isSelectingTarget && @event is InputEventMouseButton mouseEvent && mouseEvent.IsPressed() && mouseEvent.ButtonIndex == MouseButton.Left)
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
                        combatAction.Do(combatEntity);

                        UiController.Instance.CombatMenu.Hide();
                        UiController.Instance.SelectTargetPrompt.Hide();
                        isSelectingTarget = false;
                    }
                }
            }
        }
    }
}
