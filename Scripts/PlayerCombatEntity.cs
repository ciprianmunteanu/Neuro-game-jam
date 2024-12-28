using Godot;
using System.Diagnostics;
using System.Linq;

public partial class PlayerCombatEntity : CombatEntity
{
    public override void TakeTurn()
    {
        Debug.WriteLine("Player's turn");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.IsPressed() && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            var spaceState = GetWorld2D().DirectSpaceState;
            // use global coordinates, not local to node
            // TODO use IntersectPoint instead
            var mousePos = GetGlobalMousePosition();
            var query = PhysicsRayQueryParameters2D.Create(mousePos, mousePos + new Vector2(0, 1));
            query.CollideWithAreas = true;
            query.HitFromInside = true;
            var result = spaceState.IntersectRay(query);
            if(result.Any())
            {
                var col = result["collider"].Obj as Area2D;
                if (col.GetParent() is CombatEntity combatEntity)
                {
                    combatEntity.TakeDamage(1);
                }
            }
        }
    }

}
