using System.Collections.Generic;
using Godot;

namespace JumpRun.Scr.Visuals
{
    [Tool]
    public class HeroSprite : AnimatedSprite
    {
        private const float lookMax = 4;
        private List<string> includesEyes = new List<string> { "Idle" };
        [Export]
        public Vector2 LookDirection
        {
            get => lookDirection;
            set
            {
                lookDirection = value;
                if (lookDirection.DistanceTo(Vector2.Zero) > 1)
                {
                    lookDirection = lookDirection.Normalized();
                }
                Update();
            }
        }
        private Vector2 lookDirection = new Vector2();
        private string eyeStateName = "eyes";

        public override void _Draw()
        {
            Vector2 pupilOffset = new Vector2(Mathf.Round(lookDirection.x * lookMax), Mathf.Round(lookDirection.y * lookMax));
            Vector2 offCenter = -Frames.GetFrame(eyeStateName, 0).GetSize() / 2;
            if (includesEyes.Contains(Animation))
            {
                DrawTexture(Frames.GetFrame(eyeStateName, 0), offCenter, Modulate);
                DrawTexture(Frames.GetFrame(eyeStateName, 1), offCenter + pupilOffset, Modulate);
            }
            Vector2 offCenter2 = -Frames.GetFrame(Animation, 0).GetSize() / 2;
            DrawTexture(Frames.GetFrame(Animation, Frame), offCenter2);
        }
    }
}