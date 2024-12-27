using Godot;
using System.Diagnostics;

public partial class PlayerCombatEntity : CombatEntity
{
    public override void TakeTurn()
    {
        Debug.WriteLine("Player's turn");
    }
}
