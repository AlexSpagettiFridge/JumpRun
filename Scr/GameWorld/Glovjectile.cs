using Godot;

namespace JumpRun.Scr.GameWorld
{

    public class Glovjectile : KinematicBody2D
    {
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

        private void OnTimerTimeout()
        {
            QueueFree();
        }
    }
}