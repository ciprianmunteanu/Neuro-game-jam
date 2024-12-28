using Godot;
using System;
using System.Diagnostics;

public partial class EnemyCombatEntity : CombatEntity
{
    private AnimatedSprite2D attackAnimation;

    private void CreateAnimation()
    {
        attackAnimation = new AnimatedSprite2D();
        AddChild(attackAnimation);
        attackAnimation.SpriteFrames = GD.Load<SpriteFrames>("res://Assets/Attack_animation/AttackAnimation.tres");
        attackAnimation.GlobalPosition = PlayerCombatEntity.Instance.Position;
        attackAnimation.GlobalScale = new Vector2(4, 4);
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
