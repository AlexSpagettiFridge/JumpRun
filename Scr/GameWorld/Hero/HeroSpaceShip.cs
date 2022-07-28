using Godot;

namespace JumpRun.Scr.GameWorld.Hero
{
    public class HeroSpaceShip : KinematicPlatformer, IHero
    {
        [Export]
        private PackedScene psHero;
        private const float angularSpeed = 75, boostSpeed = 240;

        public override void _Ready()
        {
            gravityMultiplier = 0.25f;
            maxSpeed = 90;
            friction = 800;
            overFriction = 900;
        }

        public override void _PhysicsProcess(float delta)
        {
            float rotationInput = Input.GetAxis("gm_left", "gm_right");
            Rotate(Mathf.Deg2Rad(rotationInput * angularSpeed * delta));
            if (Input.IsActionPressed("gm_jump"))
            {
                momentum += new Vector2(0, -boostSpeed * delta).Rotated(Rotation);
            }
            base._PhysicsProcess(delta);
            if (IsOnWall())
            {
                momentum.x = -momentum.x;
            }
            if (IsOnCeiling())
            {
                momentum.y = -momentum.x;
            }
        }

        private void OnTimerTimout()
        {
            ChangeInto(psHero.Instance<Node2D>());
        }
    }
}