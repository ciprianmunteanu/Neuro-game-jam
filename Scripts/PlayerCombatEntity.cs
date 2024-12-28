using Godot;
using System.Diagnostics;
using System.Linq;

public partial class PlayerCombatEntity : CombatEntity
{
    private bool IsPlayersTurn = false;
    private TurnManager turn;

    public override void TakeTurn(TurnManager turnManager)
    {
        turn = turnManager;
        IsPlayersTurn = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (turn != null && IsPlayersTurn && @event is InputEventMouseButton mouseEvent && mouseEvent.IsPressed() && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            var spaceState = GetWorld2D().DirectSpaceState;
            var mousePos = GetGlobalMousePosition();
            var query = new PhysicsPointQueryParameters2D() { Position = mousePos, CollideWithAreas = true };
            var results = spaceState.IntersectPoint(query);
            foreach(var result in results)
            {
                if (result.Any())
                {
                    var col = result["collider"].Obj as Area2D;
                    if (col.GetParent() is CombatEntity combatEntity)
                    {
                        combatEntity.TakeDamage(1);
                        IsPlayersTurn = false;
                        turn.PassTurn();
                    }
                }
            }
        }
    }

}
