using Godot;
using System;

/// <summary>
/// This is basically a skill and everything related to that skill: effect, animation, icon, cooldown etc.
/// This is also used for basic actions like the basic attack
/// </summary>
public abstract class CombatAction
{
    public event Action OnActionDone;
    public event Action<int> OnCooldownChanged;

    public string Name { get; set; } = "CombatAction";
    public string AnimationResourcePath { get; set; } = "res://Assets/Attack_animation/AttackAnimation.tres";
    public int Cooldown { get; set; } = 1;

    private int remainingCooldown = 0;
    protected int RemainingCooldown 
    {
        get => remainingCooldown;
        set
        {
            remainingCooldown = value;
            OnCooldownChanged?.Invoke(value);
        }
    }

    public CombatAction()
    {
        CombatManager.OnNewTurn += () => RemainingCooldown--;
        //CombatManager.OnCombatStart += () => RemainingCooldown = 0;
    }

    public void Do(CombatEntity target, Node VfxParent)
    {
        var animation = new AnimatedSprite2D();
        VfxParent.AddChild(animation);
        animation.SpriteFrames = GD.Load<SpriteFrames>(AnimationResourcePath);
        animation.GlobalPosition = target.Position;
        animation.GlobalScale = new Vector2(4, 4);
        
        animation.AnimationLooped += () =>
        {
            DoEffect(target);

            RemainingCooldown = Cooldown;

            animation.Stop();
            animation.Hide();
            animation.QueueFree();
            OnActionDone?.Invoke();
        };


        animation.Show();
        animation.Play();
    }

    protected abstract void DoEffect(CombatEntity target);
}