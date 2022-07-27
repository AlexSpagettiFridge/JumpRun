using Godot;
using JumpRun.Scr.Misc;

namespace JumpRun.Scr.GameWorld
{
    public class KinematicPlatformer : KinematicBody2D
    {
        [Signal]
        public delegate void JustLanded();
        protected Vector2 momentum;
        protected float airTime = 0, gravityMultiplier = 1;
        protected float friction = 700, maxSpeed = 120, overFriction = 450;

        public override void _PhysicsProcess(float delta)
        {
            MoveAndSlide(momentum, new Vector2(0, -1));
            if (IsOnCeiling())
            {
                momentum.y = Mathf.Max(0, momentum.y);
            }
            bool isOnFloor = IsOnFloor();
            //Gravity
            if (!isOnFloor)
            {
                airTime += delta;
                momentum.y += GeneralConstants.Gravity * delta * gravityMultiplier;
            }
            else
            {
                if (airTime > 0)
                {
                    EmitSignal(nameof(JustLanded));
                }
                else
                {
                    momentum.y = Mathf.Min(0, momentum.y);
                }
                airTime = 0;
                momentum.x = Util.CalculateFriction(momentum.x, 0, friction * delta);
            }
            if (Mathf.Abs(momentum.x) > maxSpeed)
            {
                momentum.x = Util.CalculateFriction(momentum.x, 0, Mathf.Abs(momentum.x) / maxSpeed * overFriction * delta);
            }
        }
    }
}