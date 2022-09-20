using Godot;

namespace JumpRun.Scr.GameWorld.Common
{
    public class GameCamera : Camera2D
    {
        public Vector2 TopLeft => GetCameraPosition()-(GetViewport().Size/2)*Zoom;
        public Vector2 BottomRight => GetCameraPosition()+(GetViewport().Size/2)*Zoom;
    }
}