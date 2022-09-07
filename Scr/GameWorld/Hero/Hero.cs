using System;
using Godot;
using JumpRun.Scr.Misc;
using JumpRun.Scr.Visuals;

namespace JumpRun.Scr.GameWorld.Hero
{
    public class Hero : KinematicPlatformer, IHero
    {
        [Export]
        private PackedScene psThrowStuff = null;
        [Export]
        private NodePath npHeroSprite = null;
        private HeroSprite heroSprite;
        private const float JumpSpeed = 220, MoveAcceleration = 1000, ThrowHop = 200, CoyoteTime = 0.1f, AirControl = 0.5f
        , SpinControl = 0.1f, StompSpeed = 440, StompRicochet = 0.33f, DuckDash = 175, UnduckHop = 50, ThrowObjectSpeed = 180,
        AimHoldTime = 0.25f, AimSpeed = 3.5f, AimMaxDegrees = 80, MaxAimFloatTime = 2.5f;
        private bool didJump = false, didThrow = false, isSpinning = false, isStomping = false, isDucking = false, isAiming = false;

        private static bool initialized = false;
        private HeroReference heroReference;
        private float aimTime;
        public HeroReference HRef { get => heroReference; set => heroReference = value; }
        public event EventHandler<RegisterCurrentHeroArgs> SetNewCurrentHero;
        public float? AimAngle
        {
            get
            {
                if (aimTime <= AimHoldTime)
                {
                    return null;
                }
                return Mathf.Deg2Rad(Mathf.Sin((aimTime - AimHoldTime) * AimSpeed) * AimMaxDegrees);
            }
        }

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
                        if (!didThrow && !isAiming)
                        {
                            isAiming = true;
                        }
                    }
                }
            }
            if (isAiming)
            {
                aimTime += delta;
                if (aimTime > AimHoldTime)
                {
                    IsFrozen = true;
                }
                if (Input.IsActionJustReleased("gm_jump") || aimTime > MaxAimFloatTime)
                {
                    float throwAngle = AimAngle == null ? 0 : (float)AimAngle;

                    Momentum.y = Mathf.Min(-ThrowHop, Momentum.y);
                    ThrowStuff throwObject = psThrowStuff.Instance<ThrowStuff>();
                    GetParent().AddChild(throwObject);
                    throwObject.Position = Position + new Vector2(0, 4);
                    throwObject.Momentum = new Vector2(0, ThrowObjectSpeed).Rotated(throwAngle);
                    didThrow = true;
                    IsFrozen = false;
                    isAiming = false;
                    aimTime = 0;
                }

            }
            //Stomping/Ducking
            if (Input.IsActionJustPressed("gm_duck"))
            {
                if (airTime > CoyoteTime)
                {
                    isStomping = true;
                    Momentum.y = Mathf.Max(Momentum.y, StompSpeed);
                    isSpinning = false;
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
            if (!isDucking && !IsFrozen)
            {
                Momentum.x += moveDir * MoveAcceleration * delta * (isOnFloor ? 1 : isSpinning ? SpinControl : AirControl);
            }
            heroSprite.Animation = isSpinning ? "Spin" : "Idle";
            heroSprite.IsDucking = isDucking;
        }

        public void OnJustLanded()
        {
            didJump = false;
            didThrow = false;
            isSpinning = false;
            if (isStomping)
            {
                isStomping = false;
                Momentum.y *= -StompRicochet;
                isSpinning = true;
            }
        }

        protected override void OnWallCollision()
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

    }
}