using System.Collections.Generic;
using Godot;

namespace JumpRun.Scr.GameWorld
{
    public class Pulseplosion : Area2D
    {
        private Timer lifeTimer;
        private List<Node> pushedNodes = new List<Node>();

        public Pulseplosion(float radius = 16)
        {
            CollisionShape2D colShape = new CollisionShape2D();
            CircleShape2D circle = new CircleShape2D();
            circle.Radius = radius;
            colShape.Shape = circle;
            AddChild(colShape);
            Connect("body_entered", this, nameof(OnBodyEntered));

            lifeTimer = new Timer();
            AddChild(lifeTimer);
            lifeTimer.Connect("timeout", this, nameof(OnTimeout));
        }

        public override void _EnterTree()
        {
            CallDeferred(nameof(DeferredTimerStart));
        }

        private void DeferredTimerStart()
        {
            lifeTimer.Start(0.1f);
        }

        private void OnBodyEntered(Node body)
        {
            if (pushedNodes.Contains(body)) { return; }
            if (body is Hero.Hero hero)
            {
                Vector2 direction = GlobalPosition.DirectionTo(hero.GlobalPosition);
                float distance = GlobalPosition.DistanceTo(hero.GlobalPosition);
                hero.ApplyCenteredPulse(direction * (110 - distance * 1.5f) * new Vector2(3f, 1));
            }
            pushedNodes.Add(body);
        }

        private void OnTimeout()
        {
            QueueFree();
        }
    }
}