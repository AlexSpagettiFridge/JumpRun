using System;
using Godot;
using JumpRun.Scr.Misc;

namespace JumpRun.Scr.GameWorld.Hero
{
    public class ThrowStuff : KinematicBody2D
    {
        [Export]
        private NodePath npSprite;
        public Vector2 Momentum;
        private float rotationSpeed;
        private AnimatedSprite sprite;

        public override void _Ready()
        {
            sprite = GetNode<AnimatedSprite>(npSprite);
            rotationSpeed = Mathf.Deg2Rad(new Random().Next(-360, 360));
            sprite.Frame = new Random().Next(0, sprite.Frames.GetFrameCount(sprite.Animation));
        }
        public override void _Process(float delta)
        {
            KinematicCollision2D collision = MoveAndCollide(Momentum * delta);
            sprite.Rotate(rotationSpeed * delta);
            if (collision != null)
            {
                FallingSprite fallingSprite = new FallingSprite();
                fallingSprite.Texture = sprite.Frames.GetFrame(sprite.Animation, sprite.Frame);
                fallingSprite.Transform = Transform;
                fallingSprite.Rotation = sprite.Rotation;
                fallingSprite.RotationSpeed = Mathf.Deg2Rad(new Random().Next(-720, 720));
                fallingSprite.Momentum = Momentum.Bounce(collision.Normal);
                GetParent().AddChild(fallingSprite);
                QueueFree();
            }
        }
    }
}