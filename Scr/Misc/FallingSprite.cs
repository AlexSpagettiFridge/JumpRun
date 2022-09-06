using Godot;

namespace JumpRun.Scr.Misc
{
    public class FallingSprite : Sprite
    {
        public Vector2 Momentum;
        public float RotationSpeed = 0;

        public override void _Process(float delta)
        {
            Momentum.y += GeneralConstants.Gravity * delta;
            Position += Momentum * delta;
            Rotate(RotationSpeed * delta);
        }
    }
}