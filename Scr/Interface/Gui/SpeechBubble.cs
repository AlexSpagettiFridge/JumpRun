using Godot;

namespace JumpRun.Scr.Interface.Gui
{
    [Tool]
    public class SpeechBubble : Control
    {
        [Export]
        public Vector2 SpeakerPoint = Vector2.Zero;
        private Texture texture = ResourceLoader.Load<Texture>("res://Gfx/Ui/SpeechBubble.png");
        private static Color lineColor = new Color("#ff91a7"), contentColor = new Color("#e1b0d2");

        public override void _Draw()
        {
            Vector2 cornerSize = new Vector2(4, 4);
            //Top Left
            DrawTextureRectRegion(texture, new Rect2(Vector2.Zero, cornerSize), new Rect2(Vector2.Zero, cornerSize));
            //Top Right
            DrawTextureRectRegion(texture, new Rect2(RectSize.x - cornerSize.x, 0, cornerSize), new Rect2(12, 0, cornerSize));
            //Bottom Left
            DrawTextureRectRegion(texture, new Rect2(0, RectSize.y - cornerSize.y, cornerSize), new Rect2(0, 12, cornerSize));
            //Bottom Right
            DrawTextureRectRegion(texture, new Rect2(RectSize - cornerSize, cornerSize), new Rect2(12, 12, cornerSize));
            //Left Side
            DrawTextureRectRegion(texture, new Rect2(0, cornerSize.y, cornerSize.x, RectSize.y - cornerSize.y * 2)
            , new Rect2(0, cornerSize.y, cornerSize.x, 8));
            //Top Side
            DrawTextureRectRegion(texture, new Rect2(cornerSize.x, 0, RectSize.x - cornerSize.x * 2, cornerSize.y)
            , new Rect2(cornerSize.x, 0, 8, cornerSize.y));
            //Right Side
            DrawTextureRectRegion(texture, new Rect2(RectSize.x - cornerSize.x, cornerSize.y
            , cornerSize.x, RectSize.y - cornerSize.y * 2)
            , new Rect2(12, cornerSize.y, cornerSize.x, 8));
            //Bottom Side
            DrawTextureRectRegion(texture, new Rect2(cornerSize.x, RectSize.y - cornerSize.y
            , RectSize.x - cornerSize.x * 2, cornerSize.y)
            , new Rect2(cornerSize.x, 12, 8, cornerSize.y));

            //Content
            DrawTextureRectRegion(texture, new Rect2(cornerSize, RectSize - cornerSize * 2), new Rect2(cornerSize, 8, 8));

            //SpeechBubble Pointer
            Vector2 rectCenter = RectGlobalPosition + RectSize / 2;
            Vector2 speakerection = rectCenter.DirectionTo(SpeakerPoint);
            float speakerAngle = rectCenter.AngleToPoint(SpeakerPoint);
            Vector2[] polyPoints =
            {
                GetLineRectIntersectionPoint(rectCenter + Vector2.Up.Rotated(speakerAngle)*8, speakerection),
                GetLineRectIntersectionPoint(rectCenter + Vector2.Down.Rotated(speakerAngle)*8, speakerection),
                SpeakerPoint - RectPosition
            };
            DrawPolygon(polyPoints, new Color[] { contentColor });

            for (int i = 0; i < 2; i++)
            {
                DrawLine(polyPoints[i], SpeakerPoint - RectPosition, lineColor, 1.01f, true);
            }
        }

        private Vector2 GetLineRectIntersectionPoint(Vector2 lineStart, Vector2 lineDirection)
        {
            Vector2 rectCenter = RectGlobalPosition + RectSize / 2;
            float horizontalDistance = Mathf.Inf;
            float verticalDistance = Mathf.Inf;
            if (lineDirection.x != 0)
            {
                horizontalDistance = ((rectCenter - lineStart).x * Mathf.Sign(lineDirection.x) + RectSize.x / 2) / Mathf.Abs(lineDirection.x);
            }
            if (lineDirection.y != 0)
            {
                verticalDistance = ((rectCenter - lineStart).y * Mathf.Sign(lineDirection.y) + RectSize.y / 2) / Mathf.Abs(lineDirection.y);
            }

            lineStart -= RectPosition;

            if (horizontalDistance <= verticalDistance)
            {
                return lineStart + lineDirection * horizontalDistance;
            }
            else
            {
                return lineStart + lineDirection * verticalDistance;
            }
        }
    }
}