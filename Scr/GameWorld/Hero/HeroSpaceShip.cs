using Godot;
using JumpRun.Scr.GameWorld.Common;
using JumpRun.Scr.Interface.Gui;

namespace JumpRun.Scr.GameWorld.Hero
{
    public class HeroSpaceShip : RigidBody2D, IHero
    {
        [Export]
        private PackedScene psHero, psFuelGauge;
        private const float angularSpeed = 550, boostSpeed = 300, crashMultiplier = 0.02f, boostCost = 2;
        private FuelInfo fuelInfo;
        public float Fuel = 10;
        private Vector2 lastVelocity;

        private HeroReference heroReference;
        public HeroReference HRef { get => heroReference; set => heroReference = value; }

        public override void _Ready()
        {
            fuelInfo = psFuelGauge.Instance<FuelInfo>();
            fuelInfo.TrackedHero = this;
            GameController.Current.Gui.AddChild(fuelInfo);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Fuel <= 0)
            {
                heroReference.ChangeHero(psHero.Instance<Hero>());
                return;
            }
            float rotationInput = Input.GetAxis("gm_left", "gm_right");
            AngularVelocity += Mathf.Deg2Rad(rotationInput * angularSpeed * delta);
            if (Input.IsActionPressed("gm_jump"))
            {
                LinearVelocity += new Vector2(0, -boostSpeed * delta).Rotated(Rotation);
                Fuel -= delta * boostCost;
            }
            lastVelocity = LinearVelocity;
        }

        public void OnBodyEntered(Node body)
        {
            Fuel -= lastVelocity.Length() * crashMultiplier;
        }

        public override void _ExitTree()
        {
            fuelInfo.QueueFree();
        }
    }
}