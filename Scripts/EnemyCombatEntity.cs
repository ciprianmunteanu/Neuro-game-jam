using Godot;
using System;
using System.Diagnostics;

public partial class EnemyCombatEntity : CombatEntity
{
    private AnimatedSprite2D attackAnimation;

    private void CreateAnimation()
    {
        attackAnimation = new AnimatedSprite2D();
        attackAnimation.SpriteFrames = GD.Load<SpriteFrames>("res://Assets/Attack_animation/AttackAnimation.tres");
        attackAnimation.Position = PlayerCombatEntity.Instance.Position;
        attackAnimation.Scale = new Vector2(4, 4);
        GetTree().Root.AddChild(attackAnimation);
        attackAnimation.AnimationLooped += OnAnimationFinished;
    }

    public override void TakeTurn()
    {
        if(attackAnimation == null)
        {
            CreateAnimation();
        }
        attackAnimation.Show();
        attackAnimation.Play();
    }

    private void OnAnimationFinished()
    {
        PlayerCombatEntity.Instance.TakeDamage(1);
        attackAnimation.Pause();
        attackAnimation.Hide();
        TurnManager.Instance.PassTurn();
    }
}
