using Godot;
using JumpRun.Scr.Visuals;

namespace JumpRun.Scr.GameWorld
{

    public class Glovjectile : KinematicBody2D
    {
        [Export]
        private Hero hero;
        private float speed = 220;

        public override void _PhysicsProcess(float delta)
        {
            KinematicCollision2D collision = MoveAndCollide(new Vector2(0, speed) * delta);
            if (collision != null)
            {
                Pulseplosion pulseplosion = new Pulseplosion();
                pulseplosion.GlobalPosition = GlobalPosition;
                GetParent().AddChild(pulseplosion);
                QueueFree();
            }
        }

        public void Init(Node2D source)
        {
            if (source is Hero hero)
            {
                this.hero = hero;
            }
        }

        private void OnTimerTimeout()
        {
            QueueFree();
        }
    }
}