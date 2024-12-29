using Godot;

public partial class CombatEntity : Node2D
{
    public CombatEntityStats Stats { get; init; }
    public string SpriteResourcePath { get; set; } = "res://Assets/Player.png";

    private ProgressBar HealthBar { get; set; }
    private Sprite2D Sprite { get; set; }
    private Area2D Collider { get; set; }
    private CollisionShape2D ColliderShape { get; set; }

    public CombatEntity(CombatEntityStats stats)
    {
        Stats = stats;
    }

    public virtual void TakeTurn() { }

    public override void _Ready()
    {
        Sprite = new Sprite2D()
        {
            Texture = GD.Load<Texture2D>(SpriteResourcePath),
            Scale = new Vector2(4, 4)
        };
        AddChild(Sprite);

        HealthBar = new ProgressBar()
        {
            MinValue = 0,
            MaxValue = 1,
            ShowPercentage = false,
            Position = new Vector2(-40, -100),
            Size = new Vector2(80,20),
            Value = Stats.CurrentHealth / Stats.MaxHealth
        };
        AddChild(HealthBar);

        Collider = new Area2D()
        {

        };
        AddChild(Collider);

        ColliderShape = new CollisionShape2D()
        {
            Shape = new CircleShape2D() { Radius = 100 }
        };
        Collider.AddChild(ColliderShape);
    }

    public void TakeDamage(float damage)
    {
        Stats.CurrentHealth -= damage;
        HealthBar.Value = Stats.CurrentHealth / Stats.MaxHealth;

        if (Stats.CurrentHealth <= 0)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath() { }
}
