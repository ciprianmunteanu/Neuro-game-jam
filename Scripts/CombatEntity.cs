using Godot;
using System.Collections.Generic;

/// <summary>
/// Bufffs and debuffs
/// </summary>
public record CombatEffect
{
    public int Duration { get; set; } = 1;
    public double DamageAmp { get; set; } = 1;
    public double DefenseAmp { get; set; } = 1;

    public bool IsBuff => DamageAmp > 1 || DefenseAmp > 1;
}

public partial class CombatEntity : Node2D
{
    public CombatEntityStats Stats { get; init; }
    public string SpriteResourcePath { get; set; } = "res://Assets/Player.png";
    public List<CombatEffect> CombatEffects = new();
    public bool IsEnemy = true;
    public bool IsSummon = false;

    public Sprite2D WeaponSprite { get; set; }
    public Sprite2D ArmorSprite { get; set; }

    private ProgressBar HealthBar { get; set; }
    private Sprite2D Sprite { get; set; }
    private Area2D Collider { get; set; }
    private CollisionShape2D ColliderShape { get; set; }

    public CombatEntity(CombatEntityStats stats)
    {
        Stats = stats;
    }

    public virtual void TakeTurn() 
    {
    }

    public void OnTurnEnd()
    {
        CombatEffects.RemoveAll(effect => --effect.Duration <= 0);
    }

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

        WeaponSprite = new Sprite2D()
        {
            Position = new Vector2(50, 0),
            Scale = new Vector2(2, 2)
        };
        AddChild(WeaponSprite);

        ArmorSprite = new Sprite2D()
        {
            Scale = new Vector2(4,4)
        };
        AddChild(ArmorSprite);
    }

    public void TakeDamage(double damage)
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
