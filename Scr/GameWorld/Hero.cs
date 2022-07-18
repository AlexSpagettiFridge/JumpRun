using System;
using Godot;
using JumpRun.Scr.Misc;
using JumpRun.Scr.Visuals;

namespace JumpRun.Scr.GameWorld
{
    public class Hero : KinematicBody2D
    {
        [Export]
        private PackedScene psGlovjectile = null;
        [Export]
        private NodePath npHeroSprite = null;
        private HeroSprite heroSprite;
        private const float JumpSpeed = 440, MoveAcceleration = 2000, Friction = 1400, OverFriction = 700, MaxSpeed = 240,
        GloveHop = 220, CoyoteTime = 0.1f, AirControl = 0.5f, SpinControl = 0.1f, StompSpeed = 880, StompRicochet = 0.33f;
        private Vector2 momentum = new Vector2();
        private bool didJump = false, didStop = false, didPunch = false, isSpinning = false, isStomping = false;
        private float airTime = 0;

        public void ApplyCenteredPulse(Vector2 pulse)
        {
            momentum += pulse;
        }

        public override void _Ready()
        {
            heroSprite = GetNode<HeroSprite>(npHeroSprite);
        }

        public override void _Process(float delta)
        {
            heroSprite.LookDirection = momentum / (MaxSpeed * 1.2f);
        }

        public override void _PhysicsProcess(float delta)
        {
            MoveAndSlide(momentum, new Vector2(0, -1));
            if (IsOnCeiling())
            {
                momentum.y = Mathf.Max(0, momentum.y);
            }
            if (IsOnWall())
            {
                if (isSpinning)
                {
                    momentum.x *= -0.8f;
                }
                else
                {
                    momentum.x = 0;
                }
            }
            bool isOnFloor = IsOnFloor();
            //Gravity
            if (!isOnFloor)
            {
                airTime += delta;
                momentum.y += GeneralConstants.Gravity * delta * (isSpinning ? 0.5f : 1);
            }
            else
            {
                if (airTime > 0 && isStomping)
                {
                    isStomping = false;
                    momentum.y *= -StompRicochet;
                    isSpinning = true;
                }
                else
                {
                    didJump = false;
                    didStop = false;
                    didPunch = false;
                    isSpinning = false;
                    airTime = 0;
                    momentum.y = Mathf.Min(0, momentum.y);
                }
            }
            //Jumping/Punching
            if (Input.IsActionJustPressed("gm_jump"))
            {
                if (!didJump && airTime <= CoyoteTime)
                {
                    momentum.y = -JumpSpeed;
                    didJump = true;
                }
                else
                {
                    if (!didPunch)
                    {
                        isSpinning = true;
                        momentum.y = Mathf.Min(-GloveHop, momentum.y);
                        Glovjectile glove = psGlovjectile.Instance<Glovjectile>();
                        GetParent().AddChild(glove);
                        glove.Position = Position + new Vector2(0, 24);
                        glove.Init(this);
                        didPunch = true;
                        DampedSpringJoint2D springJoint = new DampedSpringJoint2D();
                        springJoint.Length = 30;
                        springJoint.RestLength = 8;
                        springJoint.NodeA = GetPath();
                        springJoint.NodeB = glove.GetPath();
                        AddChild(springJoint);

                    }
                }
            }
            if (Input.IsActionJustReleased("gm_jump") && momentum.y < 0 && !didStop)
            {
                momentum.y /= 2;
                didStop = true;
            }
            //Stomping/Ducking
            if (Input.IsActionJustPressed("gm_duck"))
            {
                if (airTime > CoyoteTime)
                {
                    if (!isSpinning)
                    {
                        isStomping = true;
                        momentum.y = Mathf.Max(momentum.y, StompSpeed);
                    }
                }
            }
            //Horizontal Movement
            float moveDir = Input.GetAxis("gm_left", "gm_right");
            momentum.x += moveDir * MoveAcceleration * delta * (isOnFloor ? 1 : isSpinning ? SpinControl : AirControl);
            if (isOnFloor)
            {
                momentum.x = Util.CalculateFriction(momentum.x, 0, Friction * delta);

            }
            if (Mathf.Abs(momentum.x) > MaxSpeed)
            {
                momentum.x = Util.CalculateFriction(momentum.x, 0, Mathf.Abs(momentum.x) / MaxSpeed * OverFriction * delta);
            }

            heroSprite.Animation = isSpinning ? "Spin" : "Idle";
        }
    }
}