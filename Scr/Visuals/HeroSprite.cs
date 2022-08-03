using System.Collections.Generic;
using Godot;

namespace JumpRun.Scr.Visuals
{
    [Tool]
    public class HeroSprite : Node2D
    {
        [Export]
        private SpriteFrames frames = null;
        private int animationLength = 1;
        private string currentAnimation = "Idle";
        public float Frame = 0;
        public string Animation
        {
            get => currentAnimation;
            set
            {
                if (value == currentAnimation) { return; }
                currentAnimation = value;
                animationLength = frames.GetFrameCount(currentAnimation);
                Frame = 0;
            }
        }

        private const float lookMax = 2;
        private List<string> includesEyes = new List<string> { "Idle" };
        private bool isDucking = false;

        public bool IsDucking { get => isDucking; set => isDucking = value; }

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

        public override void _Process(float delta)
        {
            Frame += delta;
            while (Frame > animationLength)
            {
                Frame -= animationLength;
            }
        }

        public override void _Draw()
        {
            if (frames == null) { return; }
            Vector2 pupilOffset = new Vector2(Mathf.Round(lookDirection.x * lookMax), Mathf.Round(lookDirection.y * lookMax));
            Vector2 offCenter = -frames.GetFrame(eyeStateName, 0).GetSize() / 2;
            if (includesEyes.Contains(currentAnimation) && !isDucking)
            {
                DrawTexture(frames.GetFrame(eyeStateName, 0), offCenter, Modulate);
                DrawTexture(frames.GetFrame(eyeStateName, 1), offCenter + pupilOffset, Modulate);
            }
            Vector2 offCenter2 = -frames.GetFrame(currentAnimation, 0).GetSize() / 2;
            if (isDucking) { offCenter2.y += 5; }
            DrawTexture(frames.GetFrame(currentAnimation, (int)Frame), offCenter2);
        }
    }
}