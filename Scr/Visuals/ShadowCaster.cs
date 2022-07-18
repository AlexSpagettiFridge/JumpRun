using Godot;

namespace JumpRun.Scr.Visuals
{
    public class ShadowCaster : RayCast2D
    {
        [Export]
        private Texture shadowTexture = null;

        public override void _Ready()
        {
            Connect("draw", this, nameof(_Draw));
        }

        public override void _Process(float delta)
        {
            Update();
        }

        public override void _Draw()
        {
            if (GetCollider() != null)
            {
                DrawSetTransform(GetCollisionPoint() - GlobalPosition, 0, new Vector2(1, 1));
                DrawTexture(shadowTexture, -shadowTexture.GetSize() / 2);
            }
        }
    }
}