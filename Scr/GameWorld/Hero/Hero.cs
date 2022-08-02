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
        , SpinControl = 0.1f, StompSpeed = 440, StompRicochet = 0.33f, DuckDash = 1000, UnduckHop = 50;
        private bool didJump = false, didStop = false, didPunch = false, isSpinning = false, isStomping = false, isDucking = false;

        private static bool initialized = false;
        private HeroReference heroReference;
        public HeroReference HRef { get => heroReference; set => heroReference = value; }
        public event EventHandler<RegisterCurrentHeroArgs> SetNewCurrentHero;

        public Hero()
        {
            if (!initialized) { heroReference = new HeroReference(this); }
        }

        public void ApplyCenteredPulse(Vector2 pulse)
        {
            Momentum += pulse;
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
            heroSprite.LookDirection = Momentum / (maxSpeed * 1.2f);
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            float moveDir = Input.GetAxis("gm_left", "gm_right");

            if (IsOnWall())
            {
                if (isSpinning)
                {
                    Momentum.x *= -0.8f;
                }
                else
                {
                    Momentum.x = 0;
                }
            }
            bool isOnFloor = IsOnFloor();

            //Jumping/Punching
            if (Input.IsActionJustPressed("gm_jump"))
            {
                if (isDucking)
                {
                    if (Momentum.x == 0)
                    {
                        Momentum.x = moveDir * DuckDash;
                    }
                }
                else
                {
                    if (!didJump && airTime <= CoyoteTime)
                    {
                        Momentum.y = -JumpSpeed;
                        didJump = true;
                    }
                    else
                    {
                        if (!didPunch)
                        {
                            isSpinning = true;
                            Momentum.y = Mathf.Min(-GloveHop, Momentum.y);
                            Glovjectile glove = psGlovjectile.Instance<Glovjectile>();
                            GetParent().AddChild(glove);
                            glove.Position = Position + new Vector2(0, 24);
                            didPunch = true;
                        }
                    }
                }
            }
            if (Input.IsActionJustReleased("gm_jump") && Momentum.y < 0 && !didStop)
            {
                Momentum.y /= 2;
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
                        Momentum.y = Mathf.Max(Momentum.y, StompSpeed);
                    }
                }
                else
                {
                    MoveAndCollide(new Vector2(0, -4));
                    isDucking = true;
                }
            }
            if (Input.IsActionJustReleased("gm_duck") && isDucking)
            {
                isDucking = false;
                if (isOnFloor) { Momentum.y = -UnduckHop; }
            }
            gravityMultiplier = (isSpinning ? 0.5f : 1);
            //Horizontal Movement
            if (!isDucking)
            {
                Momentum.x += moveDir * MoveAcceleration * delta * (isOnFloor ? 1 : isSpinning ? SpinControl : AirControl);
            }
            heroSprite.Animation = isSpinning ? "Spin" : "Idle";
            heroSprite.IsDucking = isDucking;
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
                Momentum.y *= -StompRicochet;
                isSpinning = true;
            }
        }


    }
}