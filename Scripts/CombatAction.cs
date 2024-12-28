using Godot;
using System;

public abstract class CombatAction
{
    public event Action OnActionDone;

    public void Do(CombatEntity target, Node VfxParent)
    {
        var animation = new AnimatedSprite2D();
        VfxParent.AddChild(animation);
        animation.SpriteFrames = GD.Load<SpriteFrames>("res://Assets/Attack_animation/AttackAnimation.tres");
        animation.GlobalPosition = target.Position;
        animation.GlobalScale = new Vector2(4, 4);
        animation.AnimationLooped += () => DoEffect(target);
        animation.AnimationLooped += () =>
        {
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