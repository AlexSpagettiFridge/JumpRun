using Godot;
using JumpRun.Scr.Misc;

namespace JumpRun.Scr.GameWorld
{
    public abstract class KinematicPlatformer : KinematicBody2D
    {
        [Signal]
        public delegate void JustLanded();
        public Vector2 Momentum;
        protected float airTime = 0, gravityMultiplier = 1;
        protected float friction = 700, maxSpeed = 120, overFriction = 450;

        public override void _PhysicsProcess(float delta)
        {
            MoveAndSlide(Momentum, new Vector2(0, -1));
            if (IsOnCeiling())
            {
                Momentum.y = Mathf.Max(0, Momentum.y);
            }
            bool isOnFloor = IsOnFloor();
            //Gravity
            if (!isOnFloor)
            {
                airTime += delta;
                Momentum.y += GeneralConstants.Gravity * delta * gravityMultiplier;
            }
            else
            {
                if (airTime > 0)
                {
                    EmitSignal(nameof(JustLanded));
                }
                else
                {
                    Momentum.y = Mathf.Min(0, Momentum.y);
                }
                airTime = 0;
                Momentum.x = Util.CalculateFriction(Momentum.x, 0, friction * delta);
            }
            if (Mathf.Abs(Momentum.x) > maxSpeed)
            {
                Momentum.x = Util.CalculateFriction(Momentum.x, 0, Mathf.Abs(Momentum.x) / maxSpeed * overFriction * delta);
            }
        }


    }
}