using Godot;

public partial class CombatEntity : Node2D
{
    [Export]
    public ProgressBar HealthBar { get; set; }

    [Export]
    public int Speed { get; set; }
    [Export]
    public float MaxHealth { get; set; }

    public float CurrentHealth { get; set; }

    public virtual void TakeTurn() { }

    public override void _Ready()
    {
        HealthBar.Value = CurrentHealth / MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        HealthBar.Value = CurrentHealth / MaxHealth;
    }
}
