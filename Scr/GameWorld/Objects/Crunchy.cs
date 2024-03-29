using System;
using Godot;
using JumpRun.Scr.GameWorld.Common;
using JumpRun.Scr.GameWorld.Hero;

namespace JumpRun.Scr.GameWorld.Objects
{
    public class Crunchy : Area2D
    {
        [Export]
        private NodePath[] npSprites = null;
        [Export]
        private NodePath npAnimationPlayer = null;
        [Export]
        private PackedScene psParticles;

        public override void _Ready()
        {
            int frame = new Random().Next(0, 4);
            foreach (NodePath npSprite in npSprites)
            {
                AnimatedSprite sprite = GetNode<AnimatedSprite>(npSprite);
                sprite.Frame = frame;
            }
            GetNode<AnimationPlayer>(npAnimationPlayer).Play("Idle");
        }

        private void OnBodyEntered(Node body)
        {
            if (body is IHero)
            {
                GameController.Current.OnCrunchyCollected();
                Node2D particles = psParticles.Instance<Node2D>();
                particles.Position = Position;
                GetParent().AddChild(particles);
                QueueFree();
            }
        }
    }
}