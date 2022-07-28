using System;
using Godot;
using JumpRun.Scr.Misc;
using JumpRun.Scr.Visuals;

namespace JumpRun.Scr.GameWorld.Hero
{
    public class Hero : KinematicPlatformer, IHero
    {
        [Export]
        private PackedScene psGlovjectile = null;
        [Export]
        private NodePath npHeroSprite = null;
        private HeroSprite heroSprite;
        private const float JumpSpeed = 220, MoveAcceleration = 1000, GloveHop = 110, CoyoteTime = 0.1f, AirControl = 0.5f
        , SpinControl = 0.1f, StompSpeed = 440, StompRicochet = 0.33f;
        private bool didJump = false, didStop = false, didPunch = false, isSpinning = false, isStomping = false;

        public void ApplyCenteredPulse(Vector2 pulse)
        {
            momentum += pulse;
        }

        public override void _Ready()
        {
            heroSprite = GetNode<HeroSprite>(npHeroSprite);
            Connect(nameof(JustLanded), this, nameof(OnJustLanded));
            maxSpeed = 120;
            friction = 700;
            overFriction = 450;
        }

        public override void _Process(float delta)
        {
            heroSprite.LookDirection = momentum / (maxSpeed * 1.2f);
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);

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
                        didPunch = true;
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
            gravityMultiplier = (isSpinning ? 0.5f : 1);
            //Horizontal Movement
            float moveDir = Input.GetAxis("gm_left", "gm_right");
            momentum.x += moveDir * MoveAcceleration * delta * (isOnFloor ? 1 : isSpinning ? SpinControl : AirControl);
            heroSprite.Animation = isSpinning ? "Spin" : "Idle";
        }

        public void OnJustLanded()
        {
            didJump = false;
            didStop = false;
            didPunch = false;
            isSpinning = false;
            if (isStomping)
            {
                isStomping = false;
                momentum.y *= -StompRicochet;
                isSpinning = true;
            }
        }
    }
}