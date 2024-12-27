using Godot;

public partial class CombatEntity : Node2D
{
    [Export]
    public ProgressBar HealthBar { get; set; }

    [Export]
    public int Speed { get; set; }
    [Export]
    public int MaxHealth { get; set; }

    public int CurrentHealth { get; set; }

    public virtual void TakeTurn() { }

    public override void _Ready()
    {
        HealthBar.Value = CurrentHealth / MaxHealth;
        Speed = 1;
    }
}
