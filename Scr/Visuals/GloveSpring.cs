using Godot;

namespace JumpRun.Scr.Visuals
{
    [Tool]
    public class GloveSpring : Node2D
    {
        [Export]
        private Texture texture = null;
        private NodePath npConnection = null;
        private Node2D connection;

        public void SetConnector(Node2D connector)
        {
            connection = connector;
        }

        public override void _EnterTree()
        {
            Connect("draw", this, nameof(_Draw));
        }

        public override void _Process(float delta)
        {
            Update();
        }

        public override void _Draw()
        {
            Vector2 point = Engine.EditorHint ? Vector2.Zero : connection.GlobalPosition;
            float distance = GlobalPosition.DistanceTo(point);
            float angle = GlobalPosition.AngleToPoint(point) - GlobalRotation;
            DrawSetTransform(Vector2.Zero, angle + Mathf.Pi / 2, new Vector2(1, 1));
            Rect2 rect = new Rect2(new Vector2(-2.5f, 0), new Vector2(5, distance));
            DrawTextureRect(texture, rect, false);
        }
    }
}