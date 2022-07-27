using System.Collections.Generic;
using Godot;

namespace JumpRunPlugin
{
    [Tool]
    public class CameraRail : Node2D
    {
        private List<Line> lines = new List<Line>();

#if TOOLS
        public override void _Input(InputEvent @event)
        {
            GD.Print("Handling Putput");
            if (@event is InputEventMouseButton inputEventMouseButton)
            {
                if (inputEventMouseButton.ButtonIndex == 1 && inputEventMouseButton.Pressed)
                {
                    GD.Print(GetLocalMousePosition());
                    GetTree().SetInputAsHandled();
                }
            }
        }
#endif

        public override void _Draw()
        {
            if (Engine.EditorHint)
            {
                Color color = new Color(0, 0, 1, 0.75f);
                foreach (Line line in lines)
                {
                    DrawCircle(line.Start, 10, color);
                    DrawLine(line.Start, line.End, color, 4);
                }
            }
        }

        private struct Line
        {
            public Vector2 Start;
            public Vector2 End;
        }
    }
}